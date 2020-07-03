using System.Collections.Generic;

public class GameConfig
{
    public static Dictionary<int, int> MonsterSpawnLimits = new Dictionary<int, int>
    {
        {1,3 },
        {2, 5 },
        {3, 8 },
        {4, 10 },
        {5, 10 }
    };

    public static Dictionary<int, int> EnemySpawnUpperLimits = new Dictionary<int, int>
    {
        {1, 4 },
        {2, 6 },
        {3, 8 },
        {4, 12 },
        {5, 12 }
    };

    public static Dictionary<int, int> EnemySpawnLowerLimits = new Dictionary<int, int>
    {
        { 1, 2 },
        { 2, 3 },
        { 3, 5 },
        { 4, 8 },
        { 5, 8 }
    };

    public static Dictionary<int, int> BuildingHP = new Dictionary<int, int>
    {
        {1, 20 },
        {2, 30 },
        {3, 50 },
    };

    public static Dictionary<int, int> BuildingRecoverEachTurn = new Dictionary<int, int>
    {
        {1, 1},
        {2, 2},
        {3, 3},
    };

    public static float EnemyAttackBuildingPriority = 0.5f;

}