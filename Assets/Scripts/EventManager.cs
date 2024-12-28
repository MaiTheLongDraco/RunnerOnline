using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public Dictionary<EventType, UnityAction<bool>> eventBackLog;
    private static EventManager _instance;
    public static EventManager Instance {  get { 
            return _instance; 
        } }
    void Start()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this) { 
            Destroy(this);
        }
        eventBackLog= new Dictionary<EventType, UnityAction<bool>>();
    }
    public void SubcribeEvent(EventType eventType, UnityAction<bool> action)
    {
        if (eventBackLog.ContainsKey(eventType)) { 
            eventBackLog[eventType]+= action;
        }
        else
        {
            eventBackLog.Add(eventType, action);
        }
    }
    public void UnSubcribeEvent(EventType eventType, UnityAction<bool> action) { 
        if(eventBackLog.ContainsKey(eventType))
        {
            eventBackLog[eventType]-= action;
        }
      
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
public enum EventType
{
    ConnectSuccess,
    ConnectFail,
    ReceiveSuccess,
    ReceiveFail,
    SendSuccess,
    SendFail,
    NewPlayerEnter
}
