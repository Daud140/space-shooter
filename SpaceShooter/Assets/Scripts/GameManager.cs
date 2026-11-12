using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playButton;
    public GameObject playerShip;
    public GameObject enemySpawner;
    public GameObject GameOverGO;
    public GameObject scoreUITextGO;
    public GameObject TimeCounterGO;
    public GameObject GameTitleGO;
    public GameObject ShootButton;

    public enum GameManagerState
    {
        Opening,
        Gameplay,
        GameOver
    }

    private GameManagerState GMState;

    void Start()
    {
        GMState = GameManagerState.Opening;
        UpdateGameManagerState();
    }

    void UpdateGameManagerState()
    {
        switch (GMState)
        {
            case GameManagerState.Opening:
                if (GameOverGO != null) GameOverGO.SetActive(false);
                if (playButton != null) playButton.SetActive(true);
                if (GameTitleGO != null) GameTitleGO.SetActive(true);
                if (ShootButton != null) ShootButton.SetActive(false);

                if (enemySpawner != null)
                {
                    EnemySpawner spawner = enemySpawner.GetComponent<EnemySpawner>();
                    if (spawner != null) spawner.UnscheduleEnemySpawner();
                }

                // Clean up any remaining enemies from previous round
                GameObject[] remainingEnemies = GameObject.FindGameObjectsWithTag("EnemyShipTag");
                foreach (GameObject enemy in remainingEnemies)
                {
                    Destroy(enemy);
                }
                break;

            case GameManagerState.Gameplay:
                if (scoreUITextGO != null)
                {
                    GameScore gs = scoreUITextGO.GetComponent<GameScore>();
                    if (gs != null) gs.Score = 0;
                }

                if (playButton != null) playButton.SetActive(false);
                if (GameTitleGO != null) GameTitleGO.SetActive(false);
                if (GameOverGO != null) GameOverGO.SetActive(false);
                if (ShootButton != null) ShootButton.SetActive(true);

                if (playerShip != null)
                {
                    PlayerControl pc = playerShip.GetComponent<PlayerControl>();
                    if (pc != null) pc.Init();
                }

                if (enemySpawner != null)
                {
                    EnemySpawner spawner = enemySpawner.GetComponent<EnemySpawner>();
                    if (spawner != null) spawner.ScheduleEnemySpawner();
                }

                if (TimeCounterGO != null)
                {
                    TimeCounter tc = TimeCounterGO.GetComponent<TimeCounter>();
                    if (tc != null) tc.StartTimeCounter();
                }
                break;

            case GameManagerState.GameOver:
                if (TimeCounterGO != null)
                {
                    TimeCounter tc = TimeCounterGO.GetComponent<TimeCounter>();
                    if (tc != null) tc.StopTimeCounter();
                }

                if (GameOverGO != null) GameOverGO.SetActive(true);
                if (ShootButton != null) ShootButton.SetActive(false);

                if (enemySpawner != null)
                {
                    EnemySpawner spawner = enemySpawner.GetComponent<EnemySpawner>();
                    if (spawner != null) spawner.UnscheduleEnemySpawner();
                }

                Invoke("ChangeToOpeningState", 6f);
                break;
        }
    }

    public void SetGameManagerState(GameManagerState state)
    {
        GMState = state;
        UpdateGameManagerState();
    }

    // Called by Play Button OnClick
    public void StartgamePlay()
    {
        GMState = GameManagerState.Gameplay;
        UpdateGameManagerState();
    }

    public void ChangeToOpeningState()
    {
        SetGameManagerState(GameManagerState.Opening);
    }
}

