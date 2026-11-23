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
    [SerializeField] float suckReach = 2f;
    [SerializeField] Vector2 suckBoxSize = new Vector2(3f, 1.5f);

    public Tool Current { get; private set; } = Tool.None; 
    float nextFireTime;

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
        Vector2 forward = Vector2.right * player.Facing;

        if (Current == Tool.MudLauncher && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireCooldown;
            MudProjectile mud = Instantiate(mudPrefab, muzzle.position, Quaternion.identity);
            mud.Launch(forward);
        }
        else if (Current == Tool.Sucker)
        {
            Vector2 center = (Vector2)transform.position + forward * suckReach;
            foreach (Collider2D hit in Physics2D.OverlapBoxAll(center, suckBoxSize, 0f))
            {
                if (hit.TryGetComponent(out Worm worm)) worm.TrySuck(); 
            }
        }
    }
}