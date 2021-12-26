// @author Programmer
// @source https://stackoverflow.com/questions/42034245/unity-eventmanager-with-delegate-instead-of-unityevent
//
// NOTE: Added minor improvements (Parameters, Function Naming and Code Shortening)

using System;
using System.Collections.Generic;

using UnityEngine;

using LAIeRS.Events;

// TODO: Listeners might need EventParameters
[DisallowMultipleComponent]
public class EventManager : MonoBehaviour
{
    public static EventManager Instance 
    {
        get
        {
            if (_eventManager == null)
            {
                _eventManager = FindObjectOfType<EventManager>();

                if (_eventManager == null)
                {
                    //GameObject managerGameObject = Instantiate(GameAssetsManager.Instance.eventManager);
                    //_eventManager = managerGameObject.GetComponent<EventManager>();
                    
                    if (_eventManager == null)
                        Debug.LogError("Failed to find: EventManager");
                }
            }
            
            _eventManager.InitializeDictionary();
            
            return _eventManager;
        }
    }
    
    private static EventManager _eventManager;
    
    private Dictionary<EventID, Action> _eventDictionary;
    private Dictionary<EventID, Action<EventParam>> _paramEventDictionary;
    
    public static void AddListenerTo(EventID eventId, Action newListener)
    {
        if (Instance._eventDictionary.TryGetValue(eventId, out Action currentEvent))
        {
            currentEvent += newListener;
            
            UpdateEvent(eventId, currentEvent);
        }
        else
        {
            //Add event to the Dictionary for the first time
            currentEvent += newListener;
            Instance._eventDictionary.Add(eventId, currentEvent);
        }
    }
    
    public static void AddListenerTo(EventID eventId, Action<EventParam> newListener)
    {
        if (Instance._paramEventDictionary.TryGetValue(eventId, out Action<EventParam> currentEvent))
        {
            currentEvent += newListener;
            
            UpdateEvent(eventId, currentEvent);
        }
        else
        {
            //Add event to the Dictionary for the first time
            currentEvent += newListener;
            Instance._paramEventDictionary.Add(eventId, currentEvent);
        }
    }
    
    public static void RemoveListenerFrom(EventID eventId, Action currentListener)
    {
        if (Instance._eventDictionary.TryGetValue(eventId, out Action currentEvent))
        {
            currentEvent -= currentListener;
            
            UpdateEvent(eventId, currentEvent);
        }
    }
    
    public static void RemoveListenerFrom(EventID eventId, Action<EventParam> currentListener)
    {
        if (Instance._paramEventDictionary.TryGetValue(eventId, out Action<EventParam> currentEvent))
        {
            currentEvent -= currentListener;
            
            UpdateEvent(eventId, currentEvent);
        }
    }
    
    public static void TriggerEvent(EventID eventId, EventParam eventParam = null)
    {
        if (Instance._eventDictionary.TryGetValue(eventId, out Action currentEvent)) 
            currentEvent.Invoke();
        
        if (Instance._paramEventDictionary.TryGetValue(eventId, out Action<EventParam> currentEventParam)) 
            currentEventParam.Invoke(eventParam);
    }
    
    private static void UpdateEvent(EventID eventId, Action currentEvent)
    {
        Instance._eventDictionary[eventId] = currentEvent;
    }
    
    private static void UpdateEvent(EventID eventId, Action<EventParam> currentEvent)
    {
        Instance._paramEventDictionary[eventId] = currentEvent;
    }
    
    private void InitializeDictionary()
    {
        if (_eventDictionary == null)
            _eventDictionary = new Dictionary<EventID, Action>();
        
        if (_paramEventDictionary == null)
            _paramEventDictionary = new Dictionary<EventID, Action<EventParam>>();
    }
}