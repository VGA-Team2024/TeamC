namespace TeamC
{
    /// <summary>魔法使いの処理</summary>
    public class Wizard : NPCSuperClass
    {
        public int GetCurrentLevel() => _currentLv;

        public void WizardBuff()
        {
            //throw new NotImplementedException();
            base.GetNPCEffects.Invoke();
        }
    }
}