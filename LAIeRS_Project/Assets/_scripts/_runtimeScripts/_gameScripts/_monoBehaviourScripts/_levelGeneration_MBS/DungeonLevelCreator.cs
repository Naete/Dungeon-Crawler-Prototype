using UnityEngine;
using UnityEngine.Tilemaps;

using LAIeRS.Miscellanious;

namespace LAIeRS.DungeonGeneration
{
    public class DungeonLevelCreator : MonoBehaviour
    {
        private Grid2D<Room2D> _gridDungeon;

        [SerializeField] private GameSettings _gameSettings;

        // TODO: Remove start function and generate levels through a level loader class
        private void Start()
        {
            GenerateDungeonLevel();
        }
        
        public void GenerateDungeonLevel()
        {
            // 1. Create grid-based dungeon
            _gridDungeon = GenerateGridDungeon(
                _gameSettings.DungeonSize, _gameSettings.DungeonSize, 
                _gameSettings.RoomWidth, _gameSettings.RoomHeight, 
                _gameSettings.InitialRoomAmount, 
                _gameSettings.InitialGridPosition);

            _gridDungeon.DrawGrid(Color.black, 1000);
            
            //ShowGridMap1();
            ShowGridMap2();
            
            // 2. Create cluster-based dungeon
            
            
            // 3. Connect the dungeons
            
        }
        
        private Grid2D<Room2D> GenerateGridDungeon(
            int dungeonWidth, int dungeonHeight, int roomWidth, int roomHeight, int roomAmount, Vector2Int gridPosition)
        { 
            Grid2D<Room2D> gridDungeon = GridDungeonGenerator2D.CreateGridDungeon(
                    dungeonWidth, dungeonHeight, 
                    roomWidth, roomHeight, 
                    roomAmount, 
                    gridPosition);
            
            // Next steps: generate room content, triggers, ...
            
            
            return gridDungeon;
        }

        private void ShowGridMap1()
        {
            Vector2 padding = new Vector2(4, 4);
            
            _gridDungeon.Foreach(room =>
            {
                if (room != null)
                {
                    Vector2 roomSize = new Vector2(_gameSettings.RoomWidth, _gameSettings.RoomHeight) - padding;
                    
                    if (room.Position == Vector2Int.zero)
                        Visualizer.DrawRectangleAt(
                            room.Position + padding / 2,
                            roomSize,
                            Color.yellow, 
                            100);
                    else
                        Visualizer.DrawRectangleAt(
                            room.Position + padding / 2,
                            roomSize,
                            Color.gray, 
                            100);
                    
                    foreach (Room2D neighbourRoom in room.NeighbourRooms)
                        Visualizer.DrawLine(room.CenterPosition, neighbourRoom.CenterPosition, Color.white, 100);
                }
            });
        }
        
        private void ShowGridMap2() 
        {
            _gridDungeon.Foreach(room =>
            {
                if (room != null)
                {
                    RoomContentGenerator2D.GenerateContentFor(room);
                    
                    // Drawing 1x1 tiles
                    for (int y = 0; y < room.Height; y++) {
                        for (int x = 0; x < room.Width; x++)
                        {
                            if (room.GroundLayout.GetItemAtIndex(x, y))
                                Visualizer.DrawSquareAt(room.GroundLayout.GetPositionAtIndex(x, y), 1, Color.gray, 100);
                    
                            if (room.ObstacleLayout.GetItemAtIndex(x, y))
                                Visualizer.DrawCircle(room.ObstacleLayout.GetPositionAtIndex(x, y) + new Vector2(0.5f, 0.5f), 0.5f, Color.red, 100);
                        }
                    }
                    
                    // Drawing 2x2 tiles
                    for (int y = 0; y < room.WallLayout.Height; y++) {
                        for (int x = 0; x < room.WallLayout.Width; x++)
                        {
                            if (room.WallLayout.GetItemAtIndex(x, y))
                                Visualizer.DrawSquareAt(room.WallLayout.GetPositionAtIndex(x, y), 2, Color.white, 100);
                        }
                    }
                }
            });
        }
    }
}