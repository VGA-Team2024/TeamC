namespace TeamC
{
    /// <summary>戦士の処理</summary>
    public class Warrior : NPC
    {
        private void FixedUpdate()
        {
            //throw new NotImplementedException();
            base.GetNPCEffects.Invoke();
        }
    }
}
