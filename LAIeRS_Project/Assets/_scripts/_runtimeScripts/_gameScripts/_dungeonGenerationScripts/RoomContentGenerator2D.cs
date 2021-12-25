using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;

using LAIeRS.DungeonGeneration;

public static class RoomContentGenerator2D
{
    public static void GenerateContentFor(Room2D room)
    {
        GenerateObstacleLayoutFor(room);
        
        GenerateWallLayoutFor(room);
    }

    private static void GenerateObstacleLayoutFor(Room2D room)
    {
        for (int y = 0; y < room.Height; y++) {
            for (int x = 0; x < room.Width; x++)
            {
                room.GroundLayout.SetItemAtIndex(x, y, true);
            }
        }

        int amountOfObstacles = 10;
        List<(int x, int y)> randomPositions = new List<(int x, int y)>();
        
        for (int i = 0; i < amountOfObstacles; i++)
        {
            int y = Random.Range(2, room.Height - 3);
            int x = Random.Range(4, room.Width - 4);

            if (randomPositions.Contains((x, y)))
            {
                amountOfObstacles--;
                continue;
            }

            room.ObstacleLayout.SetItemAtIndex(x, y, true);
            randomPositions.Add((x, y));
        }
    }
    
    private static void GenerateWallLayoutFor(Room2D room)
    {
        for (int y = 0; y < room.WallLayout.Height; y++) {
            for (int x = 0; x < room.WallLayout.Width; x++)
            {
                if (x > 0 && x < room.WallLayout.Width - 1 
                          && !(x >= 2 && x <= room.WallLayout.Width - 3 && y >= 1 && y <= room.WallLayout.Height - 2))
                    room.WallLayout.SetItemAtIndex(x, y, true);
            }
        }
    }
    
}