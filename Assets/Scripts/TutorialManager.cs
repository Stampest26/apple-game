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
        yield return new WaitUntil(() => player.transform.position.x > wormTrigger.position.x);
        player.InputLocked = true; 
        yield return dialogue.Say(Speaker.Both, "A WORM!");
        // This part makes the camera move to the middle of the scene, so the worm has more space to crawl at you!
        cameraFollow.SetTarget(wormCamPoint);
        cameraFollow.SetZoom(6f);
        yield return dialogue.Say(Speaker.Sherry, "Careful, Appy! These beings thirst for fruit flesh!");
        yield return dialogue.Say(Speaker.Charlotte, "Quick, shoot it with the mud launcher! Take it out with J and use it with the Left Click!");
        // Makes the worm be able to run at you
        worm.CanAct = true; 
        worm.Alert();
        player.InputLocked = false;
        // Waits until the worm is covered in mud, and then lets it run at the player for a bit (so they can see the speed change)
        yield return new WaitUntil(() => worm.State == WormState.InMud);
        yield return new WaitForSeconds(0.5f);
        // Then stops for more dialogue 
        worm.Frozen = true;
        player.InputLocked = true;
        yield return dialogue.Say(Speaker.Sherry, "Appy, the worm runs faster while it's in the mud!");
        yield return dialogue.Say(Speaker.Charlotte, "Quickly, take out the sucker with K and suck it up with the Left Click!");
        // Worm may be too fast here, may edit later
        worm.Frozen = false;
        player.InputLocked = false; 
        yield return new WaitUntil(() => worm.State == WormState.Sucked);
        player.InputLocked = true;
        yield return dialogue.Say(Speaker.Sherry, "Nice job, you got rid of the pesky worm!");
        cameraFollow.SetTarget(player.transform);
        cameraFollow.ResetZoom();
        dialogue.Hide();
        player.InputLocked = false; 

        // Worm fighting and retrieving nectar is next. Then Appy will arrive at the outpost and meet all of the other fruits.
        
    }
}