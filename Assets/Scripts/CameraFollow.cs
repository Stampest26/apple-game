// This script lets the camera follow the character which is useful to a proper sidescroller experience!
// Lets script know what to use 
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Basically instantiating stuff
    [SerializeField] Transform target;
    [SerializeField] float smoothing = 6f;
    [SerializeField] Vector3 offset = new Vector3(0, 1.5f, -10f);

    Camera cam; 
    float defaultSize;
    float targetSize;

    void Awake()
    {
        cam = GetComponent<Camera>();
        defaultSize = cam.orthographicSize;
        targetSize = defaultSize;
    }

    public void SetTarget(Transform newTarget) => target = newTarget;
    public void SetZoom(float size) => targetSize = size; 
    public void ResetZoom() => targetSize = defaultSize; 

    void LateUpdate()
    {
        if (target == null) return;
        transform.position = Vector3.Lerp(transform.position, target.position + offset, smoothing * Time.deltaTime);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, smoothing * Time.deltaTime);
    }
}    