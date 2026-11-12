using UnityEngine;
using UnityEditor;

public class CheckAssets
{
    public static void Run()
    {
        Debug.Log("Checking assets...");
        var spaceshipAssets = AssetDatabase.LoadAllAssetsAtPath("Assets/Sprites/spaceships.png");
        Debug.Log("Spaceship assets count: " + spaceshipAssets.Length);
        foreach (var a in spaceshipAssets)
        {
            if (a is Sprite s) Debug.Log("Sprite: " + s.name);
        }

        var bulletAssets = AssetDatabase.LoadAllAssetsAtPath("Assets/Sprites/bullets.png");
        Debug.Log("Bullet assets count: " + bulletAssets.Length);
        foreach (var a in bulletAssets)
        {
            if (a is Sprite s) Debug.Log("Sprite: " + s.name);
        }

        var explosionAssets = AssetDatabase.LoadAllAssetsAtPath("Assets/Sprites/explosion.png");
        Debug.Log("Explosion assets count: " + explosionAssets.Length);
        foreach (var a in explosionAssets)
        {
            if (a is Sprite s) Debug.Log("Sprite: " + s.name);
        }
    }
}

