using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

/// <summary> NPCデータのひな形 </summary>
[CreateAssetMenu(fileName = "GeneratedNPCData", menuName = "CreateNPCData", order = 1)]
public class NPCDataTemplate : ScriptableObject
{
    /// <summary> NPCの名称 </summary>
    public string DisplayName;

    /// <summary> NPCのベース価格 </summary>
    public float BasePrice;

    /// <summary>
    /// id これは言語によって改変されない
    /// </summary>
    public int Id;
    
    /// <summary> NPCの効果(毎フレーム呼び出すFixedUpdate内) </summary>
    /// インスペクタからAssetsのスクリプトの関数を指定できるので指定してNPCへアタッチする使い方を想定
    public UnityEvent Effects;
}