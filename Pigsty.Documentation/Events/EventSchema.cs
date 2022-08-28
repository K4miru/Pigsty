namespace Pigsty.Documentation.Events;

internal sealed record EventSchema(string Name, IEnumerable<EventFields> Fields) { }