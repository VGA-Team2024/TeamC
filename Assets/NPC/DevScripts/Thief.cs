namespace TeamC
{
    /// <summary>盗賊の処理</summary>
    public class Thief : NPCSuperClass
    {
        public int GetCurrentLevel() => _currentLv;

        private void FixedUpdate()
        {
            //throw new NotImplementedException();
            base.GetNPCEffects.Invoke();
        }
    }
}