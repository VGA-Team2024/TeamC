namespace TeamC
{
    /// <summary>盗賊の処理</summary>
    public class Thief : NPC, IInitializedTarget
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
