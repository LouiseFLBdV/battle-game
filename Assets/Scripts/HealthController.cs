using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    public PlayerController player;
    public Image hpBar;
    private float maxHP = 100;

    private void Start()
    {
        player.hp = maxHP;
    }


    void FixedUpdate()
    {
        hpBar.fillAmount = player.hp / maxHP;
        if (hpBar.fillAmount <= 0.71)
        {
            hpBar.color = Color.yellow;
        } 
        if (hpBar.fillAmount <= 0.35)
        {
            hpBar.color = Color.red;
        } 
        if (hpBar.fillAmount > 0.71)
        {
            hpBar.color = Color.green;
        }
    }
}
