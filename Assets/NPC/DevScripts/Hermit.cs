using System.Collections.Generic;
using System.Linq;

namespace TeamC
{
    /// <summary>仙人の処理</summary>
    public class Hermit : NPC, IHermit
    {
        private void FixedUpdate()
        {
            //throw new NotImplementedException();
            base.GetNPCEffects.Invoke();
        }

        public List<SkillsDataTemplate> GetFirableSkills()
        {
            //throw new System.NotImplementedException();

            List<SkillsDataTemplate> skillList = FindObjectsOfType<SkillsDataTemplate>().ToList();
            List<SkillsDataTemplate> result = new List<SkillsDataTemplate>();

            foreach (var skill in skillList)
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
