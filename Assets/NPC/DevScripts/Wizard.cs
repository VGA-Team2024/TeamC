namespace TeamC
{
    /// <summary>魔法使いの処理</summary>
    public class Wizard : NPC
    {
        public void WizardBuff()
        {
            //throw new NotImplementedException();
            base.GetNPCEffects.Invoke();
        }
    }
}
