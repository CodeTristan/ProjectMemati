using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform playerTransform;
    public Vector3 offset = new Vector3(0, 10, 0); 

    void Start()
    {
        if (playerTransform != null)
        {
            transform.position = playerTransform.position + offset;
            transform.LookAt(playerTransform);
        }

    }

    void LateUpdate()
    {
        if (playerTransform != null)
        {
            transform.position = playerTransform.position + offset;
            transform.LookAt(playerTransform);
        }
    }
}