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
        
        public static Grid2D<Room2D> CreateDungeon(
            int dungeonWidth, int dungeonHeight, int roomWidth, int roomHeight, int roomAmount, Vector2Int gridPos)
        {
            _roomList = new List<Room2D>();
            _roomQueue = new Queue<Room2D>();
            
            _randomizer = new Random();
            
            Grid2D<Room2D> dungeon = 
                new Grid2D<Room2D>(dungeonWidth, dungeonHeight, roomWidth, roomHeight, gridPos.x, gridPos.y);
            
            return GenerateProceduralDungeon(dungeon, roomWidth, roomHeight, roomAmount);
        }

        private static Grid2D<Room2D> GenerateProceduralDungeon(
            Grid2D<Room2D> dungeon, int roomWidth, int roomHeight, int amountOfRoomsToCreate)
        {
            // Adding the start room to the dungeon
            AddRoomToDungeon(dungeon, new Room2D(roomWidth, roomHeight, 0, 0));
            
            while (_roomQueue.Any())
            {
                Room2D selectedRoom = _roomQueue.Dequeue();
                
                List<Vector2Int> randomlyOrderedDirections = GetRandomlyOrderedDirections();
                
                foreach (Vector2Int direction in randomlyOrderedDirections)
                {
                    if (_roomList.HasCountReached(amountOfRoomsToCreate)) break;
                    
                    Vector2Int neighbourRoomPos = GetNeighbourRoomPosOf(selectedRoom, direction);
                    Room2D neighbourRoom = dungeon.GetItemAtPos(neighbourRoomPos.x, neighbourRoomPos.y);
                    
                    if (neighbourRoom != null)
                    {
                        // TODO: Replace magic number "0.2f" by variable and remove the comment
                        // Chance to connect the selected and neighbour room
                        if (Utilities.DiceChance(0.2f) && selectedRoom.NeighbourRooms.NotContains(neighbourRoom))
                            ConnectRooms(selectedRoom, neighbourRoom);
                        
                        continue;
                    }
                    
                    // TODO: Replace magic number "0.5f" by variable and remove the comment
                    // Chance to not create the neighbourRoom
                    if (Utilities.DiceChance(0.5f)) continue;
                    
                    neighbourRoom = new Room2D(roomWidth, roomHeight, neighbourRoomPos.x, neighbourRoomPos.y);
                    
                    AddRoomToDungeon(dungeon, neighbourRoom);
                    ConnectRooms(selectedRoom, neighbourRoom);
                }
                
                if (_roomList.Count.IsLessThan(amountOfRoomsToCreate))
                    _roomQueue.Enqueue(_roomList.GetRandomItem(_randomizer));
            }

            return dungeon;
        }
        
        private static void AddRoomToDungeon(Grid2D<Room2D> dungeon, Room2D room)
        {
            dungeon.SetItemAtPos(room.Position.x, room.Position.y, room);
            
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
        
        private static Vector2Int GetNeighbourRoomPosOf(Room2D selectedRoom, Vector2Int direction)
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
        
        private static bool HasCountReached(this List<Room2D> roomList, int targetNumber)
        {
            return roomList != null && roomList.Count == targetNumber;
        }
    }
}