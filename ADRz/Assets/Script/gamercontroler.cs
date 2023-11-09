using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gamercontroler : MonoBehaviour
{
    
    public Text coinText;
    private int coinCount = 0;

    void Start()
    {
        UpdateCoinText();
    }

    public void AddCoin()
    {
        coinCount++;
        UpdateCoinText();
    }

    void UpdateCoinText()
    {
        if (coinText != null)
        {
            coinText.text = "x " + coinCount.ToString();
        }
    }
}
