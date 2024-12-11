using System;
using UnityEngine;

/// <summary>
/// Scene上に置かれてるエフェクトの名前
/// </summary>
public enum PlayEffectName
{
    // TODO PlayerAttackEffect以外は仮で書いてる変更する可能性あり
    
    PlayerAttackEffect,
    PlayerDashEffect,
    PlayerMoveEffect,
    PlayerMusicNoteEffect,
}

/// <summary>
/// プレハブから生成されるエフェクトの名前
/// </summary>
public enum InstancePlayEffectName
{
    PlayerAttackHitEffect,
    PlayerSpecialAttackEffect, 
    PlayerJumpEffect,
}

public class EffectManager : MonoBehaviour
{
    static EffectManager _instance;
    public static EffectManager Instance => _instance;
    
    [SerializeField,InspectorVariantName("Scene上に配置されているエフェクトをアタッチする")] 
    private ParticleSystem[] _playEffectObjects;

    [SerializeField, InspectorVariantName("Prefabから呼び出すエフェクトをアタッチする")]
    private GameObject[] _instanceEffectPrefabs;
    
    //Scene上に配置されているエフェクト
    private PlayEffectName _playEffectName;
    
    //プレハブから呼び出すエフェクト
    private InstancePlayEffectName _instancePlayEffectName;



    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    /// <summary>
    /// エフェクトを再生したい場合に使うメソッド
    /// </summary>
    /// <param name="playEffectName"></param>
    /// <param name="effectFlip">エフェクトを反転させたい場合に使うパラメータ基本数値は０</param>
    public void PlayEffect(PlayEffectName playEffectName,float effectFlip)
    {
        //再生するエフェクトの番号と座標を受け取って再生する
        var renderer = _playEffectObjects[(int)playEffectName].GetComponent<ParticleSystemRenderer>();
        renderer.flip = new Vector3(effectFlip,0,0);
        _playEffectObjects[(int)playEffectName].Play();
    }

    // TODO エフェクトが増える場合オブジェクトプールもありかも 
    /// <summary>
    /// オブジェクトの子オブジェクトとしてエフェクトを生成したい場合に使うメソッド
    /// </summary>
    /// <param name="parentObj">親オブジェクトの座標</param>
    /// <param name="instancePlayEffectName">生成したいエフェクトの名前</param>
    /// <param name="effectPosition">生成したいエフェクトの座標</param>
    public void ParentInstanceEffect(Transform parentObj, InstancePlayEffectName instancePlayEffectName, Vector3 effectPosition)
    {
        //再生するエフェクトの親オブジェクトとエフェクトの名前と座標を受け取って再生する
        GameObject obj = Instantiate(_instanceEffectPrefabs[(int)instancePlayEffectName], effectPosition, Quaternion.identity);
        obj.transform.SetParent(parentObj);   
    }

    public void InstanceEffect(InstancePlayEffectName effectName, Vector3 effectPosition ,Vector3 effectRotate)
    {
        Instantiate(_instanceEffectPrefabs[(int)effectName], effectPosition, Quaternion.Euler(effectRotate));
    }
    
    /// <summary>
    /// 指定された再生してるエフェクトを一時停止するメソッド
    /// </summary>
    /// <param name="effect">再生を止めたいエフェクト</param>
    public void PauseEffect(ParticleSystem effect)
    {
        //エフェクトを一時停止する
        effect.Pause();
    }
    
    
    /// <summary>
    ///  Scene上にある全てのParticleSystemコンポーネントが付いているオブジェクトを取得して再生する  
    /// </summary>
    public void ResumeAllEffect()
    {
        var effects = FindObjectsOfType<ParticleSystem>();
        foreach (var item in effects)
        {
            item.Play();
        }
    }

    /// <summary>
    /// Scene上にある全てのParticleSystemコンポーネントが付いているオブジェクトを取得して再生する  
    /// </summary>
    public void PauseAllEffect()
    {
        var effects = FindObjectsOfType<ParticleSystem>();
        foreach (var item in effects)
        {
            item.Pause();
        }
    }

    public void StopPlayEffect(PlayEffectName playEffectName)
    {
        _playEffectObjects[(int)playEffectName].Stop();
    }
    
    public void ReStartPlayEffect(PlayEffectName playEffectName)
    {
        _playEffectObjects[(int)playEffectName].Stop();
        _playEffectObjects[(int)playEffectName].Play();
    }
}
