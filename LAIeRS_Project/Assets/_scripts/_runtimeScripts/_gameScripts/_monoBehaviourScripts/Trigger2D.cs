using UnityEngine;

using LAIeRS.Player;

namespace LAIeRS.Events
{
    public class Trigger2D : MonoBehaviour
    {
        public Vector2Int Position { get; }
        
        [SerializeField] private EventID _eventID;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.GetComponent<PlayerIdentifier>())
            {
                EventManager.TriggerEvent(_eventID);
            }
        }
    }
}