using UnityEngine;

public class Planet : MonoBehaviour
{
    public float speed = -0.5f;
    public bool isMoving = false;

    Vector2 min;
    Vector2 max;

    void Awake()
    {
        isMoving = false;

        min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null && sr.sprite != null)
        {
            max.y += sr.sprite.bounds.extents.y;
            min.y -= sr.sprite.bounds.extents.y;
        }
    }

    void Update()
    {
        if (!isMoving) return;

        Vector2 position = transform.position;
        position = new Vector2(position.x, position.y + speed * Time.deltaTime);
        transform.position = position;

        if (transform.position.y < min.y)
        {
            isMoving = false;
        }
    }

    public void ResetPosition()
    {
        transform.position = new Vector2(Random.Range(min.x, max.x), max.y);
    }
}

