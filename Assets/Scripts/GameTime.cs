using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTime : MonoBehaviour
{
    public float timeStart = 45f;
    public Text timerText;
    public GameObject player;
    public GameObject winObject;
    public GameObject loseObject;

    private GameObject oxygen;

    private void Start()
    {
        timerText.text = timeStart.ToString();
        oxygen = GameObject.Find("Oxygen");
    }

    private void Update()
    {
        timeStart -= Time.deltaTime;
        timerText.text = Mathf.Round(timeStart).ToString();

        if (timeStart <= 0)
        {
            Time.timeScale = 0f;

            if (oxygen != null && oxygen.transform.IsChildOf(player.transform))
            {
                winObject.SetActive(true);
            }
            else
            {
                loseObject.SetActive(true);
            }
        }
    }
}
