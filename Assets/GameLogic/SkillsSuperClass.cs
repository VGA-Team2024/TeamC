using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace TeamC
{
    /// <summary> SkillsのSuperクラス </summary>
    public class SkillsSuperClass : MonoBehaviour, ISkills
    {
        [SerializeField, Tooltip("スキルデータ"), Header("The Skills")]
        private SkillsDataTemplate skillData;

        private decimal _coolTimeTimer; // クールタイム用のタイマー
        private bool _isCoolTime;   // クールタイムのフラグ

        /// <summary>クールタイムのフラグを取得します</summary>
        public bool GetIsCoolTime() => _isCoolTime;
        
        /// <summary> スキルデータを返す </summary>
        protected SkillsDataTemplate GetSkillData
        {
            get { return skillData; }
        }

        public void FireSkills()
        {
            skillData.Effects.Invoke();

            if (!_isCoolTime)
            {
                _coolTimeTimer = 0;
            }
        }

        private void Start()
        {
            _coolTimeTimer = GetSkillData.CoolTime;
        }

        protected void CoolTime()
        {
            if (_coolTimeTimer < GetSkillData.CoolTime)
            {
                _isCoolTime = true;
                _coolTimeTimer += (decimal)Time.fixedDeltaTime;
            }
            else
            {
                _isCoolTime = false;

#if UNITY_EDITOR
                //Debug.Log("クールタイム終了");
#endif
            }
        }
    }
}