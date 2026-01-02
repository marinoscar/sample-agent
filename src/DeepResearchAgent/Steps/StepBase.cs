using Microsoft.Agents.AI.Workflows;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public ILogger Logger { get; init; }

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
        public ILogger Logger { get; init; }

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
    }
}
