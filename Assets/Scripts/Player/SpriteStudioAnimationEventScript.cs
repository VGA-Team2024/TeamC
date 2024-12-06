using System;
using System.Collections.Generic;
using UnityEngine;

public class SpriteStudioAnimationEventScript : MonoBehaviour
{
    private Dictionary<string, Action> _eventDictionary = new ();
    public Dictionary<string, Action> EventDictionary
    {
        get => _eventDictionary;
        set => _eventDictionary = value;
    }

    public void FunctionEventText(string value)
    {
        if (_eventDictionary.ContainsKey(value))
        {
            _eventDictionary[value]();
        }
    }
}
