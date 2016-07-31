using System;

namespace Patterns.EventSourcing.Interface
{
    [Serializable]
    public class TimestampedValue<TValue>
    {
        public TimestampedValue(TValue value, DateTime timestamp)
        {
            Timestamp = timestamp;
            Value = value;
        }

        public DateTime Timestamp { get; set; }
        public TValue Value { get; set; }

        public override string ToString() => $"[At {Timestamp.ToString("O")}] : {Value}";
    }
}