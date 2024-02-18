using UnityEngine;
using UnityEngine.Events;

// 2/16 90% implemented

namespace TeamC
{
    /// *MEMO*
    /// ～目標～
    /// NPCの
    
    /// <summary> NPCのSuperクラス </summary>
    public class NPCSuperClass : MonoBehaviour, INonPlayerCharacter, IInitializedTarget
    {
        [SerializeField, Tooltip("NPCのデータのひな形"), Header("NPCのデータのひな形")]
        private NPCDataTemplate dataTemplate; // data template

        protected int _currentLv = 1; // level

        /// <summary> NPCの効果を取得する </summary>
        protected UnityEvent GetNPCEffects
        {
            get { return dataTemplate.Effects; }
        }

        public string GetNPCName() => dataTemplate.Name;

        public float GetBasePrice() => dataTemplate.BasePrice;

        public void InitializeObject()
        {
        }

        public void FinalizeObject()
        {
        }
    }
}