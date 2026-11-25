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

    Vector3 basePosition;
    float shakeTimeLeft;
    float shakeDuration;
    float shakeMagnitude;

    void Awake()
    {
        cam = GetComponent<Camera>();
        defaultSize = cam.orthographicSize;
        targetSize = defaultSize;
        basePosition = transform.position;
    }

    public void SetTarget(Transform newTarget) => target = newTarget;
    public void SetZoom(float size) => targetSize = size;
    public void ResetZoom() => targetSize = defaultSize;

    public void Shake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeTimeLeft = duration;
        shakeMagnitude = magnitude;
    }

    void LateUpdate()
    {
        if (target == null) return;
        basePosition = Vector3.Lerp(basePosition, target.position + offset, smoothing * Time.deltaTime);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, smoothing * Time.deltaTime);
        Vector3 shake = Vector3.zero;
        if (shakeTimeLeft > 0f)
        {
            shakeTimeLeft -= Time.deltaTime;
            float strength = shakeMagnitude * (shakeTimeLeft / shakeDuration );
            shake = (Vector3)(Random.insideUnitCircle * strength);
        }

        transform.position = basePosition + shake;
    }
}    