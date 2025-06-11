using FloraApp.Model.Requests;
using FloraApp.Model.Responses;
using FloraApp.Model.SearchObjects;
using FloraApp.Services.Database;
using FloraApp.Services.Interfaces;
using MapsterMapper;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace FloraApp.Services.Services
{
    public class UserService : BaseService<UserResponse, UserSearchObject, User>, IUserService
    {
        // PBKDF2 parameters recommended by NIST
        private const int IterationCount = 10000;
        private const int NumBytesRequested = 32; // 256 bits
        private const int SaltSize = 16; // 128 bits
        
        private readonly IMapper _mapper;

        public UserService(FloraAppDbContext context, IMapper mapper) : base(context, mapper)
        {
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserResponse>> GetAllUsersAsync()
        {
            var users = await _context.Users.ToListAsync();
            return users.Select(MapToResponse);
        }

        public async Task<UserResponse?> GetUserByEmailAsync(string email)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
            return user != null ? MapToResponse(user) : null;
        }

        public async Task<UserResponse?> GetUserByUsernameAsync(string username)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);
            return user != null ? MapToResponse(user) : null;
        }

        public async Task<UserResponse> CreateUserAsync(UserUpsertRequest request)
        {
            // Check if username or email already exists
            var usernameExists = await _context.Users.AnyAsync(u => u.Username == request.Username);
            if (usernameExists)
            {
                throw new InvalidOperationException($"Username '{request.Username}' is already taken.");
            }
            
            var emailExists = await _context.Users.AnyAsync(u => u.Email == request.Email);
            if (emailExists)
            {
                throw new InvalidOperationException($"Email '{request.Email}' is already registered.");
            }

            // Create a new User entity from the request
            var user = _mapper.Map<User>(request);
            user.CreatedAt = DateTime.UtcNow;
            
            // Generate a cryptographically secure salt
            byte[] saltBytes = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            
            // Store the salt as Base64 string
            string salt = Convert.ToBase64String(saltBytes);
            user.PasswordSalt = salt;
            
            // Hash the password using PBKDF2
            user.PasswordHash = HashPassword(request.Password, saltBytes);
            
            // Add the user to the context
            _context.Users.Add(user);
            
            // Save changes with detailed error handling
            return await ExecuteDbOperationAsync(async () => {
                await _context.SaveChangesAsync();
                return MapToResponse(user);
            });
        }

        public async Task<UserResponse?> UpdateUserAsync(int id, UserUpsertRequest request)
        {
            var user = await _context.Users.FindAsync(id);
            
            if (user == null)
                return null;
                
            // Update user properties
            _mapper.Map(request, user);
            
            // Only update password if a new one is provided
            if (!string.IsNullOrEmpty(request.Password))
            {
                // Generate a new salt
                byte[] saltBytes = new byte[SaltSize];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(saltBytes);
                }
                
                // Store the salt as Base64 string
                string salt = Convert.ToBase64String(saltBytes);
                user.PasswordSalt = salt;
                
                // Hash the password using PBKDF2
                user.PasswordHash = HashPassword(request.Password, saltBytes);
            }
            
            // Save changes with detailed error handling
            return await ExecuteDbOperationAsync(async () => {
                await _context.SaveChangesAsync();
                return MapToResponse(user);
            });
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return false;

            _context.Users.Remove(user);
            
            // Save changes with detailed error handling
            return await ExecuteDbOperationAsync(async () => {
                await _context.SaveChangesAsync();
                return true;
            });
        }
        
      
        public async Task<UserResponse?> AuthenticateAsync(LoginRequest request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == request.Username);
                
            if (user == null)
                return null;
                
            // Convert stored salt from Base64 string to bytes
            byte[] saltBytes = Convert.FromBase64String(user.PasswordSalt);
            
            // Hash the provided password with the stored salt
            string hashedPassword = HashPassword(request.Password, saltBytes);
            
            // Check if the computed hash matches the stored hash
            if (hashedPassword != user.PasswordHash)
                return null;
                
            // Update last login time
            user.LastLoginAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            
            return MapToResponse(user);
        }
        
        protected override UserResponse MapToResponse(User user)
        {
            return _mapper.Map<UserResponse>(user);
        }
        
        private static string HashPassword(string password, byte[] salt)
        {
            // Using PBKDF2 with HMAC-SHA256
            string hashedPassword = Convert.ToBase64String(
                KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: IterationCount,
                    numBytesRequested: NumBytesRequested
                )
            );
            
            return hashedPassword;
        }
        
        private async Task<T> ExecuteDbOperationAsync<T>(Func<Task<T>> operation)
        {
            try
            {
                return await operation();
            }
            catch (DbUpdateException ex)
            {
                var innerException = ex.InnerException;
                string errorMessage = innerException?.Message ?? ex.Message;
                
                // Add more specific error handling based on the database provider if needed
                if (errorMessage.Contains("duplicate") || errorMessage.Contains("unique constraint"))
                {
                    throw new InvalidOperationException("A user with this username or email already exists.", ex);
                }
                
                throw new InvalidOperationException($"Database error occurred: {errorMessage}", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An error occurred: {ex.Message}", ex);
            }
        }
        
        public async Task<UserResponse> CreateTestUserAsync()
        {
            var random = new Random();
            var testUser = new UserUpsertRequest
            {
                FirstName = "Test",
                LastName = $"User{random.Next(1000, 9999)}",
                Email = $"test{random.Next(1000, 9999)}@example.com",
                Username = $"testuser{random.Next(1000, 9999)}",
                Password = "Test123!",
                PhoneNumber = $"+1{random.Next(1000000000, 2000000000)}",
                IsActive = true,
                IsAdmin = false
            };
            
            return await CreateUserAsync(testUser);
        }
        
        protected override IQueryable<User> AddFilter(IQueryable<User> query, UserSearchObject search)
        {
            if (!string.IsNullOrWhiteSpace(search?.Username))
            {
                query = query.Where(u => u.Username.Contains(search.Username));
            }
            
            if (!string.IsNullOrWhiteSpace(search?.Email))
            {
                query = query.Where(u => u.Email.Contains(search.Email));
            }
            
            if (!string.IsNullOrWhiteSpace(search?.FirstName))
            {
                query = query.Where(u => u.FirstName.Contains(search.FirstName));
            }
            
            if (!string.IsNullOrWhiteSpace(search?.LastName))
            {
                query = query.Where(u => u.LastName.Contains(search.LastName));
            }
            
            if (!string.IsNullOrWhiteSpace(search?.FTS))
            {
                query = query.Where(u => 
                    u.Username.Contains(search.FTS) || 
                    u.Email.Contains(search.FTS) || 
                    u.FirstName.Contains(search.FTS) || 
                    u.LastName.Contains(search.FTS)
                );
            }
            
            return query;
        }
       
    }
    
} 