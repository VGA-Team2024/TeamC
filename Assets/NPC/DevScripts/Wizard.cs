namespace TeamC
{
    /// <summary>魔法使いの処理</summary>
    public class Wizard : NPC
    {
        public void FixedUpdate()
        {
            if(!_isActive) return;
            //throw new NotImplementedException();
            base.GetNPCEffects.Invoke();
        }
    }
}
