using UnityEngine;
using Cinemachine;

/// <summary>カメラ切り替え用クラス</summary>
public class CameraSwitch : MonoBehaviour
{
    [SerializeField,InspectorVariantName("切り替え後のカメラ")] private CinemachineVirtualCamera _virtualCamera;

    // 切り替え後のカメラの元々のPriorityを保持しておく
    private int _defaultPriority;
    
    private CinemachineConfiner _confiner;
    
    private void Start()
    {
        Initialize();
    }
    // カメラの初期化を行う
    private void Initialize()
    {
        _defaultPriority = _virtualCamera.Priority;
        _confiner = _virtualCamera.GetComponent<CinemachineConfiner>();
    }

    // 範囲を制限しているコライダーを切り換える
    public void ChangeBoundingVolume(Collider other)
    {
        _confiner.m_BoundingVolume = other;
    }

    /// <summary>Colliderの範囲に入り続けている間実行され続ける</summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // 他のvirtualCameraよりも高い優先度にすることで切り替わる
            _virtualCamera.Priority = 100;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _virtualCamera.Priority = _defaultPriority;
        }
    }
}
