using System.Collections.Generic;

using UnityEngine;

using LAIeRS.Miscellanious;

namespace LAIeRS.DungeonGeneration
{
    public static class GridDungeonGenerator2D
    {
        private static List<Room2D> _roomList;
        private static Queue<Room2D> _roomQueue;

        private static List<Vector2> _directions = new List<Vector2>()
        {
            Vector2.up, Vector2.right, Vector2.down, Vector2.left
        };
        
        public static Grid2D<Room2D> CreateGridDungeon(
            int dungeonWidth, int dungeonHeight, int roomWidth, int roomHeight, int roomAmount)
        {
            _roomList = new List<Room2D>();
            _roomQueue = new Queue<Room2D>();
            
            // TODO: Is it important to have a specific initial grid position than (x: 0, y: 0)?
            Grid2D<Room2D> gridDungeon = 
                new Grid2D<Room2D>(dungeonWidth, dungeonHeight, roomWidth, roomHeight, 0, 0);
            
            return GenerateProceduralDungeon(gridDungeon, roomWidth, roomHeight, roomAmount);
        }

        private static Grid2D<Room2D> GenerateProceduralDungeon(
            Grid2D<Room2D> gridDungeon, int roomWidth, int roomHeight, int roomAmount)
        {
            

            return gridDungeon;
        }
    }
}