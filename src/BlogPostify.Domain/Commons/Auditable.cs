namespace BlogPostify.Domain.Commons;

public abstract class Auditable<T> : BaseModel<T>
{
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.Now;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
}
