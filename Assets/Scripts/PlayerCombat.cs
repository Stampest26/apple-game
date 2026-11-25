// This script is used to control the player's combat
using UnityEngine; 
using UnityEngine.InputSystem; 

// An array of tool options :)
public enum Tool { None, MudLauncher, Sucker }

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] PlayerController player; 
    [SerializeField] GameObject mudLauncherVisual; 
    [SerializeField] GameObject suckerVisual; 
    [SerializeField] Transform muzzle;
    [SerializeField] MudProjectile mudPrefab; 
    [SerializeField] float fireCooldown = 0.4f;
    [SerializeField] float suckScreenFraction = 0.75f;

    public Tool Current { get; private set; } = Tool.None; 
    float nextFireTime;
    Camera cam;
    CameraFollow cameraFollow;

    void Awake()
    {
        cam = Camera.main;
        cameraFollow = cam.GetComponent<CameraFollow>();
    }

    void Update()
    {
        if (player.InputLocked) return;
        
        // Keyboard controls (J is to equip the mud launcher, K is to equip the sucker)
        // Mayhaps if I add controller support it will be Square or Circle to use tools, X to jump (if we consider the PlayStation controller)
        var kb = Keyboard.current;
        if (kb.jKey.wasPressedThisFrame) Equip(Tool.MudLauncher);
        if (kb.kKey.wasPressedThisFrame) Equip(Tool.Sucker);

        // mousky controls
        var mouse = Mouse.current;
        if (mouse!= null && mouse.leftButton.wasPressedThisFrame) UseTool();
    }

    void Equip(Tool tool)
    {
        // When you press the same key twice the tool you held is put away, very convenient
        Current = Current == tool ? Tool.None : tool;
        mudLauncherVisual.SetActive(Current == Tool.MudLauncher);
        suckerVisual.SetActive(Current == Tool.Sucker);
    }

    void UseTool() 
    {
        if (Current == Tool.MudLauncher) ShootMud();
        else if (Current == Tool.Sucker) Suck();
    }

    void ShootMud()
    {
        if (Time.time < nextFireTime) return;
        nextFireTime = Time.time + fireCooldown;

        Vector2 forward = Vector2.right * player.Facing;
        MudProjectile mud = Instantiate(mudPrefab, muzzle.position, Quaternion.identity);
        mud.Launch(forward);
        cameraFollow.Shake(0.15f, 0.2f);
    }

    void Suck()
    {
        float screenWidth = cam.orthographicSize * 2f * cam.aspect;
        float maxDistance = screenWidth * suckScreenFraction;

        Worm closest = null;
        float closestDistance = float.MaxValue;

        foreach(Worm worm in FindObjectsByType<Worm>())
        {
            float offset = worm.transform.position.x - transform.position.x;
            if (offset * player.Facing <= 0f) continue;
            float distance = Mathf.Abs(offset);
            if (distance > maxDistance) continue;
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = worm;
            }
        }

        if (closest != null) closest.TrySuck(suckerVisual.transform);

    }
}