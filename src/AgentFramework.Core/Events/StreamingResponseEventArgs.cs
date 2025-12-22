using System;

namespace AgentFramework.Core.Events
{
    public class StreamingResponseEventArgs : EventArgs
    {
        public string Text { get; set; } = string.Empty;
        public object? StreamingObject { get; set; }
        public bool IsComplete { get; set; }
        public Exception? Error { get; set; }

        public StreamingResponseEventArgs(string text, object? streamingObject = null, bool isComplete = false, Exception? error = null)
        {
            Text = text;
            StreamingObject = streamingObject;
            IsComplete = isComplete;
            Error = error;
        }
    }
}
