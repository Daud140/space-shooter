using UnityEngine;

public class Destroyer : MonoBehaviour
{
    // Called by Animation Event at the end of ExplosionAnimation
    void DestroyGameObject()
    {
        Destroy(gameObject);
    }

    // Safety fallback in case animation event is missed
    void Start()
    {
        Destroy(gameObject, 1.2f);
    }
}

