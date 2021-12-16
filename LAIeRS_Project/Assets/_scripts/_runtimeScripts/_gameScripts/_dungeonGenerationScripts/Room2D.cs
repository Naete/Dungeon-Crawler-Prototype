using System.Collections.Generic;
using UnityEngine;

namespace LAIeRS.DungeonGeneration
{
    public class Room2D
    {
        public List<Room2D> NeighbourRooms;
        
        public Vector2Int Position { get; }
        
        public int Width { get; }
        public int Height { get; }

        public Room2D(int roomWidth, int roomHeight, int roomPosX, int roomPosY)
        {
            NeighbourRooms = new List<Room2D>();
            
            Width = roomWidth;
            Height = roomHeight;
            
            Position = new Vector2Int(roomPosX, roomPosY);
        }
    }
}