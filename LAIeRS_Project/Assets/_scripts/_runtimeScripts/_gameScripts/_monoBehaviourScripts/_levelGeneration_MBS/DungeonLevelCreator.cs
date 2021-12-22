using System.Collections.Generic;
using UnityEngine;

using LAIeRS.Miscellanious;
using UnityEngine.Tilemaps;

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
        
        // TODO: Remove this and load assets from a asset loader
        [SerializeField] private TileBase _wallTile;
        [SerializeField] private Tilemap _tilemap;
        
        // TODO: Remove start function and generate levels through a level loader class
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

            _gridDungeon.DrawGrid(Color.black, 1000);
            
            //ShowGridMap1();
            ShowGridMap2();
            
            // 2. Create cluster-based dungeon
            
            
            // 3. Connect the dungeons
            
        }
        
        private Grid2D<Room2D> GenerateGridDungeon(
            int dungeonWidth, int dungeonHeight, int roomWidth, int roomHeight, int roomAmount, Vector2Int gridPos)
        {
            Grid2D<Room2D> gridDungeon = GridDungeonGenerator2D.CreateGridDungeon(
                    dungeonWidth, dungeonHeight, 
                    roomWidth, roomHeight, 
                    roomAmount, 
                    gridPos);
            
            // Next steps: generate room content, triggers, ...
            // List< (int i, int j)> skipIndexes = new List<(int i, int j)>();
            //
            // for (int j = 0; j < roomHeight / 2; j++)
            // {
            //     for (int i = 0; i < roomWidth / 2; i++)
            //     {
            //         if (i >= 1 && j >= 1 && i < roomWidth / 2 - 1 && j < roomHeight / 2 - 1)
            //             skipIndexes.Add((i, j));
            //     }
            // }
            //
            // List<GameObject> tileMaps = new List<GameObject>();
            //
            // gridDungeon.Foreach(room =>
            // {
            //     if (room != null)
            //     {
            //         Vector3 pos = (Vector2)room.Position;
            //         Tilemap tilemap = Instantiate(_tilemap, pos, Quaternion.identity).GetComponent<Tilemap>();
            //         RoomContentGenerator2D.GenerateTilesFor(tilemap, _wallTile, room, 2, skipIndexes);
            //         room.WallTilemap = tilemap;
            //     }
            // });
            
            //gridDungeon.GetItemAtIndex(0, 0).WallTilemap = _tilemap;
            
            return gridDungeon;
        }

        private void ShowGridMap1()
        {
            _gridDungeon.Foreach(room =>
            {
                if (room != null)
                {
                    if (room.Position == Vector2Int.zero)
                        Visualizer.DrawSquareAt(room.CenterPos - new Vector2Int(5, 5), 10, Color.yellow, 100);
                    else
                        Visualizer.DrawSquareAt(room.CenterPos - new Vector2Int(5, 5), 10, Color.gray, 100);
                    
                    foreach (var neighbourRoom in room.NeighbourRooms)
                        Visualizer.DrawLine(room.CenterPos, neighbourRoom.CenterPos, Color.white, 100);
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
                                Visualizer.DrawSquareAt(room.GroundLayout.GetPosAtIndex(x, y), 1, Color.gray, 100);
                    
                            if (room.ObstacleLayout.GetItemAtIndex(x, y))
                                Visualizer.DrawCircle(room.ObstacleLayout.GetPosAtIndex(x, y) + new Vector2(0.5f, 0.5f), 0.5f, Color.red, 100);
                        }
                    }
                    
                    // Drawing 2x2 tiles
                    for (int y = 0; y < room.WallLayout.Height; y++) {
                        for (int x = 0; x < room.WallLayout.Width; x++)
                        {
                            if (room.WallLayout.GetItemAtIndex(x, y))
                                Visualizer.DrawSquareAt(room.WallLayout.GetPosAtIndex(x, y), 2, Color.white, 100);
                        }
                    }
                }
            });
        }
    }
    
}