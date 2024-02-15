using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace TeamC
{
    /// <summary> Skillsデータのひな形 </summary>
    [CreateAssetMenu(fileName = "GeneratedSkillsData", menuName = "CreateSkillsData", order = 1)]
    public class SkillsDataTemplate : ScriptableObject
    {
        /// <summary> 必要レベル </summary>
        public int RequiredLevel = 1; // the level required

        /// <summary> スキル名 </summary>
        public string SkillName = "SkillsName"; // the name of skills

        /// <summary> クールタイム[秒] </summary>
        public float CoolTime = 1.0f; // the cool time "unit is Seconds"

        /// <summary> 効果 </summary>
        /// インスペクタからAssetsのスクリプトの関数を指定できるので指定してSkillsへアタッチする使い方を想定
        public UnityEvent Effects; //the effects 
    }

    /// <summary> SkillsのSuperクラス </summary>
    public class SkillsSuperClass : MonoBehaviour, ISkills
    {
        
    }
}