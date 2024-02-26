using SgLibUnite.UI;
using UnityEngine;

namespace TeamC
{
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
