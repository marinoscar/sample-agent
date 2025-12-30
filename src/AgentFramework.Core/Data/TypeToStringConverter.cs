using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentFramework.Core.Data
{
    /// <summary>
    /// Converts <see cref="Type"/> instances to and from their string representations for Entity Framework Core persistence.
    /// </summary>
    /// <remarks>
    /// This converter uses assembly-qualified names to ensure type resolution across assemblies.
    /// When reading from the database, it first attempts to resolve the type using <see cref="Type.GetType(string, bool)"/>,
    /// and if that fails, scans all loaded assemblies in the current <see cref="AppDomain"/>.
    /// </remarks>
    public sealed class TypeToStringConverter : ValueConverter<Type, string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeToStringConverter"/> class.
        /// </summary>
        public TypeToStringConverter()
            : base(
                type => type.AssemblyQualifiedName!,                 // to DB
                name => ResolveTypeOrThrow(name))                    // from DB
        { }

        /// <summary>
        /// Resolves a <see cref="Type"/> from its string representation.
        /// </summary>
        /// <param name="name">The string representation of the type (typically an assembly-qualified name).</param>
        /// <returns>The resolved <see cref="Type"/>.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the type cannot be resolved from the provided name.
        /// </exception>
        /// <remarks>
        /// This method first attempts standard type resolution using <see cref="Type.GetType(string, bool)"/>.
        /// If that fails, it scans all assemblies in the current <see cref="AppDomain"/> to locate the type.
        /// </remarks>
        private static Type ResolveTypeOrThrow(string name)
        {
            // Works for assembly-qualified names; fallback scan if needed
            var type = Type.GetType(name, throwOnError: false);

            if (type != null) return type;

            type = AppDomain.CurrentDomain.GetAssemblies()
                .Select(a => a.GetType(name, throwOnError: false))
                .FirstOrDefault(t => t != null);

            return type ?? throw new InvalidOperationException($"Cannot resolve Type from '{name}'.");
        }
    }
}
