using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public TMP_Text timeText;
    
    private static TimeManager instance;
    private float startTime;
    private float totalTime;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        totalTime = Time.time - startTime;
        float minutes = Mathf.FloorToInt(totalTime / 60);

        if (timeText != null)
        {
            timeText.text = minutes.ToString("00" + " minutes");
        }
    }
}
