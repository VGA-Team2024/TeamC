using System;
using UniRx;
using UnityEngine;

/// <summary> プレイヤーがポータルに接触したときにイベントを発行するためのクラス </summary>
//[RequireComponent(typeof(Rigidbody(2D)))]   // プロトではコライダーのトリガーを使用しているため
public class OnTriggerEvent : MonoBehaviour
{
    private Collider _lastPortalUsed;   // 最後に使用したポータル
    private Subject<Collider> OnTriggerEnterSubject = new Subject<Collider>();
    private Subject<Collider> OnTriggerExitSubject = new Subject<Collider>();

    public IObservable<Collider> OnTriggerEnterAsObservable => OnTriggerEnterSubject;
    public IObservable<Collider> OnTriggerExitAsObservable => OnTriggerExitSubject;


    private void OnTriggerEnter(Collider other)
    {
        OnTriggerEnterSubject.OnNext(other);
    }

    private void OnTriggerExit(Collider other)
    {
        OnTriggerExitSubject.OnNext(other);
    }

    // クラスが破棄されるときにSubjectを解放
    private void OnDestroy()
    {
        OnTriggerEnterSubject?.OnCompleted();
        OnTriggerExitSubject?.OnCompleted();
    }
}