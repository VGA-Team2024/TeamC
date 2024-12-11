using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSwitch : MonoBehaviour
{
    [SerializeField]
    [Tooltip("切り替え後のカメラ")]
    private CinemachineVirtualCamera virtualCamera;

    // 切り替え後のカメラの元々のPriorityを保持しておく
    private int defaultPriority;

    // Start is called before the first frame update
    void Start()
    {
        defaultPriority = virtualCamera.Priority;
    }

    /// <summary>
    /// Colliderの範囲に入り続けている間実行され続ける
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        // 当たった相手に"Player"タグが付いていた場合
        if (other.gameObject.tag == "Player")
        {
            // 他のvirtualCameraよりも高い優先度にすることで切り替わる
            virtualCamera.Priority = 100;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
