using UnityEngine;
using UnityEngine.Events;

// 2/16 90% implemented

namespace TeamC
{
    /// *MEMO*
    /// ～目標～
    /// NPCの
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

    /// <summary> NPCのSuperクラス </summary>
    public class NPCSuperClass : MonoBehaviour, INonPlayerCharacter, IInitializedTarget
    {
        [SerializeField, Tooltip("NPCのデータのひな形"), Header("NPCのデータのひな形")]
        private NPCDataTemplate dataTemplate; // data template

        private int _currentLv = 1; // level

        /// <summary> levelを取得する </summary>
        public int GetLevel
        {
            get { return _currentLv; }
        }

        /// <summary> levelの値を初期化する </summary>
        public int SetLevel
        {
            set { _currentLv = value; }
        }

        /// <summary> NPCの効果を取得する </summary>
        protected UnityEvent GetNPCEffects
        {
            get { return dataTemplate.Effects; }
        }

        public string GetNPCName() => dataTemplate.Name;

        public float GetBacePrice() => dataTemplate.BasePrice;

        public void InitializeObject()
        {
        }

        public void FinalizeObject()
        {
        }
    }
}