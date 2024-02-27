using System.Collections.Generic;
using UnityEngine;

namespace TeamC
{
    /// <summary>仙人の処理</summary>
    public class Hermit : NPC, IHermit, IInitializedTarget
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

            foreach (var skill in skills)
            {
                if (!skill.IsLocked)
                {
                    result.Add(skill);
                }
            }

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