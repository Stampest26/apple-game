using System.Collections;
using UnityEngine; 
using UnityEngine.InputSystem;

public class TutorialManager : MonoBehaviour
{ 
    [SerializeField] CameraFollow cameraFollow;
    [SerializeField] Transform wormCamPoint;
    // worm worm
    [SerializeField] Worm worm;
    [SerializeField] PlayerRespawn playerRespawn;
    [SerializeField] Transform checkpoint;
    [SerializeField] Transform wormTrigger; 
    [SerializeField] Transform gapEnd;
    [SerializeField] DialogueUI dialogue;
    [SerializeField] PlayerController player;
    [SerializeField] Transform ledgeTrigger;
    [SerializeField] Transform ledgeCamPoint;
    [SerializeField] Transform ledgeCheckpoint;
    [SerializeField] Worm ledgeWorm; 
    [SerializeField] Nectar nectar;
    [SerializeField] float nectarCloseDistance = 5f;

    bool playerDied;

    void Awake() => worm.PlayerKilled += () => playerDied = true;

    void ResetFight()
    {
        cameraFollow.SetTarget(player.transform);
        cameraFollow.ResetZoom();
        dialogue.Hide();
        worm.Frozen = false;
        worm.CanAct = false;
        player.InputLocked = false;
    }

    IEnumerator Start()
    {
        player.InputLocked = true;
        yield return new WaitForSeconds(1f);

        // Tutorial for walking!
        yield return new WaitForSeconds(1f); 
        yield return dialogue.Say(Speaker.Sherry, "Appy, can you hear me?");
        yield return dialogue.Say(Speaker.Sherry, "Do you remember how to walk?");
        yield return dialogue.Say(Speaker.Charlotte, "Gosh, Sherry, you can't just ask people if they remember how to walk!");
        yield return dialogue.Say(Speaker.Sherry, "Sorry, I just remembered that Appy has memory issues..");
        yield return dialogue.Say(Speaker.Sherry, "You know, since the worms took a bite out of his head.");
        player.InputLocked = false; 
        yield return dialogue.Say(Speaker.Sherry, "Anyway, Appy, you can press A and D to walk! Or the arrow keys if you prefer.");
        float startX = player.transform.position.x;
        yield return new WaitUntil(() => Mathf.Abs(player.transform.position.x - startX) > 4f);

        //----------------------------------------------------------------------------------------------------------------------------------------------

        // Tutorial for jumping! 
        player.InputLocked = true;
        yield return dialogue.Say(Speaker.Sherry, "Nice! I knew you could do it!");
        yield return dialogue.Say(Speaker.Charlotte, "Appy, wait! Do you see that gap?");
        player.InputLocked = false;
        yield return dialogue.Say(Speaker.Charlotte, "Press Space to jump over it!");
        yield return new WaitUntil(() => player.transform.position.x > gapEnd.position.x);
        playerRespawn.SetRespawnPoint(checkpoint);
        dialogue.Hide();

        //----------------------------------------------------------------------------------------------------------------------------------------------

        // Tutorial for fighting!
        bool wormDefeated = false;
        while (!wormDefeated) 
        {
            playerDied = false;
            yield return new WaitUntil(() => player.transform.position.x > wormTrigger.position.x);
            player.InputLocked = true;
            yield return dialogue.Say(Speaker.Both, "A WORM!");
            cameraFollow.SetTarget(wormCamPoint);
            cameraFollow.SetZoom(6f);
            yield return dialogue.Say(Speaker.Sherry, "Careful, Appy! These beings thirst for fruit flesh!");
            yield return dialogue.Say(Speaker.Charlotte, "Quick, shoot it with the mud launcher! Take it out with J and use it with the Left Click!");
            worm.CanAct = true;
            worm.Alert();
            player.InputLocked = false;
            yield return new WaitUntil(() => worm.State == WormState.InMud || playerDied);
            if (playerDied) { ResetFight(); continue; }
            yield return new WaitForSeconds(0.5f);
            if (playerDied) { ResetFight(); continue; }
            worm.Frozen = true;
            player.InputLocked = true;
            yield return dialogue.Say(Speaker.Charlotte, "Appy, the worm crawls faster while it's in the mud!");
            yield return dialogue.Say(Speaker.Sherry, "Quick, take out the sucker with K and suck it up with the Left Click!");
            worm.Frozen = false;
            player.InputLocked = false; 
            yield return new WaitUntil(() => worm.State == WormState.Sucked || playerDied);
            if (playerDied) { ResetFight(); continue; }
            wormDefeated = true;            
        }

        yield return new WaitUntil(() => worm.Collected);
        player.InputLocked = true;
        yield return dialogue.Say(Speaker.Sherry, "Nice job, you got rid of that pesky worm!");
        cameraFollow.SetTarget(player.transform);
        cameraFollow.ResetZoom();
        yield return dialogue.Say(Speaker.Sherry, "Did you see that? You got a coin!");
        yield return dialogue.Say(Speaker.Sherry, "You get coins each time you defeat an enemy.");
        yield return dialogue.Say(Speaker.Charlotte, "We have no idea what they do, though.");
        yield return dialogue.Say(Speaker.Charlotte, "You're smart, Appy! I'm sure you'll find a use for them!");
        yield return dialogue.Say(Speaker.Sherry, "Onwards!");
        dialogue.Hide();
        player.InputLocked = false; 

        //----------------------------------------------------------------------------------------------------------------------------------------------

        // Tutorial for nectar!
        yield return new WaitUntil(() => player.transform.position.x > ledgeTrigger.position.x);
        player.InputLocked = true;
        playerRespawn.SetRespawnPoint(ledgeCheckpoint);

        // Whole speech they give you
        yield return dialogue.Say(Speaker.Sherry, "Look!");
        cameraFollow.SetTarget(ledgeCamPoint);
        cameraFollow.SetZoom(7f);
        yield return dialogue.Say(Speaker.Charlotte, "See that glowing orb? That's nectar!");
        yield return dialogue.Say(Speaker.Sherry, "It has the power to save our forest!");
        yield return dialogue.Say(Speaker.Sherry, "Go get it!");

        cameraFollow.SetTarget(player.transform);
        cameraFollow.ResetZoom();
        dialogue.Hide();
        player.InputLocked = false;
        ledgeWorm.CanAct = true;

        yield return new WaitUntil(() => Vector2.Distance(player.transform.position, nectar.transform.position) < nectarCloseDistance);
        player.InputLocked = true;
        ledgeWorm.Frozen = true;
        yield return dialogue.Say(Speaker.Sherry, "That's the nectar, Appy! It has healing properties.");
        yield return dialogue.Say(Speaker.Charlotte, "With it, we can heal everyone in the outpost and heal the forest!");
        yield return dialogue.Say(Speaker.Charlotte, "Go on, grab it! And no, you can't eat it.");
        dialogue.Hide();
        player.InputLocked = false; 

        // Next, the rest of the level and the outpost
    }
}