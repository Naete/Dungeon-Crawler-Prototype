using System.Collections.Generic;

using UnityEngine;

using LAIeRS.DungeonGeneration;

namespace LAIeRS.Settings
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Game Settings", order = 0)]
    public class GameSettings : ScriptableObject
    {
        public int DungeonSize => InitialRoomAmount * 2;
        public int ClusterDungeonWidth => (int)(DungeonSize / 2.5f * RoomWidth);
        public int ClusterDungeonHeigth => (int)(DungeonSize / 2.5f * RoomHeight);
        
        public int InitialRoomAmount = 5;
        
        [Space]
        public int RoomWidth = 26;
        public int RoomHeight = 14;
        
        // NOTE: The dungeon generation is not depending on this calculation,
        // so it can be removed when not needed anymore
        public Vector2Int InitialGridDungeonPosition => new Vector2Int(
            -InitialRoomAmount * RoomWidth,
            -InitialRoomAmount * RoomHeight);
        
        public Vector2Int InitialClusterDungeonPosition => new Vector2Int(
            -ClusterDungeonWidth / 2,
            -ClusterDungeonHeigth / 2);
        
        public const float CHANCE_CONNECT_ROOMS = 0.25f;
        public const float CHANCE_NOT_CREATE_ROOM = 0.5f;
        
        public static List<RoomType> defaultRoomTypes = new List<RoomType>(new[]
        {
            RoomType.CROSSING, RoomType.CROSSING, RoomType.ITEM, RoomType.BOSS 
        });
        
        public static Dictionary<RoomType, float> optionalRoomTypes = new Dictionary<RoomType, float>()
        {
            {RoomType.EMPTY, 0.50f}, {RoomType.ENEMY, 0.35f}, {RoomType.PUZZLE, 0.15f}
        };
    }
}
