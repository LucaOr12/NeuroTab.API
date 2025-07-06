using System.ComponentModel.DataAnnotations;

namespace neurotab_api.Models;

public class Content
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TabId { get; set; } //foreignKey
    
    [Required]
    [StringLength(5000)]
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Url { get; set; }//optional to reference links
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
}