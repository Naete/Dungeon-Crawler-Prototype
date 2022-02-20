using Random = UnityEngine.Random;

public static class CellularAutomata
{
    private static int _iterations = 10;
    private static float _noiseDensity = 0.63f;
    
    public static bool[,] CreateClusterDungeon(int dungeonWidth, int dungeonHeight)
    {
        bool[,] clusterDungeon = CreateNoiseDungeon(dungeonWidth, dungeonHeight);

        clusterDungeon = ApplyCellularAutomataTo(clusterDungeon, _iterations);

        clusterDungeon = SmoothOutClusterEdgesOf(clusterDungeon);

        return clusterDungeon;
    }
    
    private static bool[,] CreateNoiseDungeon(int dungeonWidth, int dungeonHeight)
    {
        bool[,] noiseArray = new bool[dungeonWidth, dungeonHeight];
        
        for (int y = 0; y < dungeonHeight; y++) {
            for (int x = 0; x < dungeonWidth; x++)
            {
                float value = Random.value;
                
                noiseArray[x, y] = value <= _noiseDensity;
            }
        }
        
        return noiseArray;
    }
    
    private static bool[,] ApplyCellularAutomataTo(bool[,] clusterDungeon, int iterations)
    {
        int dungeonWidth = clusterDungeon.GetLength(0);
        int dungeonHeight = clusterDungeon.GetLength(1);
        
        bool[,] updatedClusterDungeon = new bool[dungeonWidth, dungeonHeight];
        
        for (int i = 0; i < iterations; i++)
        {
            for (int y = 0; y < dungeonHeight; y++) {
                for (int x = 0; x < dungeonWidth; x++)
                {
                    int numberOfNeighbours = GetNumberOfNeighbours(clusterDungeon, x, y);

                    // Cellular Automata Rule
                    updatedClusterDungeon[x, y] = (numberOfNeighbours > 4);
                }
            }

            clusterDungeon = updatedClusterDungeon.Clone() as bool[,];
        }

        return clusterDungeon;
    }
    
    private static int GetNumberOfNeighbours(bool[,] clusterDungeon, int x, int y)
    {
        int dungeonWidth = clusterDungeon.GetLength(0);
        int dungeonHeight = clusterDungeon.GetLength(1);
        
        int numberOfNeighbours = 0;
        
        for (int y2 = y - 1; y2 <= y + 1; y2++) {
            for (int x2 = x - 1; x2 <= x + 1; x2++)
            {
                if (y2 < 0 || y2 >= dungeonHeight ||
                    x2 < 0 || x2 >= dungeonWidth ||
                    (x2 == x && y2 == y))
                    continue;
                            
                if (clusterDungeon[x2, y2])
                    numberOfNeighbours++;
            }
        }

        return numberOfNeighbours;
    }

    private static bool[,] SmoothOutClusterEdgesOf(bool[,] clusterMap)
    {
        for (int y = 0; y < clusterMap.GetLength(1); y++) {
            for (int x = 0; x < clusterMap.GetLength(0); x++)
            {
                if (GetNumberOfNeighbours(clusterMap, x, y) <= 3)
                    clusterMap[x, y] = false;
            }
        }
        
        return clusterMap;
    }
}
