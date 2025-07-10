using System.ComponentModel.DataAnnotations;

namespace neurotab_api.Models;

public class Connection
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public Guid FromContentId { get; set; } //foreignKey
    public Guid ToContentId { get; set; } //foreignKey
    
    [StringLength(50)]
    public string ConnectionType { get; set; } = string.Empty;// "builds_on", "contradicts", "similar_to", "leads_to"

    public int Strength { get; set; } = 1;// 1-10, how strong the connection is
    public string? Notes { get; set; }
    public bool IsAiGenerated { get; set; }// Track if AI suggested this connection
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    //Navigation Properties
    public virtual Content FromContent { get; set; } = null!;
    public virtual Content ToContent { get; set; } = null!;
}