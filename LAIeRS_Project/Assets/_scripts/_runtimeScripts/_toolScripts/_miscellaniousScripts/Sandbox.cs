using System.Collections.Generic;
using UnityEngine;

using LAIeRS.Events;

namespace LAIeRS.Sandbox
{
    public class Sandbox : MonoBehaviour
    {
        [SerializeField] private List<Animator> doorAnimator;
    
        private void Awake()
        {
            EventManager.AddListenerTo(EventID.ON_OPEN_DOORS, OpenDoor);
            EventManager.AddListenerTo(EventID.ON_CLOSE_DOORS, CloseDoor);
        }

        private void OpenDoor()
        {
            foreach (var animator in doorAnimator)
            {
                if (!animator.GetBool("IsOpen"))
                {
                    animator.SetTrigger("DoorTrigger");
                    animator.SetBool("IsOpen", true);
                }
            }
        }

        private void CloseDoor()
        {
            foreach (var animator in doorAnimator)
            {
                if (animator.GetBool("IsOpen"))
                {
                    animator.SetTrigger("DoorTrigger");
                    animator.SetBool("IsOpen", false);
                }
            }
        }
    }
}