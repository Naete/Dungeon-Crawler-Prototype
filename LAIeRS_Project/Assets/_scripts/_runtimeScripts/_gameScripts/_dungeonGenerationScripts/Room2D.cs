using System.Collections.Generic;
using LAIeRS.Miscellanious;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace LAIeRS.DungeonGeneration
{
    public class Room2D
    {
        public GameObject GameObjectReference;
        
        public Tilemap GroundTilemap;
        public Tilemap ObstacleTilemap;
        public Tilemap WallTilemap;

        public Grid2D<bool> GroundLayout;
        public Grid2D<bool> ObstacleLayout;
        public Grid2D<bool> WallLayout;
        
        public List<Room2D> NeighbourRooms { get; }
        public List<Door2D> Doors { get; }
        
        public Vector2Int Position { get; }
        public Vector2Int CenterPos => 
            new Vector2Int(Position.x + (Width / 2), Position.y + (Height / 2));
        
        public int Width { get; }
        public int Height { get; }
        
        public Room2D(int roomWidth, int roomHeight, int roomPosX, int roomPosY)
        {
            GroundTilemap = new Tilemap();
            ObstacleTilemap = new Tilemap();
            WallTilemap = new Tilemap();

            GroundLayout = new Grid2D<bool>(roomWidth, roomHeight, 1, 1, roomPosX, roomPosY);
            ObstacleLayout = new Grid2D<bool>(roomWidth, roomHeight, 1, 1, roomPosX, roomPosY);
            WallLayout = new Grid2D<bool>(roomWidth / 2, roomHeight / 2, 2, 2, roomPosX, roomPosY);
            
            NeighbourRooms = new List<Room2D>();
            Doors = new List<Door2D>();
            
            Width = roomWidth;
            Height = roomHeight;
            
            Position = new Vector2Int(roomPosX, roomPosY);
        }
    }
}