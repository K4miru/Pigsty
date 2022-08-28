namespace Pigsty.Databases;

public interface IIdentity<T>
{
    T Id { get; }
}
