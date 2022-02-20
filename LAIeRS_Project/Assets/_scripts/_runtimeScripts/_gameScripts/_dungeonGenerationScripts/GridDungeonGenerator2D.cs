using System.Collections.Generic;
using System.Linq;
using Random = System.Random;

using UnityEngine;

using LAIeRS.ExtensiveMethods;
using LAIeRS.Miscellanious;

namespace LAIeRS.DungeonGeneration
{
    public static class GridDungeonGenerator2D
    {
        private static List<Room2D> _roomList;
        private static Queue<Room2D> _roomQueue;

        private static Random _randomizer;
        
        private static List<Vector2Int> _directions = new List<Vector2Int>()
        {
            Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left
        };
        
        private static Queue<RoomType> _roomTypesToCreate;
        
        public static Grid2D<Room2D> CreateGridDungeon(
            int dungeonWidth, int dungeonHeight, int roomWidth, int roomHeight, int roomAmount, Vector2Int gridPosition)
        {
            _roomList = new List<Room2D>();
            _roomQueue = new Queue<Room2D>();
            
            _randomizer = new Random();
            
            Grid2D<Room2D> dungeonGrid = 
                new Grid2D<Room2D>(dungeonWidth, dungeonHeight, roomWidth, roomHeight, gridPosition.x, gridPosition.y);
            
            SetRoomTypesToCreate(roomAmount);
            
            return GenerateProceduralDungeon(dungeonGrid, roomWidth, roomHeight, roomAmount);
        }
        
        private static Grid2D<Room2D> GenerateProceduralDungeon(
            Grid2D<Room2D> dungeonGrid, int roomWidth, int roomHeight, int amountOfRoomsToCreate)
        {
            // Adding the start room to the dungeon
            AddRoomToDungeon(dungeonGrid, new Room2D(roomWidth, roomHeight, 0, 0, _roomTypesToCreate.Dequeue()));
            
            while (_roomQueue.Any())
            {
                Room2D selectedRoom = _roomQueue.Dequeue();
                
                foreach (Vector2Int direction in GetRandomlyOrderedDirections())
                {
                    if (_roomList.Count.IsGreaterOrEqual(amountOfRoomsToCreate)) break;
                    
                    Vector2Int neighbourRoomPosition = GetNeighbourRoomPositionOf(selectedRoom, direction);
                    Room2D neighbourRoom = dungeonGrid.GetItemAtPosition(neighbourRoomPosition.x, neighbourRoomPosition.y);
                    
                    if (neighbourRoom != null)
                    {
                        if (Utilities.DiceChance(GameSettings.CHANCE_CONNECT_ROOMS) 
                            && selectedRoom.NeighbourRooms.NotContains(neighbourRoom))
                            ConnectRooms(selectedRoom, neighbourRoom);
                        
                        continue;
                    }
                    
                    if (Utilities.DiceChance(GameSettings.CHANCE_NOT_CREATE_ROOM)) continue;
                    
                    neighbourRoom = 
                        new Room2D(
                            roomWidth, roomHeight,
                            neighbourRoomPosition.x, neighbourRoomPosition.y, 
                            _roomTypesToCreate.Dequeue());
                    
                    AddRoomToDungeon(dungeonGrid, neighbourRoom);
                    
                    ConnectRooms(selectedRoom, neighbourRoom);
                }
                
                if (_roomList.Count.IsLessThan(amountOfRoomsToCreate))
                    _roomQueue.Enqueue(_roomList.GetRandomItem(_randomizer));
            }
            
            return dungeonGrid;
        }
        
        private static void AddRoomToDungeon(Grid2D<Room2D> dungeon, Room2D room)
        {
            dungeon.SetItemAtPosition(room.Position.x, room.Position.y, room);
            
            _roomList.Add(room);
            _roomQueue.Enqueue(room);
        }
        
        private static List<Vector2Int> GetRandomlyOrderedDirections()
        {
            List<Vector2Int> randomlyOrderedDirections = new List<Vector2Int>();

            while (randomlyOrderedDirections.Count != _directions.Count)
            {
                int randomIndex = _randomizer.Next(_directions.Count);
            
                Vector2Int direction = _directions[randomIndex];
                
                if (randomlyOrderedDirections.NotContains(direction))
                    randomlyOrderedDirections.Add(direction);
            }
            
            return randomlyOrderedDirections;
        }
        
        private static Vector2Int GetNeighbourRoomPositionOf(Room2D selectedRoom, Vector2Int direction)
        {
            return new Vector2Int(
                selectedRoom.Position.x + (direction.x * selectedRoom.Width),
                selectedRoom.Position.y + (direction.y * selectedRoom.Height));
        }
        
        private static void ConnectRooms(Room2D roomA, Room2D roomB)
        {
            roomA.NeighbourRooms.Add(roomB);
            roomB.NeighbourRooms.Add(roomA);
        }
        
        private static void SetRoomTypesToCreate(int roomAmount)
        {
            _roomTypesToCreate = new Queue<RoomType>(new [] { RoomType.START });

            List<RoomType> listOfRoomTypes = new List<RoomType>(GameSettings.defaultRoomTypes);

            roomAmount -= listOfRoomTypes.Count + _roomTypesToCreate.Count;

            float maxItemsWeight = 0;

            List<KeyValuePair<RoomType, float>> optionalRoomTypes = new List<KeyValuePair<RoomType, float>>();
            
            foreach (var roomType in GameSettings.optionalRoomTypes)
            {
                maxItemsWeight += roomType.Value;
                optionalRoomTypes.Add(roomType);
            }
            
            for (int i = 0; i < roomAmount; i++)
            {
                float randomValue = UnityEngine.Random.Range(0, maxItemsWeight);

                for (int j = 0; j < optionalRoomTypes.Count; j++)
                {
                    var roomType = optionalRoomTypes[j];

                    if (randomValue <= roomType.Value)
                    {
                        listOfRoomTypes.Add(roomType.Key);
                        break;
                    }
                    
                    randomValue -= roomType.Value;
                }
            }

            listOfRoomTypes.Shuffle();
            
            foreach (RoomType roomType in listOfRoomTypes)
                _roomTypesToCreate.Enqueue(roomType);
        }
    }
}