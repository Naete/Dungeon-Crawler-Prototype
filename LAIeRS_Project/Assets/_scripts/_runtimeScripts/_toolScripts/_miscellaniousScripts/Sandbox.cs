using UnityEngine;

using LAIeRS.Events;

public class Sandbox : MonoBehaviour
{
    [SerializeField] private BoxCollider2D doorCollider;
    
    private void Awake()
    {
        EventManager.AddListenerTo(EventID.ON_OPEN_DOORS, OpenDoors);
        EventManager.AddListenerTo(EventID.ON_CLOSE_DOORS, CloseDoors);
    }

    private void OpenDoors()
    {
        doorCollider.enabled = false;
    }
    
    private void CloseDoors()
    {
        doorCollider.enabled = true;
    }
}
