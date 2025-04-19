using System.ComponentModel.DataAnnotations.Schema;

public abstract class BaseEntity
{
    [Column("CreatedAt")] // Explicit column name mapping
    public DateTime CreatedAt { get; set; }

    [Column("UpdatedAt")] // Explicit column name mapping
    public DateTime UpdatedAt { get; set; }
}