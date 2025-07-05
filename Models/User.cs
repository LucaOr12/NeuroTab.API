using System.ComponentModel.DataAnnotations;

namespace neurotab_api.Models;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty; // Full name from Google
    
    [Required]
    public string GoogleId { get; set; } = string.Empty; // Google's unique user ID
    
    public string? ProfilePictureUrl { get; set; } // Google profile picture
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginAt { get; set; }
    
    // Navigation property - this user's thoughts and ideas
    public virtual ICollection<Tab> Tabs { get; set; } = new List<Tab>();
}