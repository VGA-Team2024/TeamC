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

    /// <summary> 最後に使用したポータル </summary>
    public Collider LastPortalUsed
    {
        get => _lastPortalUsed;
        set => _lastPortalUsed = value;
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.CompareTag(""))   // ToDO:ポータルのタグを入れる
        //{
        OnTriggerEnterSubject.OnNext(other);
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        //if (other.CompareTag(""))   // ToDO:ポータルのタグを入れる
        //{
        OnTriggerExitSubject.OnNext(other);
        //}
    }

    // クラスが破棄されるときにSubjectを解放
    private void OnDestroy()
    {
        OnTriggerEnterSubject?.OnCompleted();
        OnTriggerExitSubject?.OnCompleted();
    }
}