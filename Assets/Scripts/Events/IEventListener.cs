// interface used to indicate that this class consumes events
public interface IEventListener {

    /*
    Subscribe to events:
    EventManager.GetInstance().Subscribe(typeof(EventClass), this);
    Be sure to unsubscribe to events:
    EventManager.GetInstance().Unsubscribe(typeof(EventClass), this);
    */

    void ConsumeEvent(IEvent e);
    /*
    To have the listener receive events, implement ConsumeEvent
    All events that the listener is subscribed to will be sent to ConsumeEvent,
    so the method should check e's type to ensure it's the right kind of event
    and then cast it to the correct type to retreive the payload.
    In this way, it can receive multiple event types, checking for each type it needs.
    */

}