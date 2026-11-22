// This script lets the camera follow the character which is useful to a proper sidescroller experience!
// Lets script know what to use 
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float smoothing = 6f;
    [SerializeField] Vector3 offset = new Vector3(0, 1.5f, -10f);

    void LateUpdate()
    {
        if (target == null) return;
        transform.position = Vector3.Lerp(transform.position, target.position + offset, smoothing * Time.deltaTime);
    }
}