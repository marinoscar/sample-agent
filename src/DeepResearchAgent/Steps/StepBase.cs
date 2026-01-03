using MassiveAPI.Responses;
using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Workflows;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Logging;
using OpenAI.Assistants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DeepResearchAgent.Steps
{
    /// <summary>
    /// Abstract base class for workflow steps that process input without producing output.
    /// Provides built-in logging and exception handling capabilities for all derived step implementations.
    /// </summary>
    /// <typeparam name="TInput">The type of input message this step processes.</typeparam>
    /// <remarks>
    /// This class extends <see cref="Executor{TInput}"/> and adds automatic exception handling and logging.
    /// All exceptions thrown during step execution are logged with error severity before being re-thrown.
    /// Derived classes must implement <see cref="RunStepAsync"/> to define the step's core logic.
    /// </remarks>
    public abstract class StepBase<TInput> : Executor<TInput>
    {
        /// <summary>
        /// Factory for creating logger instances.
        /// </summary>
        private readonly ILoggerFactory _loggerFactory;

        /// <summary>
        /// Gets the logger instance used for logging step execution information and errors.
        /// The logger is initialized with the step's unique identifier as the category name.
        /// </summary>
        protected ILogger Logger { get; init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StepBase{TInput}"/> class.
        /// </summary>
        /// <param name="id">The unique identifier for this step instance.</param>
        /// <param name="loggerFactory">The logger factory used to create the logger for this step. Cannot be null.</param>
        /// <param name="options">Optional configuration options for the executor.</param>
        /// <param name="declareCrossRunShareable">Indicates whether this step's state can be shared across workflow runs.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="loggerFactory"/> is null.</exception>
        protected StepBase(string id, ILoggerFactory loggerFactory, ExecutorOptions? options = null, bool declareCrossRunShareable = false) : base(id, options, declareCrossRunShareable)
        {
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            Logger = _loggerFactory.CreateLogger(id);
        }

        /// <summary>
        /// Handles the execution of this step with automatic exception handling and logging.
        /// </summary>
        /// <param name="message">The input message to process.</param>
        /// <param name="context">The workflow context containing shared state and services.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
        /// <exception cref="Exception">Re-throws any exception that occurs during step execution after logging it.</exception>
        /// <remarks>
        /// This method wraps the call to <see cref="RunStepAsync"/> with try-catch logic to ensure
        /// all exceptions are logged before propagating. The exception is logged with the step's type name
        /// and the exception message for easier debugging and traceability.
        /// </remarks>
        public async override ValueTask HandleAsync(TInput message, IWorkflowContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                await RunStepAsync(message, context, cancellationToken);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"An error occurred in {GetType().Name}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// When overridden in a derived class, executes the core logic of this workflow step.
        /// </summary>
        /// <param name="message">The input message to process.</param>
        /// <param name="context">The workflow context containing shared state and services.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// Implement this method to define the specific behavior of your workflow step.
        /// Any exceptions thrown from this method will be automatically logged and re-thrown by the base class.
        /// </remarks>
        public abstract ValueTask RunStepAsync(TInput message, IWorkflowContext context, CancellationToken cancellationToken = default);


        /// <summary>
        /// Deserializes a JSON string into an instance of the specified type using custom serialization options.
        /// </summary>
        /// <typeparam name="TResponse">The type to deserialize the JSON string into.</typeparam>
        /// <param name="input">The JSON string to deserialize. Must be valid JSON that matches the structure of <typeparamref name="TResponse"/>.</param>
        /// <param name="options">Custom JSON serialization options to control the deserialization behavior, including property naming policies, reference handling, and formatting.</param>
        /// <returns>An instance of <typeparamref name="TResponse"/> populated with data from the JSON string.</returns>
        /// <exception cref="JsonException">Thrown when the JSON is invalid or cannot be deserialized into <typeparamref name="TResponse"/>.</exception>
        /// <exception cref="InvalidOperationException">Thrown when deserialization completes successfully but returns null.</exception>
        /// <remarks>
        /// This overload allows full control over the deserialization process through the <paramref name="options"/> parameter.
        /// Use this method when you need specific serialization settings that differ from the default behavior.
        /// </remarks>
        protected virtual TResponse Deserialize<TResponse>(string input, JsonSerializerOptions options)
        {
            return JsonSerializer.Deserialize<TResponse>(input, options)
                   ?? throw new InvalidOperationException($"{typeof(TResponse).Name} deserialization returned null.");
        }

        /// <summary>
        /// Deserializes a JSON string into an instance of the specified type using custom serialization options.
        /// </summary>
        /// <typeparam name="TResponse">The type to deserialize the JSON string into.</typeparam>
        /// <param name="input">The JSON string to deserialize. Must be valid JSON that matches the structure of <typeparamref name="TResponse"/>.</param>
        /// <returns>An instance of <typeparamref name="TResponse"/> populated with data from the JSON string.</returns>
        /// <exception cref="JsonException">Thrown when the JSON is invalid or cannot be deserialized into <typeparamref name="TResponse"/>.</exception>
        /// <exception cref="InvalidOperationException">Thrown when deserialization completes successfully but returns null.</exception>
        /// <remarks>
        /// This overload allows full control over the deserialization process through the <paramref name="options"/> parameter.
        /// Use this method when you need specific serialization settings that differ from the default behavior.
        /// </remarks>
        protected virtual TResponse Deserialize<TResponse>(string input)
        {
            return JsonSerializer.Deserialize<TResponse>(input, new JsonSerializerOptions() { 
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            }
                ) ?? throw new InvalidOperationException($"{typeof(TResponse).Name} deserialization returned null.");
        }


    }

    /// <summary>
    /// Abstract base class for workflow steps that process input and produce output.
    /// Provides built-in logging and exception handling capabilities for all derived step implementations.
    /// </summary>
    /// <typeparam name="TInput">The type of input message this step processes.</typeparam>
    /// <typeparam name="TOutput">The type of output this step produces.</typeparam>
    /// <remarks>
    /// This class extends <see cref="Executor{TInput, TOutput}"/> and adds automatic exception handling and logging.
    /// All exceptions thrown during step execution are logged with error severity before being re-thrown.
    /// Derived classes must implement <see cref="RunStepAsync"/> to define the step's core logic.
    /// The output from <see cref="RunStepAsync"/> is automatically returned by the base class's <see cref="HandleAsync"/> method.
    /// </remarks>
    public abstract class StepBase<TInput, TOutput> : Executor<TInput, TOutput>
    {
        /// <summary>
        /// Factory for creating logger instances.
        /// </summary>
        private readonly ILoggerFactory _loggerFactory;

        /// <summary>
        /// Gets the logger instance used for logging step execution information and errors.
        /// The logger is initialized with the step's unique identifier as the category name.
        /// </summary>
        protected ILogger Logger { get; init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StepBase{TInput, TOutput}"/> class.
        /// </summary>
        /// <param name="id">The unique identifier for this step instance.</param>
        /// <param name="loggerFactory">The logger factory used to create the logger for this step. Cannot be null.</param>
        /// <param name="options">Optional configuration options for the executor.</param>
        /// <param name="declareCrossRunShareable">Indicates whether this step's state can be shared across workflow runs.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="loggerFactory"/> is null.</exception>
        protected StepBase(string id, ILoggerFactory loggerFactory, ExecutorOptions? options = null, bool declareCrossRunShareable = false) : base(id, options, declareCrossRunShareable)
        {
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            Logger = _loggerFactory.CreateLogger(id);
        }

        /// <summary>
        /// Handles the execution of this step with automatic exception handling and logging.
        /// </summary>
        /// <param name="message">The input message to process.</param>
        /// <param name="context">The workflow context containing shared state and services.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="ValueTask{TOutput}"/> representing the asynchronous operation with the step's output.</returns>
        /// <exception cref="Exception">Re-throws any exception that occurs during step execution after logging it.</exception>
        /// <remarks>
        /// This method wraps the call to <see cref="RunStepAsync"/> with try-catch logic to ensure
        /// all exceptions are logged before propagating. The exception is logged with the step's type name
        /// and the exception message for easier debugging and traceability.
        /// If an exception occurs, the method returns the default value of <typeparamref name="TOutput"/> before re-throwing.
        /// </remarks>
        public async override ValueTask<TOutput> HandleAsync(TInput message, IWorkflowContext context, CancellationToken cancellationToken = default)
        {
            TOutput output = default!;
            try
            {
                output = await RunStepAsync(message, context, cancellationToken);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"An error occurred in {GetType().Name}: {ex.Message}");
                throw;
            }
            return output;
        }

        /// <summary>
        /// When overridden in a derived class, executes the core logic of this workflow step and produces output.
        /// </summary>
        /// <param name="message">The input message to process.</param>
        /// <param name="context">The workflow context containing shared state and services.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="ValueTask{TOutput}"/> representing the asynchronous operation with the step's output.</returns>
        /// <remarks>
        /// Implement this method to define the specific behavior of your workflow step.
        /// Any exceptions thrown from this method will be automatically logged and re-thrown by the base class.
        /// The returned value will be passed to subsequent steps in the workflow.
        /// </remarks>
        public abstract ValueTask<TOutput> RunStepAsync(TInput message, IWorkflowContext context, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deserializes a JSON string into an instance of the specified type using custom serialization options.
        /// </summary>
        /// <typeparam name="TResponse">The type to deserialize the JSON string into.</typeparam>
        /// <param name="input">The JSON string to deserialize. Must be valid JSON that matches the structure of <typeparamref name="TResponse"/>.</param>
        /// <param name="options">Custom JSON serialization options to control the deserialization behavior, including property naming policies, reference handling, and formatting.</param>
        /// <returns>An instance of <typeparamref name="TResponse"/> populated with data from the JSON string.</returns>
        /// <exception cref="JsonException">Thrown when the JSON is invalid or cannot be deserialized into <typeparamref name="TResponse"/>.</exception>
        /// <exception cref="InvalidOperationException">Thrown when deserialization completes successfully but returns null.</exception>
        /// <remarks>
        /// This overload allows full control over the deserialization process through the <paramref name="options"/> parameter.
        /// Use this method when you need specific serialization settings that differ from the default behavior.
        /// </remarks>
        protected virtual TResponse Deserialize<TResponse>(string input, JsonSerializerOptions options)
        {
            return JsonSerializer.Deserialize<TResponse>(input, options)
                   ?? throw new InvalidOperationException($"{typeof(TResponse).Name} deserialization returned null.");
        }

        /// <summary>
        /// Deserializes a JSON string into an instance of the specified type using custom serialization options.
        /// </summary>
        /// <typeparam name="TResponse">The type to deserialize the JSON string into.</typeparam>
        /// <param name="input">The JSON string to deserialize. Must be valid JSON that matches the structure of <typeparamref name="TResponse"/>.</param>
        /// <returns>An instance of <typeparamref name="TResponse"/> populated with data from the JSON string.</returns>
        /// <exception cref="JsonException">Thrown when the JSON is invalid or cannot be deserialized into <typeparamref name="TResponse"/>.</exception>
        /// <exception cref="InvalidOperationException">Thrown when deserialization completes successfully but returns null.</exception>
        /// <remarks>
        /// This overload allows full control over the deserialization process through the <paramref name="options"/> parameter.
        /// Use this method when you need specific serialization settings that differ from the default behavior.
        /// </remarks>
        protected virtual TResponse Deserialize<TResponse>(string input)
        {
            return JsonSerializer.Deserialize<TResponse>(input, new JsonSerializerOptions()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            }
                ) ?? throw new InvalidOperationException($"{typeof(TResponse).Name} deserialization returned null.");
        }
    }

    /// <summary>
    /// Abstract base class for workflow steps that process input without producing output and utilize an AI agent.
    /// Provides built-in AI agent management, thread handling, logging, and exception handling capabilities.
    /// </summary>
    /// <typeparam name="TInput">The type of input message this step processes.</typeparam>
    /// <remarks>
    /// This class extends <see cref="StepBase{TInput}"/> and adds AI agent functionality through the <see cref="AIAgent"/> class.
    /// Each instance maintains its own agent thread for conversation continuity. If no thread is provided during construction,
    /// a new thread is automatically created.
    /// Derived classes have access to the <see cref="Agent"/> and <see cref="Thread"/> properties to interact with the AI agent.
    /// </remarks>
    public abstract class AgentStepBase<TInput> : StepBase<TInput>
    {
        private readonly AIAgent _aiAgent;

        /// <summary>
        /// Gets the AI agent instance used for processing within this step.
        /// </summary>
        /// <remarks>
        /// This property provides access to the AI agent's capabilities, including generating responses,
        /// executing functions, and managing conversations.
        /// </remarks>
        protected AIAgent Agent { get; init; }

        /// <summary>
        /// Gets the agent thread used to maintain conversation context and state for this step.
        /// </summary>
        /// <remarks>
        /// The thread represents a persistent conversation context that spans multiple interactions with the agent.
        /// If no thread is provided during construction, a new thread is automatically created.
        /// </remarks>
        protected AgentThread Thread { get; init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentStepBase{TInput}"/> class.
        /// </summary>
        /// <param name="id">The unique identifier for this step instance.</param>
        /// <param name="agent">The AI agent instance used for processing. Cannot be null.</param>
        /// <param name="loggerFactory">The logger factory used to create the logger for this step. Cannot be null.</param>
        /// <param name="thread">Optional agent thread for maintaining conversation context. If null, a new thread is created automatically.</param>
        /// <param name="options">Optional configuration options for the executor.</param>
        /// <param name="declareCrossRunShareable">Indicates whether this step's state can be shared across workflow runs.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="agent"/> or <paramref name="loggerFactory"/> is null.</exception>
        /// <remarks>
        /// The constructor automatically creates a new agent thread if one is not provided, ensuring that each step
        /// has a valid conversation context for interacting with the AI agent.
        /// </remarks>
        protected AgentStepBase(string id, AIAgent agent, ILoggerFactory loggerFactory, AgentThread thread = default!, ExecutorOptions? options = null, bool declareCrossRunShareable = false) : base(id, loggerFactory, options, declareCrossRunShareable)
        {
            _aiAgent = agent ?? throw new ArgumentNullException(nameof(agent));
            Agent = _aiAgent;
            Thread = thread ?? _aiAgent.GetNewThread();
        }
    }

    /// <summary>
    /// Abstract base class for workflow steps that process input, produce output, and utilize an AI agent.
    /// Provides built-in AI agent management, thread handling, logging, and exception handling capabilities.
    /// </summary>
    /// <typeparam name="TInput">The type of input message this step processes.</typeparam>
    /// <typeparam name="TOutput">The type of output this step produces.</typeparam>
    /// <remarks>
    /// This class extends <see cref="StepBase{TInput, TOutput}"/> and adds AI agent functionality through the <see cref="AIAgent"/> class.
    /// Each instance maintains its own agent thread for conversation continuity. If no thread is provided during construction,
    /// a new thread is automatically created.
    /// Derived classes have access to the <see cref="Agent"/> and <see cref="Thread"/> properties to interact with the AI agent
    /// and produce output based on agent responses.
    /// </remarks>
    public abstract class AgentStepBase<TInput, TOutput> : StepBase<TInput, TOutput>
    {
        private readonly AIAgent _aiAgent;

        /// <summary>
        /// Gets the AI agent instance used for processing within this step.
        /// </summary>
        /// <remarks>
        /// This property provides access to the AI agent's capabilities, including generating responses,
        /// executing functions, and managing conversations.
        /// </remarks>
        protected AIAgent Agent { get; init; }

        /// <summary>
        /// Gets the agent thread used to maintain conversation context and state for this step.
        /// </summary>
        /// <remarks>
        /// The thread represents a persistent conversation context that spans multiple interactions with the agent.
        /// If no thread is provided during construction, a new thread is automatically created.
        /// </remarks>
        protected AgentThread Thread { get; init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentStepBase{TInput, TOutput}"/> class.
        /// </summary>
        /// <param name="id">The unique identifier for this step instance.</param>
        /// <param name="agent">The AI agent instance used for processing. Cannot be null.</param>
        /// <param name="loggerFactory">The logger factory used to create the logger for this step. Cannot be null.</param>
        /// <param name="thread">Optional agent thread for maintaining conversation context. If null, a new thread is created automatically.</param>
        /// <param name="options">Optional configuration options for the executor.</param>
        /// <param name="declareCrossRunShareable">Indicates whether this step's state can be shared across workflow runs.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="agent"/> or <paramref name="loggerFactory"/> is null.</exception>
        /// <remarks>
        /// The constructor automatically creates a new agent thread if one is not provided, ensuring that each step
        /// has a valid conversation context for interacting with the AI agent.
        /// </remarks>
        protected AgentStepBase(string id, AIAgent agent, ILoggerFactory loggerFactory, AgentThread thread = default!, ExecutorOptions? options = null, bool declareCrossRunShareable = false) : base(id, loggerFactory, options, declareCrossRunShareable)
        {
            _aiAgent = agent ?? throw new ArgumentNullException(nameof(agent));
            Agent = _aiAgent;
            Thread = thread ?? _aiAgent.GetNewThread();
        }
    }
}
