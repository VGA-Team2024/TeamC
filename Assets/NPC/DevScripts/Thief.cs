namespace TeamC
{
    /// <summary>盗賊の処理</summary>
    public class Thief : NPC
    {
        private void FixedUpdate()
        {
            if(!_isActive) return;
            //throw new NotImplementedException();
            base.GetNPCEffects.Invoke();
        }
    }
}
