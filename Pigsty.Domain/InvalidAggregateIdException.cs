namespace Pigsty.Domain;

public class InvalidAggregateIdException : DomainException
{
    public override string Code => "invalid_aggregate_id";
    public InvalidAggregateIdException(Guid id) : base($"Invalid aggregate id {id}") { }
}