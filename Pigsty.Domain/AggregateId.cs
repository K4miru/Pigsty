namespace Pigsty.Domain;

public class AggregateId : IEquatable<AggregateId>
{
    public Guid Value { get; private set; }

    public AggregateId() : this(Guid.NewGuid()) { }
    public AggregateId(Guid value)
    {
        if (value == Guid.Empty) { throw new InvalidAggregateIdException(value); }
        Value = value;
    }

    public static implicit operator Guid(AggregateId id) => id.Value;
    public static implicit operator AggregateId(Guid id) => new(id);

    public override string ToString() => Value.ToString();
    public override int GetHashCode() => Value.GetHashCode();
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals(obj as AggregateId);
    }

    public bool Equals(AggregateId? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Value.Equals(other.Value);
    }
}