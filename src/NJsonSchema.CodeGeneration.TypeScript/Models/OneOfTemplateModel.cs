//-----------------------------------------------------------------------
// <copyright file="EnumTemplateModel.cs" company="NJsonSchema">
//     Copyright (c) Rico Suter. All rights reserved.
// </copyright>
// <license>https://github.com/RicoSuter/NJsonSchema/blob/master/LICENSE.md</license>
// <author>Rico Suter, mail@rsuter.com</author>
//-----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using NJsonSchema.CodeGeneration.Models;

namespace NJsonSchema.CodeGeneration.TypeScript.Models
{
    /// <summary>The TypeScript OneOf template model.</summary>
    public class OneOfTemplateModel
    {
        private readonly JsonSchema _schema;
        private readonly TypeScriptTypeResolver _resolver;
        private readonly TypeScriptGeneratorSettings _settings;

        /// <summary>Initializes a new instance of the <see cref="EnumTemplateModel" /> class.</summary>
        /// <param name="typeName">Name of the type.</param>
        /// <param name="schema">The schema.</param>
        /// <param name="resolver">The resolver.</param>
        /// <param name="settings">The settings.</param>
        public OneOfTemplateModel(string typeName, JsonSchema schema,
            TypeScriptTypeResolver resolver, TypeScriptGeneratorSettings settings)
        {
            _schema = schema;
            _resolver = resolver;
            _settings = settings;
            Name = typeName;
        }

        /// <summary>Gets the name of the enum.</summary>
        public string Name { get; }

        /// <summary>Gets a value indicating whether the enum has description.</summary>
        public bool HasDescription => !(_schema is JsonSchemaProperty) && !string.IsNullOrEmpty(_schema.Description);

        /// <summary>Gets the description.</summary>
        public string Description => ConversionUtilities.RemoveLineBreaks(_schema.Description);

        /// <summary>Gets a value indicating whether the export keyword should be added to all enums.</summary>
        public bool ExportTypes => _settings.ExportTypes;

        /// <summary>Gets the property extension data.</summary>
        public IDictionary<string, object> ExtensionData => _schema.ExtensionData;

        /// <summary>Gets the enum values.</summary>
        public List<EnumerationItemModel> Enums
        {
            get
            {
                
                var entries = new List<EnumerationItemModel>();
                for (int i = 0; i < _schema.OneOf.Count; i++)
                {
                    var value = _schema.OneOf.ElementAt(i);
                    if (value != null)
                    {
                        var resolvedType = _resolver.Resolve(value, false, "");
                        entries.Add(new EnumerationItemModel
                        {
                            Name = resolvedType,
                            Value = resolvedType,
                        });
                    }
                }
                return entries;
            }
        }
    }
}