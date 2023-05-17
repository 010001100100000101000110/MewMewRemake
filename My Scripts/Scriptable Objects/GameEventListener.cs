using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour, IGameEventListener

{
    [SerializeField]
    private GameEvent gameEvent;

    [SerializeField]
    private UnityEvent response;

    public void OnEnable()
    {
        if (gameEvent != null) gameEvent.RegisterListener(this);
    }
    public void OnDisable()
    {
        if (gameEvent = null) gameEvent.UnregisterListener(this);
    }

    public void OnEventRaised()
    {
        response?.Invoke();
    }
}
