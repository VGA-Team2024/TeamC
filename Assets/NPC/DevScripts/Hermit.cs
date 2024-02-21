namespace TeamC
{
    /// <summary>仙人の処理</summary>
    public class Hermit : NPC
    {
        private void FixedUpdate()
        {
            //throw new NotImplementedException();
            base.GetNPCEffects.Invoke();
        }
    }
}
