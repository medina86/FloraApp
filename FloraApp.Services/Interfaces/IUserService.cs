using FloraApp.Model.Requests;
using FloraApp.Model.Responses;
using FloraApp.Model.SearchObjects;
using FloraApp.Services.Database;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FloraApp.Services.Interfaces
{
    public interface IUserService : IService<UserResponse, UserSearchObject>
    {
        Task<IEnumerable<UserResponse>> GetAllUsersAsync();
        Task<UserResponse?> GetUserByEmailAsync(string email);
        Task<UserResponse?> GetUserByUsernameAsync(string username);
        Task<UserResponse> CreateUserAsync(UserUpsertRequest request);
        Task<UserResponse?> UpdateUserAsync(int id, UserUpsertRequest request);
        Task<bool> DeleteUserAsync(int userId);
        Task<UserResponse?> AuthenticateAsync(LoginRequest request);
        Task<UserResponse> CreateTestUserAsync();
    }
} 