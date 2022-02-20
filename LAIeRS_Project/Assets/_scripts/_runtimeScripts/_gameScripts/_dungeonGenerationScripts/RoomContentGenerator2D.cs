using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

using LAIeRS.DungeonGeneration;
using LAIeRS.ExtensiveMethods;

public static class RoomContentGenerator2D
{
    public static void GenerateContentFor(Room2D room)
    {
        GenerateObstacleLayoutFor(room);
        
        GenerateWallLayoutFor(room);
    }

    // TODO: Refactor
    public static void CreateEntrancesFor(Room2D room, Action<Vector2> CreateDoorAt)
    {
        Tilemap wallTilemap = 
            room.GameObjectReference.transform.Find("Grid 2x2").transform.Find("Walls").GetComponent<Tilemap>();

        int tilemapHalfWidth = Mathf.CeilToInt((float)room.Width / 4);
        int tilemapHalfHeight = Mathf.CeilToInt((float)room.Height / 4);
        Vector2Int tilemapCenterPosition = new Vector2Int(tilemapHalfWidth - 1, tilemapHalfHeight - 1);
                    
        Vector2 roomPosition = room.Position;
                    
        foreach (Room2D neighbourRoom in room.NeighbourRooms)
        {
            Vector2Int directionToNeighbourRoom = 
                Vector2Int.RoundToInt(roomPosition.GetDirectionTo(neighbourRoom.Position));

            Vector3Int targetPosition = (Vector3Int)tilemapCenterPosition;

            while (true)
            {
                if (wallTilemap.GetTile(targetPosition) != null)
                {
                    wallTilemap.SetTile(targetPosition, null);
                    CreateDoorAt((Vector3)roomPosition + (Vector3)targetPosition * 2);
                    break;
                }

                if (targetPosition.x <= 0 || targetPosition.y <= 0 ||
                    targetPosition.x >= wallTilemap.size.x - 1 ||
                    targetPosition.y >= wallTilemap.size.y - 1)
                    break;

                targetPosition += (Vector3Int)directionToNeighbourRoom;
            }
        }
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