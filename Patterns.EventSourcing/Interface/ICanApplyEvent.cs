namespace Patterns.EventSourcing.Interface
{
    public interface ICanApplyEvent<TEvent, TState>
    {
        TState ApplyEvent(TimestampedValue<TEvent> value, TState currentState);
    }
}