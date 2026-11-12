using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameScore : MonoBehaviour
{
    private TMP_Text scoreTMP;
    private Text scoreText;
    private int score = 0;

    public int Score
    {
        get { return score; }
        set
        {
            score = value;
            UpdateScoreTextUI();
        }
    }

    void Awake()
    {
        scoreTMP = GetComponent<TMP_Text>();
        scoreText = GetComponent<Text>();
    }

    void Start()
    {
        UpdateScoreTextUI();
    }

    void UpdateScoreTextUI()
    {
        string scoreStr = string.Format("{0:000000}", score);
        if (scoreTMP != null) scoreTMP.text = scoreStr;
        if (scoreText != null) scoreText.text = scoreStr;
    }
}

