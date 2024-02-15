using UnityEngine;
using UnityEngine.Events;

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

        /// <summary> NPCの効果(毎フレーム呼び出す) </summary>
        public UnityEvent Effects;
    }

    /// <summary> NPCのSuperクラス </summary>
    public class NPCSuperClass : MonoBehaviour
    {
        [SerializeField, Tooltip("NPCのデータのひな形")]
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

        /// <summary> NPCの名前を取得する </summary>
        public string GetNPCName
        {
            get { return dataTemplate.Name; }
        }

        /// <summary> ベース価格の値を取得する </summary>
        public float GetBasePrice
        {
            get { return dataTemplate.BasePrice; }
        }

        /// <summary> NPCの効果を取得する </summary>
        public UnityEvent GetNPCEffects
        {
            get { return dataTemplate.Effects; }
        }
    }
}