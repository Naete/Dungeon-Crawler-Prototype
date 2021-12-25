using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Game Settings", order = 0)]
public class GameSettings : ScriptableObject
{
    // TODO: Make members immutable
    public int DungeonSize => InitialRoomAmount * 2;
    
    public int InitialRoomAmount = 5;
    
    [Space]
    public int RoomWidth = 26;
    public int RoomHeight = 14;
    
    // NOTE: The dungeon generation is not depending on this calculation,
    // so it can be removed when not needed anymore
    public Vector2Int InitialGridPosition => new Vector2Int(
        -InitialRoomAmount * RoomWidth,
        -InitialRoomAmount * RoomHeight);
    
    public const float CHANCE_CONNECT_ROOMS = 0.25f;
    public const float CHANCE_NOT_CREATE_ROOM = 0.5f;
}
