using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace neurotab_api.Models;

public class Tab // container of thoughts
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; } //foreignKey
    
    [Required]
    [StringLength(500)]
    public string Title { get; set; } = string.Empty;

    public List<Content> Content { get; set; } = null!;//the actual idea content
    public string? Url { get; set; }//optional to reference stuff
    public string? Description { get; set; }
    public List<string> Tags { get; set; } = null!;// ["topic", "genre", "creative", "urgent"]
    public bool IsActive { get; set; } = true;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    //Navigation Properties
    [ValidateNever]
    public virtual User User { get; set; } = null!;
}