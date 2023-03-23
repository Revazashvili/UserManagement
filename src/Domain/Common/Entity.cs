namespace Domain.Common;

public abstract class Entity : IEntity
{
    public long Id { get; set; }

    protected Entity() { }

    protected Entity(long id) { Id = id; }

    public override bool Equals(object? obj)
    {
        if (obj is not Entity other)
            return false;
            
        if (ReferenceEquals(this, other))
            return false;
            
        if (Id.Equals(default) || other.Id.Equals(default))
            return false;

        return Id.Equals(other.Id);
    }
        
    public static bool operator ==(Entity a, Entity b) => a.Equals(b);
    public static bool operator !=(Entity a, Entity b) => !(a == b);
}