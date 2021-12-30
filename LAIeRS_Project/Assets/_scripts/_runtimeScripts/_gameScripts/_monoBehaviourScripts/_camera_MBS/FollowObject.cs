using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float followSpeed;

    private void Update()
    {
        SmoothFollow(transform, target, followSpeed);
        transform.position = new Vector3(transform.position.x, transform.position.y, -1);
    }

    private void SmoothFollow(Transform origin, Transform target, float followSpeed)
    { 
        origin.position = Vector3.Lerp(origin.position, target.position, followSpeed);
    }
}