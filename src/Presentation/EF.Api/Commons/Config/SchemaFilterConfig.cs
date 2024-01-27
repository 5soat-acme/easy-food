using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace EF.Api.Commons.Config;

public class SchemaFilterConfig: ISchemaFilter
{
    private readonly Type typeToIgnore;
    private readonly string propertyNameToIgnore;

    public SchemaFilterConfig(Type typeToIgnore, string propertyNameToIgnore)
    {
        this.typeToIgnore = typeToIgnore;
        this.propertyNameToIgnore = propertyNameToIgnore;
    }

    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        // if (context.Type == typeToIgnore)
        // {
            var propertyNameToIgnoreLower = propertyNameToIgnore.ToLowerInvariant();

            var propertyToRemove = schema.Properties.Keys
                .FirstOrDefault(name => name.ToLowerInvariant() == propertyNameToIgnoreLower);

            if (propertyToRemove != null)
            {
                schema.Properties.Remove(propertyToRemove);
            }
        // }
    }
}