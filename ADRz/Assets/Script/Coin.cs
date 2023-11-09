using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI; 

using UnityEngine;

public class Coin : MonoBehaviour
{
    public Text textmoeda;
    public int coletor;
    public gamercontroler oi;
    
    void Start()
    {
        textmoeda.text = "x " + coletor.ToString();
        oi = FindObjectOfType<gamercontroler>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            oi.AddCoin();
            Destroy(gameObject);
        }
    }
}
