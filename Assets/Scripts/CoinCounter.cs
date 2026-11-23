// This script is in charge of showing the coin counter on screen. Might modify it to show a coin sprite later on instead. 
using TMPro; 
using UnityEngine;

public class CoinCounter : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    public int Coins { get; private set; }

    void Start() => Refresh();

    // Function that adds coins to your coin counter. 
    public void Add(int amount)
    {
        Coins += amount;
        Refresh();
    }
    void Refresh() => text.text = "Coins: " + Coins;
}