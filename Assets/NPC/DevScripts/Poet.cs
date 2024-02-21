namespace TeamC
{
    /// <summary>詩人の処理</summary>
    public class Poet : NPC
    {
        private int _effectMagnification = 1;

        //全てのNPCの効果を2×購入数倍する
        public int GetEffectMagnification() => _effectMagnification;
        public void SetEffectMagnification(int level) => _effectMagnification = level * 2;

        private void FixedUpdate()
        {
            //throw new NotImplementedException();
            base.GetNPCEffects.Invoke();
        }
    }
}
