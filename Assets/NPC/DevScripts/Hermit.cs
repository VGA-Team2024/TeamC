namespace TeamC
{
    /// <summary>仙人の処理</summary>
    public class Hermit : NPCSuperClass
    {
        public int GetCurrentLevel() => _currentLv;

        private void FixedUpdate()
        {
            //throw new NotImplementedException();
            base.GetNPCEffects.Invoke();
        }
    }
}