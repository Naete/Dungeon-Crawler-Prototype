using System.Collections.Generic;
using UnityEngine;

using LAIeRS.Events;

namespace LAIeRS.Sandbox
{
    public class Sandbox : MonoBehaviour
    {
        [SerializeField] private List<Animator> blockadeAnimator;
        
        private void Awake()
        {
            EventManager.AddListenerTo(EventID.ON_RELEASE_BLOCKADE, ReleaseBlockade);
            EventManager.AddListenerTo(EventID.ON_UNRELEASE_BLOCKADE, UnreleaseBlockade);
        }
        
        private void ReleaseBlockade()
        {
            foreach (var animator in blockadeAnimator)
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
            foreach (var animator in blockadeAnimator)
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