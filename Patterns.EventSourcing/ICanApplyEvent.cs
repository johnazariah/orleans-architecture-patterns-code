namespace Patterns.EventSourcing
{
    public interface ICanApplyEvent<TEvent, TState>
    {
        TState ApplyEvent(TimestampedEvent<TEvent> _event, TState currentState);
    }
}