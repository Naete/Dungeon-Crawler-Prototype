using System.Collections.Generic;
using UnityEngine;

using LAIeRS.Miscellanious;

namespace LAIeRS.DungeonGeneration
{
    public class DungeonLevelCreator : MonoBehaviour
    {
        private Grid2D<Room2D> _gridDungeon;
        
        public void GenerateDungeonLevel()
        {
            // 1. Create grid-based dungeon
            _gridDungeon = GenerateGridDungeon();
            
            // 2. Create cluster-based dungeon
            
            
            // 3. Connect the dungeons
            
        }
        
        private Grid2D<Room2D> GenerateGridDungeon()
        {
            Grid2D<Room2D> gridDungeon = GridDungeonGenerator2D.CreateGridDungeon(10, 10, 8, 5, 6);

            // Next steps: generate room content, triggers, ...
            
            return gridDungeon;
        }
    }
}