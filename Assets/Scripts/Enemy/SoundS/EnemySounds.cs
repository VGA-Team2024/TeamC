using System;
using System.Collections.Generic;
using UnityEngine;

public enum EnemySeEnum
{
    Attack1,
    Walk,
    Attack2,
    Brake,
    Rush,
    JumpAttack,
    Breath,
    Fly
}

[Serializable]
public class EnemyCue
{
    public EnemySeEnum seEnum;
    public string seName;
}

public class EnemySounds : MonoBehaviour
{
    [SerializeField] private string _cueSheet;
    [SerializeField] private List<EnemyCue> _cueName;

    public void PlayEnemySE(EnemySeEnum seEnum)
    {
        var cue = _cueName.Find(_ => _.seEnum == seEnum);
        CRIAudioManager.BGM.Play(_cueSheet, cue.seName);
    }
}
