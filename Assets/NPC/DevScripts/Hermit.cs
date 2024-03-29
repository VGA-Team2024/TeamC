using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TeamC
{
    /// <summary>仙人の処理</summary>
    public class Hermit : NPC, IInitializedTarget
    {
        [SerializeField, Header("スキルのデータベース")] private SkillsDataTemplate[] skills;

        private void FixedUpdate()
        {
            _isActive = GetCurrentLevel() > 0;
            if (!_isActive) return;
            //throw new NotImplementedException();
            base.GetNPCEffects.Invoke();
        }

        public List<SkillsDataTemplate> GetFirableSkills()
        {
            //throw new System.NotImplementedException();

            List<SkillsDataTemplate> result = new List<SkillsDataTemplate>();
            
#if UNITY_EDITOR
            // 使用可能なスキルをコンソールに出力する
            string message = String.Empty;

            foreach (var skill in result)
            {
                message += $"{skill.SkillName}\n";
            }

            Debug.Log(message);
#endif

            return result;
        }

        public void InitializeObject()
        {
            TaskOnShopBoughtCharacter += (x) => { this._currentLv = x; };
        }

        public void PauseObject()
        {
            throw new System.NotImplementedException();
        }

        public void ResumeObject()
        {
            throw new System.NotImplementedException();
        }

        public void FinalizeObject()
        {
        }
    }
}