namespace TeamC
{
    /// <summary>戦士の処理</summary>
    public class Warrior : NPCSuperClass
    {
        public int GetCurrentLevel() => _currentLv;

        private void FixedUpdate()
        {
            //throw new NotImplementedException();
            base.GetNPCEffects.Invoke();
        }
    }
}
