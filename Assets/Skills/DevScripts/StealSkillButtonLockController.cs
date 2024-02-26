using SgLibUnite.UI;
using UnityEngine;

namespace TeamC
{
    [RequireComponent(typeof(ClicableObject))]
    [RequireComponent(typeof(StealSkill))]
    public class StealSkillButtonLockController : MonoBehaviour
    {
        private ClicableObject _button;
        private StealSkill _skill;

        private void Start()
        {
            _button = GetComponent<ClicableObject>();
            _skill = GetComponent<StealSkill>();
        }

        private void Update()
        {
            _button.interactable = !(_skill.GetIsCoolTime() || _skill.GetIsLocked());
        }
    }
}
