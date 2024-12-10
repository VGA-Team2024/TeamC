/// <summary>
/// プレイヤーの技能を開放するインターフェース
/// </summary>
public interface ITechnicalable
{
    /// <summary>
    /// プレイヤーの技能を開放する関数
    /// </summary>
    /// <param name="tech">技能の種類enum</param>
    void setTechnical(Technical tech);
}

public enum Technical
{
    Dash,
    SecondJump,
    LongRangeAttack
}