namespace TeamC
{
    /// <summary>戦士の処理</summary>
    public class Warrior : NPC
    {
        private void FixedUpdate()
        {
            if(!_isActive) return;
            //throw new NotImplementedException();
            base.GetNPCEffects.Invoke();
        }
    }
}
