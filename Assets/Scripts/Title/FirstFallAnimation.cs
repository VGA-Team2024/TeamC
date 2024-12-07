using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Title
{
    // タイトル画面で最初に落とされる人形のアニメーション
    public class FirstFallAnimation : OpeningCharacterAnimation
    {
        public override async UniTask AnimationSettings()
        {
            // 落下タイミングの制御
            await UniTask.Delay(TimeSpan.FromSeconds(_fallStartTime));
            // 回転
            transform.DORotate(new Vector3(0f, 0f, 360f), 1f / (_rotationSpeed / 360f), RotateMode.FastBeyond360)
                .SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
        
        
            Vector3 targetPosition = transform.position + _moveOffset;

            // 移動のAnimation
            transform.DOMove(targetPosition, _fallDuration).SetEase(Ease.InOutQuad).OnComplete(() =>
            {
                if (this != null)
                    Destroy(gameObject);
            });
        }
    }
}