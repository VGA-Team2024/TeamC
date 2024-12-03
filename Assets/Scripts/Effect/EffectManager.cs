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
    PlayerGameOverEffect,
    PlayerAttackHitEffect,
    PlayerMusicNoteEffect,
}

/// <summary>
/// プレハブから生成されるエフェクトの名前
/// </summary>
public enum InstancePlayEffectName
{
    
}

public class EffectManager : MonoBehaviour
{
    static EffectManager _instance;
    public static EffectManager Instance => _instance;
    
    [SerializeField,InspectorVariantName("Scene上に配置されているエフェクトをアタッチする")] 
    private ParticleSystem[] _playParticleObjects;

    [SerializeField, InspectorVariantName("Prefabから呼び出すエフェクトをアタッチする")]
    private GameObject[] _instanceParticlePrefabs;
    
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
    /// エフェクト全般を再生するメソッド
    /// </summary>
    /// <param name="effectIndex">再生するエフェクトの番号</param>
    /// <param name="y">0で大丈夫</param>
    public void PlayEffect(PlayEffectName playEffectName,float y)
    {
        //再生するエフェクトの番号と座標を受け取って再生する
        var renderer = _playParticleObjects[(int)playEffectName].GetComponent<ParticleSystemRenderer>();
        renderer.flip = new Vector3(y,0,0);
        _playParticleObjects[(int)playEffectName].Play();
    }

    // TODO エフェクトが増える場合オブジェクトプールもありかも 
    /// <summary>
    /// エフェクト全般を生成して再生する
    /// </summary>
    /// <param name="parentObject">再生したいエフェクトの親オブジェクト</param>
    /// <param name="playEffectName">再生したいエフェクトの名前</param>
    /// <param name="effectPosition">再生したいエフェクトの位置</param>
    public void PlayInstanceEffect(GameObject parentObject, PlayEffectName playEffectName, Vector3 effectPosition)
    {
        //再生するエフェクトの親オブジェクトとエフェクトの名前と座標を受け取って再生する
        GameObject obj = Instantiate(_instanceParticlePrefabs[(int)_playEffectName], effectPosition, Quaternion.identity);
        obj.transform.SetParent(parentObject.transform);
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
}
