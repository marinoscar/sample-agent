using AgentFramework.Core.Agents;
using AgentFramework.Core.Configuration;
using AgentFramework.Core.DeepResearch;
using Microsoft.Agents.AI.Workflows;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System.Diagnostics;

namespace AgentFramework.Terminal
{
    /// <summary>
    /// Application entry point
    /// This class sets up dependency injection, logging, and provides
    /// a structured way to run console actions with error handling and logging.
    /// </summary>
    /// <remarks>
    /// Requires the nuget packages: 
    ///  - Microsoft.Extensions.Logging.Console
    ///  - Microsoft.Extensions.Hosting
    /// </remarks>
    class Program
    {
        /// <summary>
        /// The logger instance used for logging messages to the console.
        /// </summary>
        private static ILogger _logger;


        /// <summary>
        /// Main entry point for the application.
        /// Initializes services, logging, parses arguments, and executes the main action.
        /// </summary>
        /// <param name="args">Command-line arguments passed to the application.</param>
        static void Main(string[] args)
        {
            //Set the consolor color
            Console.ForegroundColor = ConsoleColor.White;

            // Create a HostApplicationBuilder instance.
            // This provides configuration, dependency injection (DI), and logging setup for console apps.
            var builder = Host.CreateApplicationBuilder(args);

            // Parse command-line arguments into a strongly-typed options object.
            var arguments = new ConsoleOptions(args);

            // Configure logging providers (console, file, etc.) through a custom initialization method.
            InitializeLogger(builder);

            // Register application services into the DI container.
            builder.Services.AddAgentFactory();

            // Build the Host from the configured builder.
            // This finalizes the DI container and prepares services for use.
            var app = builder.Build();

            // Resolve the ILoggerFactory service from the DI container.
            var factory = app.Services.GetRequiredService<ILoggerFactory>();

            // Create a logger instance specifically for the Program class.
            _logger = factory.CreateLogger<Program>();

            // Execute the main console logic, passing in parsed command-line arguments.
            RunConsole(arguments, app);

            // Keep the application alive and responsive (useful for background services).
            // This blocks until the host is shut down (e.g., by Ctrl+C).
            app.Run();
        }

        /// <summary>
        /// Executes the main logic of the application.
        /// </summary>
        /// <param name="arguments">Parsed command-line options.</param>
        /// <param name="app">The host application instance.</param>
        static void RunConsole(ConsoleOptions arguments, IHost app)
        {
            Console.Clear();

            var workflowBuilder = new DeepResearchWorkflow(app.Services);
            var workflow = workflowBuilder.Build();
            var prompt = "Write a detailed research report on the impact of climate change on global agriculture.";
            var message = new ChatMessage(ChatRole.User, [new TextContent(prompt)]);
            var run = InProcessExecution.RunAsync(workflow, message).GetAwaiter().GetResult();
            foreach (var evt in run.OutgoingEvents)
            {
                Console.WriteLine(evt.Data);
            }
        }

        #region Console Methods

        private static void WriteConsole(string format, params object[] arg)
        {
            var or = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine();
            Console.WriteLine(format, arg);
            Console.WriteLine();
            Console.ForegroundColor = or;
        }

        private static void WriteAssistant(string format, params object[] arg)
        {
            var or = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(format, arg);
            Console.ForegroundColor = or;
        }

        private static void WriteUser(string format, params object[] arg)
        {
            var or = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            Console.WriteLine(format, arg);
            Console.WriteLine();
            Console.ForegroundColor = or;
        }

        /// <summary>
        /// Logs an informational message with formatting.
        /// </summary>
        /// <param name="format">Message format string.</param>
        /// <param name="arg">Arguments for the format string.</param>
        public static void WriteLineInfo(string format, params object[] arg)
        {
            _logger.LogInformation(format, arg);
        }

        /// <summary>
        /// Logs an informational message.
        /// </summary>
        /// <param name="message">Message to log.</param>
        public static void WriteLineInfo(string message)
        {
            _logger.LogInformation(message);
        }

        /// <summary>
        /// Logs a warning message with formatting.
        /// </summary>
        /// <param name="format">Message format string.</param>
        /// <param name="arg">Arguments for the format string.</param>
        public static void WriteLineWarning(string format, params object[] arg)
        {
            _logger.LogWarning(format, arg);
        }

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        /// <param name="message">Message to log.</param>
        public static void WriteLineWarning(string message)
        {
            _logger.LogWarning(message);
        }

        /// <summary>
        /// Logs an error message with formatting.
        /// </summary>
        /// <param name="format">Message format string.</param>
        /// <param name="arg">Arguments for the format string.</param>
        public static void WriteLineError(string format, params object[] arg)
        {
            _logger.LogError(format, arg);
        }

        /// <summary>
        /// Logs an error message.
        /// </summary>
        /// <param name="message">Message to log.</param>
        public static void WriteLineError(string message)
        {
            _logger.LogError(message);
        }

        /// <summary>
        /// Configures and adds logging services to the service collection.
        /// Sets up console logging with custom formatting and color behavior.
        /// </summary>
        private static void InitializeLogger(HostApplicationBuilder builder)
        {
            builder.Services.AddLogging(builder =>
            {
                builder.AddConsole(options =>
                {
                    // Enable colors for the console logger
                    options.LogToStandardErrorThreshold = LogLevel.Debug; // Example: log warnings and above to stderr
                    options.FormatterName = "simple"; // Use the default simple formatter
                }).AddSimpleConsole(o =>
                {
                    o.SingleLine = true; // Write each log message on a single line
                    o.TimestampFormat = "yyyy-MM-dd HH:mm:ss "; // Custom timestamp format
                    o.IncludeScopes = true; // Include scopes in log messages
                    o.ColorBehavior = LoggerColorBehavior.Enabled; // Enable colors in the console output
                    o.UseUtcTimestamp = false; // Use UTC for timestamps
                });
            });
        }

        #endregion
    }

    /// <summary>
    /// Provides an abstraction to handle common console switches and arguments.
    /// </summary>
    public class ConsoleOptions
    {
        private List<string> _args;

        /// <summary>
        /// Creates an instance of the class, storing the provided arguments.
        /// </summary>
        /// <param name="args">Collection of arguments.</param>
        public ConsoleOptions(IEnumerable<string> args)
        {
            _args = new List<string>(args);
        }

        /// <summary>
        /// Gets the value for the switch in the argument collection.
        /// </summary>
        /// <param name="name">The name of the switch.</param>
        /// <returns>The switch value if present, otherwise null.</returns>
        public string this[string name]
        {
            get
            {
                var idx = _args.IndexOf(name);
                if (idx == (_args.Count - 1)) return null;
                return _args[idx + 1];
            }
        }

        /// <summary>
        /// Indicates if the switch exists in the argument collection.
        /// </summary>
        /// <param name="name">The name of the switch.</param>
        /// <returns>True if the switch name is in the collection, otherwise false.</returns>
        public bool ContainsSwitch(string name)
        {
            return _args.Contains(name);
        }
    }
}
