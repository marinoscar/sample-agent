using Microsoft.Extensions.AI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AgentFramework.Core.Configuration
{
    /// <summary>
    /// Represents the configuration settings for an AI agent instance.
    /// This entity is designed for use with Entity Framework Core and contains all necessary 
    /// parameters to initialize, configure, and control an AI agent's behavior.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <strong>Entity Framework Usage:</strong>
    /// This class serves as an EF Core entity that persists agent configurations to a database.
    /// When used with EF Core, consider the following:
    /// </para>
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// <strong>Primary Key:</strong> The <see cref="Id"/> property serves as the primary key 
    /// and is auto-generated as a GUID string (format "N") in the constructor.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <strong>Required Fields:</strong> Properties like <see cref="Id"/>, <see cref="Provider"/>, 
    /// <see cref="Name"/>, and <see cref="ToolList"/> should be configured as required in your EF model 
    /// configuration since they have non-null defaults.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <strong>Type Serialization:</strong> The <see cref="ResponseFormat"/> property stores a 
    /// <see cref="Type"/> object which requires custom value conversion for database storage 
    /// (typically serialized as a string containing the type's assembly-qualified name).
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <strong>Default Values:</strong> All properties are initialized with sensible defaults 
    /// in the parameterless constructor, making this entity ready to use immediately after instantiation.
    /// </description>
    /// </item>
    /// </list>
    /// <para>
    /// <strong>Example EF Core Configuration:</strong>
    /// </para>
    /// <code>
    /// protected override void OnModelCreating(ModelBuilder modelBuilder)
    /// {
    ///     modelBuilder.Entity&lt;AgentConfiguration&gt;(entity =>
    ///     {
    ///         entity.HasKey(e => e.Id);
    ///         entity.Property(e => e.Id).IsRequired().HasMaxLength(32);
    ///         entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
    ///         entity.Property(e => e.Provider).IsRequired().HasMaxLength(100);
    ///         entity.Property(e => e.ResponseFormat)
    ///               .HasConversion&lt;TypeToStringConverter&gt;();
    ///     });
    /// }
    /// </code>
    /// </remarks>
    public class AgentConfiguration
    {
        /// <summary>
        /// Gets or sets the unique identifier for this agent configuration.
        /// This property serves as the primary key when used with Entity Framework Core.
        /// </summary>
        /// <remarks>
        /// <para>
        /// <strong>Default Value:</strong> A new GUID string in "N" format (32 hexadecimal digits with no hyphens).
        /// </para>
        /// <para>
        /// <strong>Database Considerations:</strong>
        /// </para>
        /// <list type="bullet">
        /// <item><description>Should be configured as a primary key in EF Core.</description></item>
        /// <item><description>Recommended max length: 32 characters (GUID without hyphens).</description></item>
        /// <item><description>Auto-generated in constructor; typically not set manually unless restoring from backup.</description></item>
        /// </list>
        /// </remarks>
        /// <value>A unique identifier string. Default: <c>Guid.NewGuid().ToString("N")</c></value>
        public string Id { get; set; }
        
        /// <summary>
        /// Gets or sets the AI provider name for this agent configuration.
        /// Identifies which AI service provider should be used to instantiate the agent.
        /// </summary>
        /// <remarks>
        /// <para>
        /// <strong>Default Value:</strong> "OpenAI"
        /// </para>
        /// <para>
        /// <strong>Supported Values:</strong> "OpenAI", "AzureOpenAI", "Anthropic", or other registered providers.
        /// </para>
        /// <para>
        /// <strong>Database Considerations:</strong>
        /// </para>
        /// <list type="bullet">
        /// <item><description>Should be configured as required (non-nullable) in EF Core.</description></item>
        /// <item><description>Recommended max length: 100 characters.</description></item>
        /// <item><description>Consider adding an index if frequently querying by provider.</description></item>
        /// </list>
        /// </remarks>
        /// <value>The provider name string. Default: <c>"OpenAI"</c></value>
        public string Provider { get; set; }
        
        /// <summary>
        /// Gets or sets the display name for this agent configuration.
        /// Used for identification in user interfaces and logging.
        /// </summary>
        /// <remarks>
        /// <para>
        /// <strong>Default Value:</strong> "Default Agent"
        /// </para>
        /// <para>
        /// <strong>Database Considerations:</strong>
        /// </para>
        /// <list type="bullet">
        /// <item><description>Should be configured as required (non-nullable) in EF Core.</description></item>
        /// <item><description>Recommended max length: 200 characters.</description></item>
        /// <item><description>Consider making this unique if agent names should be distinct.</description></item>
        /// </list>
        /// </remarks>
        /// <value>The agent name string. Default: <c>"Default Agent"</c></value>
        public string Name { get; set; }
        
        /// <summary>
        /// Gets or sets an optional description for this agent configuration.
        /// Provides additional context about the agent's purpose or capabilities.
        /// </summary>
        /// <remarks>
        /// <para>
        /// <strong>Default Value:</strong> <c>string.Empty</c>
        /// </para>
        /// <para>
        /// <strong>Database Considerations:</strong>
        /// </para>
        /// <list type="bullet">
        /// <item><description>Nullable property; can be null in the database.</description></item>
        /// <item><description>Recommended max length: 1000 characters or use nvarchar(max) for longer descriptions.</description></item>
        /// </list>
        /// </remarks>
        /// <value>An optional description string. Default: <c>string.Empty</c></value>
        public string? Description { get; set; }
        
        /// <summary>
        /// Gets or sets the AI model identifier to use for this agent.
        /// Specifies which specific model version or variant should be used by the provider.
        /// </summary>
        /// <remarks>
        /// <para>
        /// <strong>Default Value:</strong> "gpt-5-nano"
        /// </para>
        /// <para>
        /// <strong>Examples:</strong> "gpt-4", "gpt-3.5-turbo", "claude-3-opus", "gpt-5-nano", "gpt-5-mini"
        /// </para>
        /// <para>
        /// <strong>Database Considerations:</strong>
        /// </para>
        /// <list type="bullet">
        /// <item><description>Nullable property; can be null in the database.</description></item>
        /// <item><description>Recommended max length: 100 characters.</description></item>
        /// <item><description>Store as-is without validation; model availability may change over time.</description></item>
        /// </list>
        /// </remarks>
        /// <value>The model identifier string. Default: <c>"gpt-5-nano"</c></value>
        public string? Model { get; set; }
        
        /// <summary>
        /// Gets or sets the system instructions (system prompt) that define the agent's behavior and personality.
        /// These instructions guide how the agent responds and interacts with users.
        /// </summary>
        /// <remarks>
        /// <para>
        /// <strong>Default Value:</strong> "You are a helpful AI assistant."
        /// </para>
        /// <para>
        /// <strong>Database Considerations:</strong>
        /// </para>
        /// <list type="bullet">
        /// <item><description>Nullable property; can be null in the database.</description></item>
        /// <item><description>Can contain lengthy text; use nvarchar(max) or text column type.</description></item>
        /// <item><description>Consider compression if storing many large instruction sets.</description></item>
        /// </list>
        /// </remarks>
        /// <value>The system instructions string. Default: <c>"You are a helpful AI assistant."</c></value>
        public string? Instructions { get; set; }
        
        /// <summary>
        /// Gets or sets the tool invocation mode that controls how the agent uses available tools.
        /// Determines whether tools are automatically invoked or require explicit confirmation.
        /// </summary>
        /// <remarks>
        /// <para>
        /// <strong>Default Value:</strong> The name of <see cref="ChatToolMode.Auto"/> (typically "Auto")
        /// </para>
        /// <para>
        /// <strong>Supported Values:</strong>
        /// </para>
        /// <list type="bullet">
        /// <item><description>"Auto" - Tools are automatically invoked as needed</description></item>
        /// <item><description>"RequireAny" - At least one tool must be used in the response</description></item>
        /// </list>
        /// <para>
        /// <strong>Database Considerations:</strong>
        /// </para>
        /// <list type="bullet">
        /// <item><description>Nullable property; stored as string representation of enum name.</description></item>
        /// <item><description>Recommended max length: 50 characters.</description></item>
        /// </list>
        /// </remarks>
        /// <value>The tool mode name string. Default: <c>ChatToolMode.Auto.GetType().Name</c> (typically "Auto")</value>
        /// <seealso cref="GetToolMode"/>
        public string? ToolMode { get; set; }
        
        /// <summary>
        /// Gets or sets the temperature parameter that controls randomness in the AI model's responses.
        /// Higher values (e.g., 0.8) make output more random; lower values (e.g., 0.2) make it more deterministic.
        /// </summary>
        /// <remarks>
        /// <para>
        /// <strong>Default Value:</strong> <c>null</c> (uses model's default temperature)
        /// </para>
        /// <para>
        /// <strong>Valid Range:</strong> Typically 0.0 to 2.0, though this varies by provider.
        /// </para>
        /// <para>
        /// <strong>Important Note:</strong> Some models (e.g., gpt-5-nano, gpt-5-mini) do not support 
        /// temperature configuration and should leave this as null.
        /// </para>
        /// <para>
        /// <strong>Database Considerations:</strong>
        /// </para>
        /// <list type="bullet">
        /// <item><description>Nullable float; stored as SQL REAL or FLOAT type.</description></item>
        /// <item><description>Default null means use provider/model default temperature.</description></item>
        /// </list>
        /// </remarks>
        /// <value>The temperature value or null. Default: <c>null</c></value>
        public float? Temperature { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether conversation history should be persisted to storage.
        /// When true, the agent's chat messages are saved for future retrieval and context.
        /// </summary>
        /// <remarks>
        /// <para>
        /// <strong>Default Value:</strong> <c>true</c>
        /// </para>
        /// <para>
        /// <strong>Database Considerations:</strong>
        /// </para>
        /// <list type="bullet">
        /// <item><description>Non-nullable boolean; stored as SQL BIT type.</description></item>
        /// <item><description>Has inline default value initialization (<c>= true</c>).</description></item>
        /// <item><description>Controls whether associated chat messages are stored in related tables.</description></item>
        /// </list>
        /// </remarks>
        /// <value>True if conversations should be persisted; otherwise, false. Default: <c>true</c></value>
        public bool PersistConversation { get; set; } = true;

        /// <summary>
        /// Gets or sets the response format type that defines the structure of the agent's output.
        /// When set to a specific type, the AI will format responses as structured JSON matching that type's schema.
        /// When null, the agent returns plain text responses.
        /// </summary>
        /// <remarks>
        /// <para>
        /// <strong>Default Value:</strong> <c>null</c> (plain text responses)
        /// </para>
        /// <para>
        /// <strong>Usage:</strong>
        /// Set to a class type (e.g., <c>typeof(Person)</c>) to force structured responses.
        /// The AI will return JSON conforming to that class's property structure.
        /// </para>
        /// <para>
        /// <strong>Database Considerations:</strong>
        /// </para>
        /// <list type="bullet">
        /// <item>
        /// <description>
        /// <strong>Critical:</strong> Requires custom value conversion in EF Core configuration.
        /// The <see cref="Type"/> object cannot be directly stored and must be converted to/from 
        /// a string representation (typically using <see cref="Type.AssemblyQualifiedName"/>).
        /// </description>
        /// </item>
        /// <item>
        /// <description>Nullable property; null in database means plain text responses.</description></item>
        /// <item><description>Consider using nvarchar(500) or larger for the converted string.</description></item>
        /// </list>
        /// <para>
        /// <strong>Example Value Converter:</strong>
        /// </para>
        /// <code>
        /// public class TypeToStringConverter : ValueConverter&lt;Type?, string?&gt;
        /// {
        ///     public TypeToStringConverter() : base(
        ///         v => v == null ? null : v.AssemblyQualifiedName,
        ///         v => v == null ? null : Type.GetType(v))
        ///     { }
        /// }
        /// </code>
        /// </remarks>
        /// <value>A <see cref="Type"/> object defining the response schema, or null for text. Default: <c>null</c></value>
        /// <seealso cref="GetResponseFormat"/>
        public Type? ResponseFormat { get; set; }
        
        /// <summary>
        /// Gets or sets a comma-separated list of tool identifiers that are available to this agent.
        /// Tools extend the agent's capabilities by allowing it to perform specific actions or access external services.
        /// </summary>
        /// <remarks>
        /// <para>
        /// <strong>Default Value:</strong> "web_search,code_interpreter,datetime"
        /// </para>
        /// <para>
        /// <strong>Format:</strong> Comma-separated string of tool identifiers (e.g., "tool1,tool2,tool3")
        /// </para>
        /// <para>
        /// <strong>Common Tools:</strong>
        /// </para>
        /// <list type="bullet">
        /// <item><description>"web_search" - Allows the agent to search the web for information</description></item>
        /// <item><description>"code_interpreter" - Enables code execution and analysis</description></item>
        /// <item><description>"datetime" - Provides current date and time information</description></item>
        /// </list>
        /// <para>
        /// <strong>Database Considerations:</strong>
        /// </para>
        /// <list type="bullet">
        /// <item><description>Should be configured as required (non-nullable) in EF Core.</description></item>
        /// <item><description>Recommended max length: 500-1000 characters depending on tool name lengths.</description></item>
        /// <item><description>Alternative: Consider a separate many-to-many relationship table for more complex tool management.</description></item>
        /// </list>
        /// </remarks>
        /// <value>A comma-separated list of tool identifiers. Default: <c>"web_search,code_interpreter,datetime"</c></value>
        public string ToolList { get; set; }
        
        /// <summary>
        /// Gets or sets additional provider-specific or custom settings as a serialized string.
        /// Can store JSON, XML, or other formatted configuration data that doesn't fit into standard properties.
        /// </summary>
        /// <remarks>
        /// <para>
        /// <strong>Default Value:</strong> Not explicitly set in constructor (will be <c>null</c>)
        /// </para>
        /// <para>
        /// <strong>Usage:</strong>
        /// Store provider-specific parameters, custom metadata, or extended configuration options
        /// that don't warrant dedicated properties. Typically stored as JSON for easy parsing.
        /// </para>
        /// <para>
        /// <strong>Database Considerations:</strong>
        /// </para>
        /// <list type="bullet">
        /// <item><description>Nullable property; can be null in the database.</description></item>
        /// <item><description>Use nvarchar(max) or text type for flexible storage.</description></item>
        /// <item><description>If storing JSON, consider using JSON column type (SQL Server 2016+) for querying capabilities.</description></item>
        /// </list>
        /// <para>
        /// <strong>Note:</strong> Property name contains typo "Additinonal" instead of "Additional" - 
        /// consider renaming in a future version for consistency.
        /// </para>
        /// </remarks>
        /// <value>Additional settings as a string, or null. Default: <c>null</c> (not initialized)</value>
        public string? AdditinonalSettings { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentConfiguration"/> class with default values.
        /// This parameterless constructor is required for Entity Framework Core entity instantiation.
        /// </summary>
        /// <remarks>
        /// <para>
        /// All properties are initialized with sensible defaults suitable for immediate use or 
        /// database insertion. The following defaults are set:
        /// </para>
        /// <list type="table">
        /// <listheader>
        /// <term>Property</term>
        /// <description>Default Value</description>
        /// </listheader>
        /// <item>
        /// <term><see cref="Id"/></term>
        /// <description>New GUID in "N" format (32 hex digits, no hyphens)</description>
        /// </item>
        /// <item>
        /// <term><see cref="Provider"/></term>
        /// <description>"OpenAI"</description>
        /// </item>
        /// <item>
        /// <term><see cref="Name"/></term>
        /// <description>"Default Agent"</description>
        /// </item>
        /// <item>
        /// <term><see cref="Description"/></term>
        /// <description>Empty string (<c>string.Empty</c>)</description>
        /// </item>
        /// <item>
        /// <term><see cref="Model"/></term>
        /// <description>"gpt-5-nano"</description>
        /// </item>
        /// <item>
        /// <term><see cref="Instructions"/></term>
        /// <description>"You are a helpful AI assistant."</description>
        /// </item>
        /// <item>
        /// <term><see cref="Temperature"/></term>
        /// <description><c>null</c> (not supported by gpt-5-nano and gpt-5-mini models)</description>
        /// </item>
        /// <item>
        /// <term><see cref="ToolMode"/></term>
        /// <description>Name of <see cref="ChatToolMode.Auto"/> (typically "Auto")</description>
        /// </item>
        /// <item>
        /// <term><see cref="ResponseFormat"/></term>
        /// <description><c>null</c> (plain text responses)</description>
        /// </item>
        /// <item>
        /// <term><see cref="ToolList"/></term>
        /// <description>"web_search,code_interpreter,datetime"</description>
        /// </item>
        /// <item>
        /// <term><see cref="PersistConversation"/></term>
        /// <description><c>true</c></description>
        /// </item>
        /// <item>
        /// <term><see cref="AdditinonalSettings"/></term>
        /// <description><c>null</c> (not explicitly initialized)</description>
        /// </item>
        /// </list>
        /// </remarks>
        public AgentConfiguration()
        {
            Id = Guid.NewGuid().ToString("N");
            Instructions = "You are a helpful AI assistant.";
            Temperature = null;
            Model = "gpt-5-nano";
            Name = "Default Agent";
            ResponseFormat = null;
            ToolMode = ChatToolMode.Auto.GetType().Name;
            Description = string.Empty;
            ToolList = "web_search,code_interpreter,datetime";
            Provider = "OpenAI";
        }

        /// <summary>
        /// Converts the string-based <see cref="ToolMode"/> property into a strongly-typed <see cref="ChatToolMode"/> enumeration value.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method provides type-safe access to the tool mode configuration.
        /// It handles null/empty values gracefully by returning the default <see cref="ChatToolMode.Auto"/>.
        /// </para>
        /// <para>
        /// <strong>Supported Conversions:</strong>
        /// </para>
        /// <list type="bullet">
        /// <item><description>Null or empty string → <see cref="ChatToolMode.Auto"/></description></item>
        /// <item><description>"Auto" → <see cref="ChatToolMode.Auto"/></description></item>
        /// <item><description>"RequireAny" → <see cref="ChatToolMode.RequireAny"/></description></item>
        /// <item><description>Any other value → <see cref="ChatToolMode.Auto"/> (fallback)</description></item>
        /// </list>
        /// </remarks>
        /// <returns>A <see cref="ChatToolMode"/> enumeration value corresponding to the configured tool mode.</returns>
        public ChatToolMode GetToolMode()
        {
            if (string.IsNullOrEmpty(ToolMode))
            {
                return ChatToolMode.Auto;
            }

            return ToolMode switch
            {
                nameof(ChatToolMode.Auto) => ChatToolMode.Auto,
                nameof(ChatToolMode.RequireAny) => ChatToolMode.RequireAny,
                _ => ChatToolMode.Auto
            };
        }

        /// <summary>
        /// Converts the configured <see cref="ResponseFormat"/> type into a <see cref="ChatResponseFormat"/> 
        /// that can be used by the AI chat completion service to structure the response according to a JSON schema.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method determines how the AI agent should format its responses:
        /// </para>
        /// <list type="bullet">
        /// <item>
        /// <description>
        /// <strong>Text Response:</strong> If <see cref="ResponseFormat"/> is <see langword="null"/>, 
        /// the agent will return unstructured text responses (<see cref="ChatResponseFormat.Text"/>).
        /// This is the default behavior when no specific output structure is required.
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// <strong>Structured JSON Response:</strong> If <see cref="ResponseFormat"/> is set to a specific 
        /// <see cref="Type"/> (e.g., a class like <c>Person</c> with properties <c>Name</c> and <c>Age</c>), 
        /// the method generates a JSON schema from that type using <see cref="AIJsonUtilities.CreateJsonSchema(Type)"/>.
        /// The AI model will then be constrained to return responses matching this schema structure.
        /// </description>
        /// </item>
        /// </list>
        /// <para>
        /// When creating a structured response format, the method extracts:
        /// </para>
        /// <list type="number">
        /// <item><description>The JSON schema definition from the type</description></item>
        /// <item><description>The schema name from the type's <see cref="Type.Name"/> property</description></item>
        /// <item><description>An optional schema description from the type's <see cref="DescriptionAttribute"/> if present</description></item>
        /// </list>
        /// <para>
        /// <strong>Example Usage:</strong>
        /// </para>
        /// <code>
        /// // For text responses
        /// var config = new AgentConfiguration { ResponseFormat = null };
        /// var format = config.GetResponseFormat(); // Returns ChatResponseFormat.Text
        /// 
        /// // For structured responses
        /// [Description("Represents a person with basic information")]
        /// public class Person 
        /// { 
        ///     public string Name { get; set; } 
        ///     public int Age { get; set; } 
        /// }
        /// 
        /// var config = new AgentConfiguration { ResponseFormat = typeof(Person) };
        /// var format = config.GetResponseFormat(); // Returns structured JSON format for Person
        /// </code>
        /// </remarks>
        /// <returns>
        /// A <see cref="ChatResponseFormat"/> instance that specifies either:
        /// <list type="bullet">
        /// <item><description><see cref="ChatResponseFormat.Text"/> for unstructured text responses, or</description></item>
        /// <item><description>A JSON schema-based format for structured responses matching the configured <see cref="ResponseFormat"/> type</description></item>
        /// </list>
        /// </returns>
        /// <seealso cref="ResponseFormat"/>
        /// <seealso cref="ChatResponseFormat"/>
        /// <seealso cref="AIJsonUtilities.CreateJsonSchema(Type)"/>
        public ChatResponseFormat GetResponseFormat()
        {
            if (ResponseFormat == null)
            {
                return ChatResponseFormat.Text;
            }
            var schema = AIJsonUtilities.CreateJsonSchema(ResponseFormat);
            return ChatResponseFormat.ForJsonSchema(
                                                schema: schema,
                                                schemaName: ResponseFormat.Name,
                                                schemaDescription: GetTypeDescription(ResponseFormat));
        }

        /// <summary>
        /// Extracts the description attribute from a type if present.
        /// Used internally to provide schema descriptions when converting <see cref="ResponseFormat"/> to <see cref="ChatResponseFormat"/>.
        /// </summary>
        /// <param name="type">The type to extract the description from.</param>
        /// <returns>
        /// The description text from the <see cref="DescriptionAttribute"/> if present; otherwise, <c>null</c>.
        /// </returns>
        private static string? GetTypeDescription(Type type)
        {
            return type
                .GetCustomAttribute<DescriptionAttribute>()
                ?.Description;
        }
    }
}
