using UnityEngine;

using LAIeRS.Player;

namespace LAIeRS.DungeonGeneration
{
    public class Trigger2D : MonoBehaviour
    {
        public Vector2Int Position { get; }
        
        // TODO: Each trigger must have a specific event type which the trigger will call
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.GetComponent<PlayerIdentifier>())
            {
                // TODO: Trigger event
                
            }
        }
    }
}