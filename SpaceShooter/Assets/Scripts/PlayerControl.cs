using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerControl : MonoBehaviour
{
    public float speed = 8f;
    public GameObject GameManagerGO;
    public GameObject bulletPosition01;
    public GameObject bulletPosition02;
    public GameObject PlayerBulletGO;
    public GameObject ExplosionGO;

    public TMP_Text LivesTMPText;
    public Text LivesUIText;

    const int MaxLives = 3;
    int lives;

    public void Init()
    {
        lives = MaxLives;
        UpdateLivesUI();
        transform.position = new Vector2(0, 0);
        gameObject.SetActive(true);
    }

    void Start()
    {
        // Auto-assign LivesUIText or LivesTMPText if not wired in Inspector
        if (LivesTMPText == null && LivesUIText == null)
        {
            GameObject livesObj = GameObject.Find("TextLives");
            if (livesObj != null)
            {
                LivesTMPText = livesObj.GetComponent<TMP_Text>();
                LivesUIText = livesObj.GetComponent<Text>();
            }
        }
        UpdateLivesUI();
    }

    void Update()
    {
        // 1. Fire on Space key
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }

        // 2. Movement Input: Keyboard + Accelerometer
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        // Accelerometer fallback/mobile support
        if (Mathf.Abs(x) < 0.1f && Mathf.Abs(y) < 0.1f)
        {
            x = Input.acceleration.x * 2f;
            y = Input.acceleration.y * 2f;
        }

        Vector2 direction = new Vector2(x, y).normalized;
        Move(direction);
    }

    void Move(Vector2 direction)
    {
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        max.x -= 0.28f;
        min.x += 0.28f;
        max.y -= 0.35f;
        min.y += 0.35f;

        Vector2 pos = transform.position;
        pos += direction * speed * Time.deltaTime;

        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        pos.y = Mathf.Clamp(pos.y, min.y, max.y);

        transform.position = pos;
    }

    // Public method callable by UI Button
    public void Shoot()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.Play();
        }

        if (PlayerBulletGO != null)
        {
            if (bulletPosition01 != null)
            {
                GameObject bullet01 = Instantiate(PlayerBulletGO);
                bullet01.transform.position = bulletPosition01.transform.position;
            }
            if (bulletPosition02 != null)
            {
                GameObject bullet02 = Instantiate(PlayerBulletGO);
                bullet02.transform.position = bulletPosition02.transform.position;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("EnemyShipTag") || col.CompareTag("EnemyBulletTag"))
        {
            PlayExplosion();

            lives--;
            UpdateLivesUI();

            if (lives <= 0)
            {
                if (GameManagerGO != null)
                {
                    GameManager gm = GameManagerGO.GetComponent<GameManager>();
                    if (gm != null)
                    {
                        gm.SetGameManagerState(GameManager.GameManagerState.GameOver);
                    }
                }
                gameObject.SetActive(false);
            }
        }
    }

    void PlayExplosion()
    {
        if (ExplosionGO != null)
        {
            GameObject explosion = Instantiate(ExplosionGO);
            explosion.transform.position = transform.position;
        }
    }

    void UpdateLivesUI()
    {
        string livesStr = lives.ToString();
        if (LivesTMPText != null) LivesTMPText.text = livesStr;
        if (LivesUIText != null) LivesUIText.text = livesStr;
    }
}

