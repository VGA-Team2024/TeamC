namespace TeamC
{
    /// <summary>詩人の処理</summary>
    public class Poet : NPC, IInitializedTarget
    {
        private int _effectMagnification = 1;

        //全てのNPCの効果を2×購入数倍する
        public int GetEffectMagnification() => _effectMagnification;
        public void SetEffectMagnification(int level) => _effectMagnification = level * 2;

        private void FixedUpdate()
        {
            _isActive = GetCurrentLevel() > 0;
            if(!_isActive) return;
            //throw new NotImplementedException();
            base.GetNPCEffects.Invoke();
        }

        public void InitializeObject()
        {
            TaskOnShopBoughtCharacter += (x) => { this._currentLv = x; };
        }

        public void PauseObject()
        {
            throw new System.NotImplementedException();
        }

        public void ResumeObject()
        {
            throw new System.NotImplementedException();
        }

        public void FinalizeObject()
        {
            
        }
    }
}
