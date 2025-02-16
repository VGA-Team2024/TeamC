using UnityEngine;
using UnityEngine.InputSystem;

public class InGameOption : MonoBehaviour
{
    [SerializeField] private string _sceneName;
    private PlayerControls _controls;

    private void Awake()
    {
        _controls = new PlayerControls();
        _controls.UI.Pause.started += OnPause; // Pauseボタンが押されたら実行
    }

    private void OnEnable()
    {
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }

    private void OnDestroy()
    {
        _controls.UI.Pause.started -= OnPause;
        _controls.Dispose();
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        TitleBack();
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame) 
        {
            TitleBack();
        }
    }

    private void TitleBack()
    {
        CRIAudioManager.BGM.Stop();
        SceneLoader.LoadSceneSimple(_sceneName);
    }
}