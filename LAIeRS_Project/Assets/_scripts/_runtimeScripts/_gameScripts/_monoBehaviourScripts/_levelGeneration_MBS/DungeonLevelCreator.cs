using UnityEngine;

using LAIeRS.Miscellanious;

namespace LAIeRS.DungeonGeneration
{
    public class DungeonLevelCreator : MonoBehaviour
    {
        private Grid2D<Room2D> _gridDungeon;

        // TODO: Move the members to appropriate class
        private int _dungeonSize;
        [SerializeField] private int _roomWidth = 20;
        [SerializeField] private int _roomHeight = 13;
        [SerializeField] private int _roomAmount = 10;
        private Vector2Int _gridPos;
        
        // TODO: Remove start function
        private void Start()
        {
            _dungeonSize = _roomAmount * 2;
            
            // Calculation to enable the dungeon generation to start from the grid center than left bottom corner
            // NOTE: The dungeon generation is not depending on this calculation,
            // so it can be removed when not needed anymore
            _gridPos = new Vector2Int(
                -_roomAmount * _roomWidth,
                -_roomAmount * _roomHeight);
            
            GenerateDungeonLevel();
        }
        
        public void GenerateDungeonLevel()
        {
            // 1. Create grid-based dungeon
            _gridDungeon = GenerateGridDungeon(
                _dungeonSize, _dungeonSize, 
                _roomWidth, _roomHeight, 
                _roomAmount, 
                _gridPos);
            
            
            _gridDungeon.DrawGrid(Color.white, 1000);
            
            _gridDungeon.Foreach(room =>
            {
                if (room != null)
                    Visualizer.DrawCircle(room.Position + new Vector2Int(room.Width / 2, room.Height / 2), 5, Color.red, 100);
            });
            
            // 2. Create cluster-based dungeon
            
            
            // 3. Connect the dungeons
            
        }
        
        private Grid2D<Room2D> GenerateGridDungeon(
            int dungeonWidth, int dungeonHeight, int roomWidth, int roomHeight, int roomAmount, Vector2Int gridPos)
        {
            Grid2D<Room2D> gridDungeon = GridDungeonGenerator2D.CreateDungeon(
                    dungeonWidth, dungeonHeight, 
                    roomWidth, roomHeight, 
                    roomAmount, 
                    gridPos);

            // Next steps: generate room content, triggers, ...
            
            return gridDungeon;
        }
    }
}