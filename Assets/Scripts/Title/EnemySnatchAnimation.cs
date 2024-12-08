using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Title
{
    // 敵がオルゴールを盗む演出
    public class EnemySnatchAnimation : OpeningCharacterAnimation
    {
        [SerializeField, InspectorVariantName("敵が止まる位置を設定")] private GameObject _targetPosition;
        [SerializeField] private Vector3 _rotation;
        
        private Vector3 _currentPosition;

        // 初期位置を保存する
        private Vector3 GetCurrentPosition()
        {
            return _currentPosition;
        }
        
        private void Awake()
        {
            _currentPosition = transform.position;
        }
        public override async UniTask AnimationSettings()
        {
            // 人形が走り去る
            await transform.DOMove(_currentPosition, _duration).SetEase(Ease.InOutQuad);
            // オルゴールを持って小人がフェードアウト
        }

        // 小人が走って現れる
        public async UniTask Move()
        {
            await transform.DOMove(_targetPosition.transform.position, _duration).SetEase(Ease.InOutQuad);
        }

        // 人形を少し傾ける
        public async UniTask HeelOver()
        {
            await transform.DORotate(new Vector3(_rotation.x, _rotation.y, _rotation.z), 1);
        }
    }
}