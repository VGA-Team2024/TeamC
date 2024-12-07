using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

/// <summary>敵のAnimationを制御</summary>
public abstract class OpeningCharacterAnimation : MonoBehaviour
{
    #region フィールド

    [SerializeField, InspectorVariantName("回転速度")] protected float _rotationSpeed;

    [SerializeField, InspectorVariantName("落下時間")] protected float _fallDuration;

    [SerializeField, InspectorVariantName("移動先")] protected Vector3 _moveOffset;

    [SerializeField, InspectorVariantName("落下を開始する時間")] protected float _fallStartTime;

    #endregion

    /// <summary>指定された位置まで回転しながら移動するアニメーションを再生</summary>
    public abstract UniTask AnimationSettings();

    protected void OnDestroy()
    {
        DOTween.Kill(transform);
    }
}