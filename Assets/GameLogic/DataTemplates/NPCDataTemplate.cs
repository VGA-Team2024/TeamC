using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TeamC
{
    /// <summary> NPCデータのひな形 </summary>
    [CreateAssetMenu(fileName = "GeneratedNPCData", menuName = "CreateNPCData", order = 1)]
    public class NPCDataTemplate : ScriptableObject
    {
        /// <summary> NPCの名称 </summary>
        public string Name;

        /// <summary> NPCのベース価格 </summary>
        public float BasePrice;

        /// <summary> NPCの効果(毎フレーム呼び出すFixedUpdate内) </summary>
        /// インスペクタからAssetsのスクリプトの関数を指定できるので指定してNPCへアタッチする使い方を想定
        public UnityEvent Effects;
    }
}
