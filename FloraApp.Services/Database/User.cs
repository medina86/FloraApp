using System;
using System.ComponentModel.DataAnnotations;

namespace FloraApp.Services.Database
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(100)]
        public string Username { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(256)]
        public string PasswordHash { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(128)]
        public string PasswordSalt { get; set; } = string.Empty;
        
        public bool IsActive { get; set; } = true;
        
        public bool IsAdmin { get; set; } = false;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? LastLoginAt { get; set; }
        
        [Phone]
        [MaxLength(20)]
        public string? PhoneNumber { get; set; }
    }
} 