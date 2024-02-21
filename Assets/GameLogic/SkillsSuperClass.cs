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

        /// <summary> スキルデータを返す </summary>
        protected SkillsDataTemplate GetSkillData
        {
            get { return skillData; }
        }
        
        public void FireSkills()
        {
            skillData.Effects.Invoke();
        }
    }
}