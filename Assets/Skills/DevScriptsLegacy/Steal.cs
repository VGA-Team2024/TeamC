namespace TeamC
{
    public class Steal : SkillsSuperClassL
    {
        /// <summary>スキルロックののフラグを取得します</summary>
        public bool GetIsLocked() => GetSkillData.IsLocked;

        private void FixedUpdate()
        {
            if (GetSkillData.IsLocked) return;

            base.CoolTime();
        }
    }
}
