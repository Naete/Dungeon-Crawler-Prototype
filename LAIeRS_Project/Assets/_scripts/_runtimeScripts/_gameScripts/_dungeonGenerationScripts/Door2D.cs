using UnityEngine;

namespace LAIeRS.DungeonGeneration
{
    public class Door2D
    {
        public GameObject GameObjectReference { get; }
        
        public Vector2Int Position { get; }

        public Door2D(Vector2Int doorPos)
        {
            Position = doorPos;
        }
    }
}