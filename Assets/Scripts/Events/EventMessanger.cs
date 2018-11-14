using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manages all subscription and broadcast of events. A singleton.
public class EventMessanger : MonoBehaviour {

    public static EventMessanger instance;
    private Dictionary<System.Type, List<IEventListener>> eventDictionary;

    private bool executingEvent = false;
    private System.Type currentEvent = null;

    void Awake() {
        Init();
        if (instance == null) {
            instance = FindObjectOfType<EventMessanger>();
            DontDestroyOnLoad(instance);
            if (instance == null) {
                instance = this;
                DontDestroyOnLoad(this);
            }
        }
    }

    public static EventMessanger GetInstance() {
        if (instance != null) {
            return instance;
        } else {
            instance = FindObjectOfType<EventMessanger>();
            if (instance != null) {
                instance.Init();
                DontDestroyOnLoad(instance);
                return instance;
            } else {
                Debug.LogError("Returning null EventMessanger");
                return null;
            }
        }
    }

    void Init() {
        if (eventDictionary == null) {
            eventDictionary = new Dictionary<System.Type, List<IEventListener>>();
        }
    }

    // Allow a listener to subscribe to an eventType
    public void SubscribeEvent(System.Type eventType, IEventListener consumer) {
        List<IEventListener> listeners;
        if (eventDictionary.TryGetValue(eventType, out listeners)) {
            listeners.Add(consumer);
            eventDictionary[eventType] = listeners;
        } else {
            listeners = new List<IEventListener>();
            listeners.Add(consumer);
            eventDictionary[eventType] = listeners;
        }
    }

    // Allow a listener to unsubscribe to an eventType it previously subscribed to
    public void UnsubscribeEvent(System.Type eventType, IEventListener consumer) {
        if (executingEvent && eventType == currentEvent) {
            StartCoroutine(QueueUnsubscribe(eventType, consumer));
        } else {
            List<IEventListener> listeners;
            if (eventDictionary.TryGetValue(eventType, out listeners)) {
                listeners.Remove(consumer);
                eventDictionary[eventType] = listeners;
            }
        }
    }

    private IEnumerator QueueUnsubscribe(System.Type eventType, IEventListener consumer) {
        do {
            yield return null;
        } while (executingEvent);
        List<IEventListener> listeners;
        if (eventDictionary.TryGetValue(eventType, out listeners)) {
            listeners.Remove(consumer);
            eventDictionary[eventType] = listeners;
        }
    }

    // Trigger an event to all the subscribed listeners with the passed event as the payload
    public void TriggerEvent(IEvent e) {
        executingEvent = true;
        currentEvent = e.GetType();
        List<IEventListener> listeners;
        if (eventDictionary.TryGetValue(e.GetType(), out listeners)) {
            foreach (IEventListener listener in listeners) {
                listener.ConsumeEvent(e);
            }
        }
        executingEvent = false;
        currentEvent = null;
    }

}
