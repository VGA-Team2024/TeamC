using System;
using UnityEngine;
using UnityEngine.Events;

namespace TeamC
{
    #region TODO

    // 2/17 菅沼 → 五島 NPCの効果はNPCの効果のScriptableObject(以降SO)が
    // ありますので、それのEffectsへSOを継承したクラスのメソッドを割り当ててください。
    // 並びにSOを継承していて各NPCの効果の処理を実現しているメソッドを実装してください。
    // base.GetPlayerApplayingDamage() でプレイヤーのダメージ量の取得
    // base.SetPlayerApplayingDamage() でプレイヤーのダメージ量を引数の値で初期化
    // 実装漏れあれば問い合わせて下さい

    #endregion

    /// <summary> NPCの機能を提供 </summary>
    public class NPC : MonoBehaviour, INonPlayerCharacter
    {
        [SerializeField, Tooltip("NPCのデータのひな形"), Header("NPCのデータのひな形")]
        private NPCDataTemplate dataTemplate; // data template

        protected int _currentLv = 0; // level

        protected bool _isActive = false;
        private IInitializedTarget _initializedTargetImplementation;

        /// <summary> NPCの効果を取得する </summary>
        protected UnityEvent GetNPCEffects
        {
            get { return dataTemplate.Effects; }
        }

        public string GetNPCName() => dataTemplate.Name;

        public float GetBasePrice() => dataTemplate.BasePrice;

        public Action<int> TaskOnShopBoughtCharacter { get; set; }

        public void SetActivation(bool cond)
        {
            _isActive = cond;
        }

        public int GetCurrentLevel() => _currentLv;
        
        public int SetLevel
        {
            set { _currentLv = value; }
        }
    }
}