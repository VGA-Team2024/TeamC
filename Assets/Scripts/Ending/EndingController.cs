using UnityEngine;
using UnityEngine.InputSystem;

namespace Ending
{
    public class EndingController : MonoBehaviour
    {
        [SerializeField] private GameObject _nancyObject;
        
        private PlayerControls _playerControls;
        private EndingSequence _endingSequence;
        
        private bool _isEnding;
        
        private void Awake()
        {
            if (!_endingSequence)
                _endingSequence = FindAnyObjectByType<EndingSequence>();

            _playerControls = new PlayerControls();
            _isEnding = false;
        }

        private void OnEnable()
        {
            _playerControls.Enable();
        }

        private void OnDisable()
        {
            _playerControls.Disable();
            _playerControls.InGame.Jump.started -= Show;
        }

        private void Start()
        {
            _playerControls.InGame.Jump.started += Show;
        }

        private async void Show(InputAction.CallbackContext callbackContext)
        {
            if (_isEnding) return;
            
            _isEnding = true;
            CRIAudioManager.BGM.Stop();
            await _endingSequence.PlayEnding();
            _nancyObject.SetActive(false);
        }
    }
}