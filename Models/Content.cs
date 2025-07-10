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
    
    //positions
    [Required]
    public float PositionX { get; set; } = 100;
    [Required]
    public float PositionY { get; set; } = 100;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    
    public virtual ICollection<Connection> OutGoingConnections { get; set; } = new List<Connection>();
    public virtual ICollection<Connection> IncomingConnections { get; set; } = new List<Connection>();
}