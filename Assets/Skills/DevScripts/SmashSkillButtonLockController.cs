using SgLibUnite.UI;
using UnityEngine;

namespace TeamC
{
    /// <summary>スマッシュのスキルボタンのロック状況を管理します</summary>
    [RequireComponent(typeof(ClicableObject))]
    [RequireComponent(typeof(SmashSkill))]
    public class SmashSkillButtonLockController : MonoBehaviour
    {
        private ClicableObject _button;
        private SmashSkill _skill;

        private void Start()
        {
            _button = GetComponent<ClicableObject>();
            _skill = GetComponent<SmashSkill>();
        }

        private void Update()
        {
            _button.interactable = !(_skill.GetIsCoolTime() || _skill.GetIsLocked());
        }
    }
}
