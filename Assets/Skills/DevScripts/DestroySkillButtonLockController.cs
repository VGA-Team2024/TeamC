using SgLibUnite.UI;
using UnityEngine;

namespace TeamC
{
    [RequireComponent(typeof(ClicableObject))]
    [RequireComponent(typeof(DestroySkill))]
    public class DestroySkillButtonLockController : MonoBehaviour
    {
        private ClicableObject _button;
        private DestroySkill _skill;

        private void Start()
        {
            _button = GetComponent<ClicableObject>();
            _skill = GetComponent<DestroySkill>();
        }

        private void Update()
        {
            _button.interactable = !(_skill.GetIsCoolTime() || _skill.GetIsLocked());
        }
    }
}
