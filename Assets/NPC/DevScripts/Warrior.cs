namespace TeamC
{
    /// <summary>戦士の処理</summary>
    public class Warrior : NPC,IInitializedTarget
    {
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
