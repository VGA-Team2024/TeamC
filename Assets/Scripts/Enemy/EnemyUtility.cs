using UnityEngine;
using Random = System.Random;

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
    
    /// <summary>
    /// ある地点からある地点へ移動させるための初速度を計算
    /// </summary>
    /// <param name="pointA"> 移動元の座標 </param>
    /// <param name="pointB"> 移動先の座標 </param>
    /// <param name="maxHeight"> ジャンプ中の限界高度 </param>
    /// <returns> 初速度 </returns>
    public static Vector3 CalculateVelocity(Vector3 pointA, Vector3 pointB, float maxHeight)
    {
        Vector3 horizontal = new Vector3(pointB.x - pointA.x, 0, pointB.z - pointA.z);

        float verticalSpeed = Mathf.Sqrt(2 * Mathf.Abs(Physics.gravity.y) * (maxHeight - pointA.y));

        float timeToApex = verticalSpeed / Mathf.Abs(Physics.gravity.y);
        float totalTime = timeToApex + Mathf.Sqrt(2 * (maxHeight - pointB.y) / Mathf.Abs(Physics.gravity.y));

        Vector3 horizontalVelocity = horizontal / totalTime;

        return horizontalVelocity + Vector3.up * verticalSpeed;
    }
}
