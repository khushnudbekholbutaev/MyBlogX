namespace BlogPostify.Domain.Commons;

public abstract class Auditable<T> : BaseModel<T>
{
    public DateTime? UpdatedAt { get; set; }
    public DateTime CreatedAt { get; set; }
}
