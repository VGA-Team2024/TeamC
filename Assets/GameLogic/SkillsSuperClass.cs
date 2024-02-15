using UnityEngine;
using UnityEngine.Events;

namespace TeamC
{
    /// <summary> SkillsのSuperクラス </summary>
    public class SkillsSuperClass : MonoBehaviour
    {
        private int _requiredLv = 1; // the level required
        private string _skillName = "SkillsName"; // the name of skills
        private float _coolTime = 1.0f; // the cool time "unit is Seconds"
        private UnityEvent Effects; //the effects 
    }
}