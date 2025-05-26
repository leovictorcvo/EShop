namespace Ordering.Domain.Abstractions;

public interface IEntity
{
    public DateTime? CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public string? LastModifiedBy { get; set; }
}

public interface IEntity<out TKey> : IEntity
{
    public TKey Id { get; }
}