using System.Collections.Generic;
using UnityEngine;

namespace TeamC
{
    /// <summary>仙人の処理</summary>
    public class Hermit : NPC, IHermit
    {
        [SerializeField, Header("スキルのデータベース")] private SkillsDataTemplate[] skills;
        
        private void FixedUpdate()
        {
            if(!_isActive) return;
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
    }
}
