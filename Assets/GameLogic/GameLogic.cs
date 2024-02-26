using System;
using SgLibUnite.Singleton;

namespace TeamC
{
    /// <summary>
    /// ゲームロジックの主幹をになう
    /// </summary>
    public class GameLogic : SingletonBaseClass<GameLogic>, IGameLogic
    {
        // ↓ ゲームシステム起動
        protected override void ToDoAtAwakeSingleton()
        {
            throw new NotImplementedException();
        }

        private void OnEnable()
        {
            throw new NotImplementedException();
        }

        // ↓ ゲーム内モジュール起動
        private void Start()
        {
            throw new NotImplementedException();
        }

        // ↓ ゲームループ処理
        private void FixedUpdate()
        {
            throw new NotImplementedException();
        }

        private void OnDisable()
        {
            throw new NotImplementedException();
        }

        public decimal ReturnReward()
        {
            throw new NotImplementedException();
        }
    }
}