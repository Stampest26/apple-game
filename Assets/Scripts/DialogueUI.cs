// Lets script know what to use 
using System.Collections;
using TMPro;
using UnityEngine;

public enum Speaker { Sherry, Charlotte, Both }

public class DialogueUI : MonoBehaviour
{
    // Basically instantiating stuff
    [SerializeField] GameObject panel;
    [SerializeField] RectTransform sherryPortrait;
    [SerializeField] RectTransform charlottePortrait;
    [SerializeField] TMP_Text sherryText;
    [SerializeField] TMP_Text charlotteText;
    [SerializeField] TMP_Text bothText;
    [SerializeField] float speakingScale = 1.2f;
    [SerializeField] float idleScale = 0.8f;
    [SerializeField] float charsPerSecond = 40f;
    [SerializeField] float holdTime = 1.5f;

    void Awake() => panel.SetActive(false);

    public IEnumerator Say(Speaker speaker, string line)
    {
        panel.SetActive(true);

        // Instead of making it charBig = speaker = Speaker.Char, it makes it charBig = speaker != Speaker.Char2. 
        // This is so that it is easier to make it so that both characters are big when they both speak. 
        bool sherryBig = speaker != Speaker.Charlotte; 
        bool charlotteBig = speaker != Speaker.Sherry; 
        sherryPortrait.localScale = Vector3.one * (sherryBig ? speakingScale : idleScale);
        charlottePortrait.localScale = Vector3.one * (charlotteBig ? speakingScale : idleScale);

        sherryText.gameObject.SetActive(speaker == Speaker.Sherry);
        charlotteText.gameObject.SetActive(speaker == Speaker.Charlotte); 
        bothText.gameObject.SetActive(speaker == Speaker.Both);

        TMP_Text active = speaker == Speaker.Sherry ? sherryText
                        : speaker == Speaker.Charlotte ? charlotteText
                        : bothText;

        active.text = line;
        for (int i = 0; i <= line.Length; i++)
        {
            active.maxVisibleCharacters = i;
            yield return new WaitForSeconds(1f / charsPerSecond);
        }
        yield return new WaitForSeconds(holdTime);
    }
    public void Hide() => panel.SetActive(false);
}