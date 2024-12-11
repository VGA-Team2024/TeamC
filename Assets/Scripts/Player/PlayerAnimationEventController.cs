using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEventController : MonoBehaviour
{
    private Dictionary<animationType,Action> _animEventDic = new Dictionary<animationType, Action>();

    public Dictionary<animationType, Action> AnimEventDic
    {
        get => _animEventDic;
        set => _animEventDic = value;
    }

    public void EventPlay(animationType type)
    {
        _animEventDic[type]?.Invoke();
    }
    public enum animationType
    {
        AttackColliderEnable,
        AttackRangeEnable,
        StepAudio,
    }
}