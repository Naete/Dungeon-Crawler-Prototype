using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

using LAIeRS.Events;
using LAIeRS.ExtensiveMethods;
using LAIeRS.Miscellanious;
using LAIeRS.Settings;

namespace LAIeRS.DungeonGeneration
{
    public class DungeonLevelCreator : MonoBehaviour
    {
        private Grid2D<Room2D> _gridDungeon;
        private bool[,] _clusterDungeonAsBools;
        [SerializeField] private GameObject _clusterDungeon;
        
        [SerializeField] private GameSettings _gameSettings;
        [SerializeField] private List<GameObject> _testRoomPrefabs;
        [SerializeField] private GameObject _testDoorPrefab;
        [SerializeField] private GameObject _ladderPrefab;
        [SerializeField] private GameObject _triggerPrefab;
        [SerializeField] private Tile _groundTile;
        [SerializeField] private Tile _unwalkableTile;
        [SerializeField] private TileBase _wallTile;
        
        private List<Animator> _blockadeAnimators;

        // TODO: Remove start function and generate levels through a level loader class
        private void Start()
        {
            _blockadeAnimators = new List<Animator>();
            
            EventManager.AddListenerTo(EventID.ON_CROSSING, SwitchBetweenDungeons);
            EventManager.AddListenerTo(EventID.ON_RELEASE_BLOCKADE, ReleaseBlockade);
            EventManager.AddListenerTo(EventID.ON_UNRELEASE_BLOCKADE, UnreleaseBlockade);
            
            GenerateDungeonLevel();
        }
        
        public void GenerateDungeonLevel()
        {
            // 1. Create grid-based dungeon
            _gridDungeon = GenerateGridDungeon(
                _gameSettings.DungeonSize, _gameSettings.DungeonSize, 
                _gameSettings.RoomWidth, _gameSettings.RoomHeight, 
                _gameSettings.InitialRoomAmount, 
                _gameSettings.InitialGridDungeonPosition);
            
            // 2. Create cluster-based dungeon
            _clusterDungeonAsBools = GenerateClusterDungeon(
                _gameSettings.ClusterDungeonWidth, _gameSettings.ClusterDungeonHeigth);
            
            ShowClusterMap();
            
            // 3. Connect the dungeons
            
            _gridDungeon.DrawGrid(Color.black, 1000);
        }
        
        private Grid2D<Room2D> GenerateGridDungeon(
            int dungeonWidth, int dungeonHeight, int roomWidth, int roomHeight, int roomAmount, Vector2Int gridPosition)
        {
            Grid2D<Room2D> gridDungeon = GridDungeonGenerator2D.CreateGridDungeon(
                    dungeonWidth, dungeonHeight, 
                    roomWidth, roomHeight, 
                    roomAmount, 
                    gridPosition);
            
            System.Random randomizer = new System.Random();
            
            // Generate rooms
            gridDungeon.Foreach(room =>
            {
                room.GameObjectReference = 
                    Instantiate(
                        _testRoomPrefabs.GetRandomItem(randomizer), 
                        (Vector2)room.Position, 
                        Quaternion.identity);

                RoomContentGenerator2D.CreateEntrancesFor(room, doorPosition =>
                {
                    GameObject door = Instantiate(_testDoorPrefab, doorPosition, Quaternion.identity);

                    door.transform.parent = room.GameObjectReference.transform.Find("Grid 2x2").Find("Doors").transform;
                    
                    _blockadeAnimators.Add(door.GetComponent<Animator>());
                });
                
                GameObject trigger = Instantiate(_triggerPrefab, room.CenterPosition + new Vector2(-2, -3), Quaternion.identity);
                GameObject trigger2 = Instantiate(_triggerPrefab, room.CenterPosition + new Vector2(1, 2), Quaternion.identity);

                trigger2.GetComponent<Trigger2D>()._eventID = EventID.ON_UNRELEASE_BLOCKADE;
                
                trigger.transform.parent =
                    room.GameObjectReference.transform.Find("Grid 1x1").Find("Triggers").transform;
                
                trigger2.transform.parent =
                    room.GameObjectReference.transform.Find("Grid 1x1").Find("Triggers").transform;
            });

            return gridDungeon;
        }
        
        private bool[,] GenerateClusterDungeon(int dungeonWidth, int dungeonHeight)
        {
            bool[,] clusterDungeon = CellularAutomata.CreateClusterDungeon(dungeonWidth, dungeonHeight);
            
            List<Room2D> crossingRooms = new List<Room2D>();
            
            _gridDungeon.Foreach(room =>
            {
                if (room.Type == RoomType.CROSSING)
                {
                    crossingRooms.Add(room);
                    Visualizer.DrawCircle(room.CenterPosition, 2, Color.red, 1000);
                }
            });
            
            foreach (Room2D crossingRoom in crossingRooms)
            {
                for (int y = 0; y < crossingRoom.Height; y++) {
                    for (int x = 0; x < crossingRoom.Width; x++)
                    {
                        Vector2Int roomPositionInClusterDungeon = 
                            crossingRoom.Position - _gameSettings.InitialClusterDungeonPosition;
                        
                        clusterDungeon[x + roomPositionInClusterDungeon.x, y + roomPositionInClusterDungeon.y] = true;
                    }
                }
                
                Instantiate(_ladderPrefab, (Vector2)crossingRoom.CenterPosition, Quaternion.identity);
            }
            
            for (int y = 0; y < _gameSettings.ClusterDungeonHeigth; y++) {
                for (int x = 0; x < _gameSettings.ClusterDungeonWidth; x++)
                {
                    if (clusterDungeon[x, y])
                    {
                        List<(int x, int y)> emptyNeighbours = GetEmptyNeighbours(clusterDungeon, x, y);
                        
                        if (emptyNeighbours.Count > 0)
                        {
                            _clusterDungeon.transform.Find("Walls").GetComponent<Tilemap>().SetTile(
                                new Vector3Int(
                                    x - _gameSettings.ClusterDungeonWidth / 2, 
                                    y - _gameSettings.ClusterDungeonHeigth / 2, 0),
                                _wallTile);
                        }
                        else
                        {
                            _clusterDungeon.transform.Find("Ground").GetComponent<Tilemap>().SetTile(
                                new Vector3Int(
                                    x - _gameSettings.ClusterDungeonWidth / 2, 
                                    y - _gameSettings.ClusterDungeonHeigth / 2, 0),
                                _groundTile);    
                        }
                    }
                    else
                    {
                        _clusterDungeon.transform.Find("Unwalkables").GetComponent<Tilemap>().SetTile(
                            new Vector3Int(
                                x - _gameSettings.ClusterDungeonWidth / 2, 
                                y - _gameSettings.ClusterDungeonHeigth / 2, 0),
                            _unwalkableTile);
                    }
                }
            }
            
            return clusterDungeon;
        }
        
        // TODO: Remove all following functions
        private void ShowGridMap1()
        {
            Vector2 padding = new Vector2(4, 4);
            
            _gridDungeon.Foreach(room =>
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
            });
        }
        
        private void ShowGridMap2() 
        {
            _gridDungeon.Foreach(room =>
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
            });
        }
        
        private void ShowClusterMap()
        {
            for (int j = 0; j < _clusterDungeonAsBools.GetLength(1); j++) {
                for (int i = 0; i < _clusterDungeonAsBools.GetLength(0); i++)
                {
                    if (_clusterDungeonAsBools[i, j]) 
                        Visualizer.DrawSquareAt(
                            _gameSettings.InitialClusterDungeonPosition + new Vector2(i, j), 1f, Color.black, 1000);
                }
            }
        }
        
        private void SwitchBetweenDungeons()
        {
            if (_clusterDungeon.gameObject.activeInHierarchy)
            {
                _clusterDungeon.gameObject.SetActive(false);
                _gridDungeon.Foreach(room => { room.GameObjectReference.SetActive(true);});
            }
            else
            {
                _clusterDungeon.gameObject.SetActive(true);
                _gridDungeon.Foreach(room => { room.GameObjectReference.SetActive(false);});
            }
        }

        private static List<(int x, int y)> GetEmptyNeighbours(bool[,] clusterDungeon, int x, int y)
        {
            List<(int x, int y)> emptyNeighbours = new List<(int x, int y)>();
            
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

                    if (!clusterDungeon[x2, y2])
                        emptyNeighbours.Add((x2, y2));
                }
            }

            return emptyNeighbours;
        }
        
        private void ReleaseBlockade()
        {
            foreach (var animator in _blockadeAnimators)
            {
                if (!animator.GetBool("IsReleased"))
                {
                    animator.SetTrigger("BlockadeTrigger");
                    animator.SetBool("IsReleased", true);
                }
            }
        }
        
        private void UnreleaseBlockade()
        {
            foreach (var animator in _blockadeAnimators)
            {
                if (animator.GetBool("IsReleased"))
                {
                    animator.SetTrigger("BlockadeTrigger");
                    animator.SetBool("IsReleased", false);
                }
            }
        }
    }
}