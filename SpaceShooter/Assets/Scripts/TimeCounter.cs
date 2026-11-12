using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeCounter : MonoBehaviour
{
    private TMP_Text timeTMP;
    private Text timeText;

    private float startTime;
    private float elapsedTime;
    private bool startCounter;

    private int minutes;
    private int seconds;

    void Awake()
    {
        timeTMP = GetComponent<TMP_Text>();
        timeText = GetComponent<Text>();
    }

    void Start()
    {
        startCounter = false;
        SetText("00:00");
    }

    public void StartTimeCounter()
    {
        startTime = Time.time;
        startCounter = true;
    }

    public void StopTimeCounter()
    {
        startCounter = false;
    }

    void Update()
    {
        if (startCounter)
        {
            elapsedTime = Time.time - startTime;
            minutes = (int)(elapsedTime / 60);
            seconds = (int)(elapsedTime % 60);

            SetText(string.Format("{0:00}:{1:00}", minutes, seconds));
        }
    }

    void SetText(string text)
    {
        if (timeTMP != null) timeTMP.text = text;
        if (timeText != null) timeText.text = text;
    }
}

