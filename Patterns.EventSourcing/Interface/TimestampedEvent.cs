using System;

namespace Patterns.EventSourcing.Interface
{
    [Serializable]
    public class TimestampedEvent<TEvent>
    {
        public TimestampedEvent(TEvent eventArgs, DateTime timestamp)
        {
            Timestamp = timestamp;
            EventArgs = eventArgs;
        }

        public DateTime Timestamp { get; set; }
        public TEvent EventArgs { get; set; }

        public override string ToString() => $"[At {Timestamp.ToString("O")}] : {EventArgs}";
    }
}