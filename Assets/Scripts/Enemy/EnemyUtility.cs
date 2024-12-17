using System;

public static class EnemyUtility
{
    /// <summary> 重み付き抽選関数 </summary>
    /// <param name="weights"> 各確率の重み </param>
    /// <returns> ランダムで選ばれたindex </returns>
    public static int ProbabilityCalculate(int[] weights)
    {
        var rnd = new Random().Next(1, 101);
        var cumulative = 0;
        var index = 0;
        for (var i = 0; i < weights.Length; i++)
        {
            cumulative += weights[i];
            if (rnd > cumulative) continue;
            index = i;
            break;
        }

        return index;
    }
}
