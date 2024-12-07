using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Title
{
    // 2回目の落ちる人形のアニメーションを制御
    public class SecondFallAnimation : OpeningCharacterAnimation
    {
        public override async UniTask AnimationSettings()
        {
            // 落下タイミングの制御
            await UniTask.Delay(TimeSpan.FromSeconds(_fallStartTime));
            
            Vector3 targetPosition = transform.position + _moveOffset;

            // 移動のAnimation
            transform.DOMove(targetPosition, _fallDuration).SetEase(Ease.InOutQuad).OnComplete(() =>
            {
                // 移動が完了したら床にぶつかるアニメーションを発動
            });
        }
    }
}