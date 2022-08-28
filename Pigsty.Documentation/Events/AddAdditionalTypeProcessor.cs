using NJsonSchema;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace Pigsty.Documentation.Events;

internal sealed class AddAdditionalTypeProcessor<T> : IDocumentProcessor where T : class
{
    public void Process(DocumentProcessorContext context)
    {
        context.SchemaResolver.AddSchema(typeof(T), isIntegerEnumeration: false, JsonSchema.FromType<T>());
    }
}