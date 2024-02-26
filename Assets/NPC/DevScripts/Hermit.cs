using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

            // Resources/SkillsDataフォルダからIsLockedがfalseのスキルのデータをListで取得します
            List<SkillsDataTemplate> result = Resources.LoadAll("SkillsData", typeof(SkillsDataTemplate))
                .OfType<SkillsDataTemplate>().Where(data => !data.IsLocked).ToList();

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
    }
}
