using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    public float speed = 2f;
    public GameObject ExplosionGO;
    private GameObject scoreUITextGO;

    void Start()
    {
        scoreUITextGO = GameObject.FindGameObjectWithTag("ScoreTextTag");
    }

    void Update()
    {
        Vector2 position = transform.position;
        position = new Vector2(position.x, position.y - speed * Time.deltaTime);
        transform.position = position;

        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        if (transform.position.y < min.y)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("PlayerShipTag") || col.CompareTag("PlayerBulletTag"))
        {
            PlayExplosion();

            if (scoreUITextGO != null)
            {
                GameScore gs = scoreUITextGO.GetComponent<GameScore>();
                if (gs != null) gs.Score += 100;
            }

            Destroy(gameObject);
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
}

