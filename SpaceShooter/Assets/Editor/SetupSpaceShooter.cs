using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Events;
using UnityEngine.Events;

public class SetupSpaceShooter
{
    private static Sprite FindSprite(string path, string spriteName)
    {
        Object[] assets = AssetDatabase.LoadAllAssetsAtPath(path);
        foreach (Object o in assets)
        {
            if (o is Sprite s && s.name == spriteName) return s;
        }
        return AssetDatabase.LoadAssetAtPath<Sprite>(path);
    }

    [MenuItem("SpaceShooter/Build Project")]
    public static void BuildProject()
    {
        Debug.Log("Starting Space Shooter Project Setup...");

        // Ensure directories
        if (!AssetDatabase.IsValidFolder("Assets/Prefabs"))
            AssetDatabase.CreateFolder("Assets", "Prefabs");
        if (!AssetDatabase.IsValidFolder("Assets/Scenes"))
            AssetDatabase.CreateFolder("Assets", "Scenes");

        // 1. Load Audio Clips
        AudioClip bgMusic = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Audio/BackgroundMusic.mp3");
        AudioClip explosionClip = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Audio/Explosion2.wav");
        AudioClip laserClip = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Audio/Laser_Shoot16.wav");

        // 2. Load Controllers
        RuntimeAnimatorController playerAnimCtrl = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>("Assets/Animations/PlayerAnimController.controller");
        RuntimeAnimatorController enemyAnimCtrl = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>("Assets/Animations/EnemyAnimController.controller");
        RuntimeAnimatorController explosionAnimCtrl = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>("Assets/Animations/ExplosionAnimController.controller");

        // 3. Load Sprites
        Sprite playerSprite0 = FindSprite("Assets/Sprites/spaceships.png", "spaceships_0");
        Sprite enemySprite0 = FindSprite("Assets/Sprites/spaceships.png", "spaceships_4");
        Sprite playerBulletSprite = FindSprite("Assets/Sprites/bullets.png", "bullets_0");
        Sprite enemyBulletSprite = FindSprite("Assets/Sprites/bullets.png", "bullets_1");
        Sprite explosionSprite0 = FindSprite("Assets/Sprites/explosion.png", "explosion_0");

        Sprite starSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/Star.png");
        Sprite planetBlue = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/planetBlue.png");
        Sprite planetGreen = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/planetGreen.png");
        Sprite planetPink = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/planetPink.png");

        Sprite gameTitleSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/gameTitle.png");
        Sprite playBtnSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/playButton.png");
        Sprite playBtnHighSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/playButtonHighlighted.png");
        Sprite gameOverSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/gameOver.png");
        Sprite scoreBgSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/scoreUIBg.png");
        Sprite livesBgSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/livesUIBg.png");
        Sprite timeBgSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/timeUIBg.png");
        Sprite shootBtnSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/shoot_button02.png");

        Font standardFont = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        if (standardFont == null) standardFont = Resources.GetBuiltinResource<Font>("Arial.ttf");

        // ==========================================
        // 4. Create PREFABS
        // ==========================================

        // --- A. PlayerBulletGO Prefab ---
        GameObject pbGO = new GameObject("PlayerBulletGO");
        pbGO.tag = "PlayerBulletTag";
        SpriteRenderer pbSR = pbGO.AddComponent<SpriteRenderer>();
        pbSR.sprite = playerBulletSprite;
        pbSR.sortingOrder = 2;
        Rigidbody2D pbRB = pbGO.AddComponent<Rigidbody2D>();
        pbRB.bodyType = RigidbodyType2D.Kinematic;
        BoxCollider2D pbCol = pbGO.AddComponent<BoxCollider2D>();
        pbCol.isTrigger = true;
        pbCol.size = new Vector2(0.12f, 0.28f);
        pbGO.AddComponent<PlayerBullet>();
        GameObject playerBulletPrefab = PrefabUtility.SaveAsPrefabAsset(pbGO, "Assets/Prefabs/PlayerBulletGO.prefab");
        Object.DestroyImmediate(pbGO);

        // --- B. EnemyBulletGO Prefab ---
        GameObject ebGO = new GameObject("EnemyBulletGO");
        ebGO.tag = "EnemyBulletTag";
        SpriteRenderer ebSR = ebGO.AddComponent<SpriteRenderer>();
        ebSR.sprite = enemyBulletSprite;
        ebSR.sortingOrder = 2;
        Rigidbody2D ebRB = ebGO.AddComponent<Rigidbody2D>();
        ebRB.bodyType = RigidbodyType2D.Kinematic;
        CircleCollider2D ebCol = ebGO.AddComponent<CircleCollider2D>();
        ebCol.isTrigger = true;
        ebCol.radius = 0.12f;
        ebGO.AddComponent<EnemyBullet>();
        GameObject enemyBulletPrefab = PrefabUtility.SaveAsPrefabAsset(ebGO, "Assets/Prefabs/EnemyBulletGO.prefab");
        Object.DestroyImmediate(ebGO);

        // --- C. ExplosionGO Prefab ---
        GameObject exGO = new GameObject("ExplosionGO");
        SpriteRenderer exSR = exGO.AddComponent<SpriteRenderer>();
        exSR.sprite = explosionSprite0;
        exSR.sortingOrder = 5;
        Animator exAnim = exGO.AddComponent<Animator>();
        exAnim.runtimeAnimatorController = explosionAnimCtrl;
        exGO.AddComponent<Destroyer>();
        AudioSource exAudio = exGO.AddComponent<AudioSource>();
        exAudio.clip = explosionClip;
        exAudio.playOnAwake = true;
        GameObject explosionPrefab = PrefabUtility.SaveAsPrefabAsset(exGO, "Assets/Prefabs/ExplosionGO.prefab");
        Object.DestroyImmediate(exGO);

        // --- D. StarGO Prefab ---
        GameObject starGO = new GameObject("StarGO");
        SpriteRenderer starSR = starGO.AddComponent<SpriteRenderer>();
        starSR.sprite = starSprite;
        starSR.sortingOrder = -5;
        starGO.AddComponent<Star>();
        GameObject starPrefab = PrefabUtility.SaveAsPrefabAsset(starGO, "Assets/Prefabs/StarGO.prefab");
        Object.DestroyImmediate(starGO);

        // --- E. EnemyGO Prefab ---
        GameObject enemyGO = new GameObject("EnemyGO");
        enemyGO.tag = "EnemyShipTag";
        SpriteRenderer enemySR = enemyGO.AddComponent<SpriteRenderer>();
        enemySR.sprite = enemySprite0;
        enemySR.sortingOrder = 1;
        Animator enemyAnim = enemyGO.AddComponent<Animator>();
        enemyAnim.runtimeAnimatorController = enemyAnimCtrl;
        Rigidbody2D enemyRB = enemyGO.AddComponent<Rigidbody2D>();
        enemyRB.bodyType = RigidbodyType2D.Kinematic;
        PolygonCollider2D enemyCol = enemyGO.AddComponent<PolygonCollider2D>();
        enemyCol.isTrigger = true;
        EnemyControl ec = enemyGO.AddComponent<EnemyControl>();
        ec.speed = 2f;
        ec.ExplosionGO = explosionPrefab;

        // EnemyGun child
        GameObject enemyGunGO = new GameObject("EnemyGun");
        enemyGunGO.transform.parent = enemyGO.transform;
        enemyGunGO.transform.localPosition = new Vector3(0, -0.2f, 0);
        EnemyGun eg = enemyGunGO.AddComponent<EnemyGun>();
        eg.EnemyBulletGO = enemyBulletPrefab;

        GameObject enemyPrefab = PrefabUtility.SaveAsPrefabAsset(enemyGO, "Assets/Prefabs/EnemyGO.prefab");
        Object.DestroyImmediate(enemyGO);

        // ==========================================
        // 5. Build MAIN SCENE
        // ==========================================
        var newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        // --- A. Main Camera ---
        GameObject camGO = new GameObject("Main Camera");
        Camera cam = camGO.AddComponent<Camera>();
        cam.orthographic = true;
        cam.orthographicSize = 5f;
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = Color.black;
        camGO.AddComponent<AudioListener>();
        camGO.transform.position = new Vector3(0, 0, -10);
        camGO.tag = "MainCamera";

        // --- B. Background Music ---
        GameObject bgmGO = new GameObject("BackgroundMusicGO");
        AudioSource bgmSource = bgmGO.AddComponent<AudioSource>();
        bgmSource.clip = bgMusic;
        bgmSource.loop = true;
        bgmSource.playOnAwake = true;
        bgmSource.volume = 0.5f;

        // --- C. Star Generator ---
        GameObject starGenGO = new GameObject("StarGeneratorGO");
        StarGenerator sg = starGenGO.AddComponent<StarGenerator>();
        sg.StarGO = starPrefab;
        sg.MaxStars = 35;

        // --- D. Planet Controller ---
        GameObject planetCtrlGO = new GameObject("PlanetControllerGO");
        PlanetController pcCtrl = planetCtrlGO.AddComponent<PlanetController>();

        GameObject p1 = new GameObject("Planet01");
        p1.transform.parent = planetCtrlGO.transform;
        p1.transform.position = new Vector3(0, -20, 0);
        SpriteRenderer p1SR = p1.AddComponent<SpriteRenderer>();
        p1SR.sprite = planetBlue;
        p1SR.sortingOrder = -3;
        Planet pl1 = p1.AddComponent<Planet>();
        pl1.speed = -0.5f;

        GameObject p2 = new GameObject("Planet02");
        p2.transform.parent = planetCtrlGO.transform;
        p2.transform.position = new Vector3(0, -20, 0);
        SpriteRenderer p2SR = p2.AddComponent<SpriteRenderer>();
        p2SR.sprite = planetGreen;
        p2SR.sortingOrder = -3;
        Planet pl2 = p2.AddComponent<Planet>();
        pl2.speed = -0.4f;

        GameObject p3 = new GameObject("Planet03");
        p3.transform.parent = planetCtrlGO.transform;
        p3.transform.position = new Vector3(0, -20, 0);
        SpriteRenderer p3SR = p3.AddComponent<SpriteRenderer>();
        p3SR.sprite = planetPink;
        p3SR.sortingOrder = -3;
        Planet pl3 = p3.AddComponent<Planet>();
        pl3.speed = -0.3f;

        pcCtrl.Planets = new GameObject[] { p1, p2, p3 };

        // --- E. Enemy Spawner ---
        GameObject spawnerGO = new GameObject("EnemySpawnerGO");
        EnemySpawner spawner = spawnerGO.AddComponent<EnemySpawner>();
        spawner.EnemyGO = enemyPrefab;

        // --- F. GameManager GO ---
        GameObject gmGO = new GameObject("GameManagerGO");
        GameManager gm = gmGO.AddComponent<GameManager>();

        // --- G. Player GO ---
        GameObject playerGO = new GameObject("PlayerGO");
        playerGO.tag = "PlayerShipTag";
        playerGO.transform.position = Vector3.zero;
        SpriteRenderer playerSR = playerGO.AddComponent<SpriteRenderer>();
        playerSR.sprite = playerSprite0;
        playerSR.sortingOrder = 1;
        Animator playerAnim = playerGO.AddComponent<Animator>();
        playerAnim.runtimeAnimatorController = playerAnimCtrl;
        Rigidbody2D playerRB = playerGO.AddComponent<Rigidbody2D>();
        playerRB.bodyType = RigidbodyType2D.Kinematic;
        PolygonCollider2D playerCol = playerGO.AddComponent<PolygonCollider2D>();
        playerCol.isTrigger = true;
        AudioSource playerAudio = playerGO.AddComponent<AudioSource>();
        playerAudio.clip = laserClip;
        playerAudio.playOnAwake = false;

        GameObject bp1 = new GameObject("BulletPosition01");
        bp1.transform.parent = playerGO.transform;
        bp1.transform.localPosition = new Vector3(-0.16f, 0.28f, 0);

        GameObject bp2 = new GameObject("BulletPosition02");
        bp2.transform.parent = playerGO.transform;
        bp2.transform.localPosition = new Vector3(0.16f, 0.28f, 0);

        PlayerControl pc = playerGO.AddComponent<PlayerControl>();
        pc.speed = 8f;
        pc.bulletPosition01 = bp1;
        pc.bulletPosition02 = bp2;
        pc.PlayerBulletGO = playerBulletPrefab;
        pc.ExplosionGO = explosionPrefab;
        pc.GameManagerGO = gmGO;

        // --- H. UI Canvas ---
        GameObject canvasGO = new GameObject("Canvas");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(800, 600);
        scaler.matchWidthOrHeight = 0.5f;
        canvasGO.AddComponent<GraphicRaycaster>();

        // EventSystem
        GameObject eventSys = new GameObject("EventSystem");
        eventSys.AddComponent<UnityEngine.EventSystems.EventSystem>();
        eventSys.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();

        // Title
        GameObject titleGO = new GameObject("GameTitleGO");
        titleGO.transform.SetParent(canvasGO.transform, false);
        Image titleImg = titleGO.AddComponent<Image>();
        titleImg.sprite = gameTitleSprite;
        titleImg.preserveAspect = true;
        RectTransform titleRT = titleGO.GetComponent<RectTransform>();
        titleRT.anchoredPosition = new Vector2(0, 120);
        titleRT.sizeDelta = new Vector2(400, 140);

        // Play Button
        GameObject playBtnGO = new GameObject("ButtonPlay");
        playBtnGO.transform.SetParent(canvasGO.transform, false);
        Image playImg = playBtnGO.AddComponent<Image>();
        playImg.sprite = playBtnSprite;
        playImg.preserveAspect = true;
        Button playBtn = playBtnGO.AddComponent<Button>();
        playBtn.transition = Selectable.Transition.SpriteSwap;
        SpriteState ss = new SpriteState
        {
            highlightedSprite = playBtnHighSprite,
            pressedSprite = playBtnHighSprite,
            selectedSprite = playBtnHighSprite
        };
        playBtn.spriteState = ss;
        UnityEventTools.AddPersistentListener(playBtn.onClick, new UnityAction(gm.StartgamePlay));
        RectTransform playRT = playBtnGO.GetComponent<RectTransform>();
        playRT.anchoredPosition = new Vector2(0, -60);
        playRT.sizeDelta = new Vector2(162, 56);

        // Game Over
        GameObject gameOverGO = new GameObject("GameOverGO");
        gameOverGO.transform.SetParent(canvasGO.transform, false);
        Image goImg = gameOverGO.AddComponent<Image>();
        goImg.sprite = gameOverSprite;
        goImg.preserveAspect = true;
        RectTransform goRT = gameOverGO.GetComponent<RectTransform>();
        goRT.anchoredPosition = new Vector2(0, 30);
        goRT.sizeDelta = new Vector2(377, 47);
        gameOverGO.SetActive(false);

        // Virtual Shoot Button (bottom-right)
        GameObject shootBtnGO = new GameObject("ButtonShoot");
        shootBtnGO.transform.SetParent(canvasGO.transform, false);
        Image shootImg = shootBtnGO.AddComponent<Image>();
        shootImg.sprite = shootBtnSprite;
        shootImg.preserveAspect = true;
        Button shootBtn = shootBtnGO.AddComponent<Button>();
        UnityEventTools.AddPersistentListener(shootBtn.onClick, new UnityAction(pc.Shoot));
        RectTransform shootRT = shootBtnGO.GetComponent<RectTransform>();
        shootRT.anchorMin = new Vector2(1, 0);
        shootRT.anchorMax = new Vector2(1, 0);
        shootRT.pivot = new Vector2(1, 0);
        shootRT.anchoredPosition = new Vector2(-20, 20);
        shootRT.sizeDelta = new Vector2(80, 80);
        shootBtnGO.SetActive(false);

        // Score Panel (Top Right)
        GameObject scorePanelGO = new GameObject("ImageScoreUIBg");
        scorePanelGO.transform.SetParent(canvasGO.transform, false);
        Image scoreBg = scorePanelGO.AddComponent<Image>();
        scoreBg.sprite = scoreBgSprite;
        RectTransform scorePanelRT = scorePanelGO.GetComponent<RectTransform>();
        scorePanelRT.anchorMin = new Vector2(1, 1);
        scorePanelRT.anchorMax = new Vector2(1, 1);
        scorePanelRT.pivot = new Vector2(1, 1);
        scorePanelRT.anchoredPosition = new Vector2(-20, -20);
        scorePanelRT.sizeDelta = new Vector2(164, 30);

        GameObject scoreTextGO = new GameObject("TextScore");
        scoreTextGO.tag = "ScoreTextTag";
        scoreTextGO.transform.SetParent(scorePanelGO.transform, false);
        Text scoreTxt = scoreTextGO.AddComponent<Text>();
        scoreTxt.text = "000000";
        scoreTxt.font = standardFont;
        scoreTxt.fontSize = 20;
        scoreTxt.alignment = TextAnchor.MiddleCenter;
        scoreTxt.color = Color.white;
        RectTransform scoreTxtRT = scoreTextGO.GetComponent<RectTransform>();
        scoreTxtRT.anchorMin = Vector2.zero;
        scoreTxtRT.anchorMax = Vector2.one;
        scoreTxtRT.sizeDelta = Vector2.zero;
        scoreTextGO.AddComponent<GameScore>();

        // Lives Panel (Top Left)
        GameObject livesPanelGO = new GameObject("ImagesLivesUIBg");
        livesPanelGO.transform.SetParent(canvasGO.transform, false);
        Image livesBg = livesPanelGO.AddComponent<Image>();
        livesBg.sprite = livesBgSprite;
        RectTransform livesPanelRT = livesPanelGO.GetComponent<RectTransform>();
        livesPanelRT.anchorMin = new Vector2(0, 1);
        livesPanelRT.anchorMax = new Vector2(0, 1);
        livesPanelRT.pivot = new Vector2(0, 1);
        livesPanelRT.anchoredPosition = new Vector2(20, -20);
        livesPanelRT.sizeDelta = new Vector2(136, 30);

        GameObject livesTextGO = new GameObject("TextLives");
        livesTextGO.transform.SetParent(livesPanelGO.transform, false);
        Text livesTxt = livesTextGO.AddComponent<Text>();
        livesTxt.text = "3";
        livesTxt.font = standardFont;
        livesTxt.fontSize = 20;
        livesTxt.alignment = TextAnchor.MiddleCenter;
        livesTxt.color = Color.white;
        RectTransform livesTxtRT = livesTextGO.GetComponent<RectTransform>();
        livesTxtRT.anchorMin = Vector2.zero;
        livesTxtRT.anchorMax = Vector2.one;
        livesTxtRT.sizeDelta = Vector2.zero;

        // Wire player's Lives text
        pc.LivesUIText = livesTxt;

        // Time Counter Panel (Top Center)
        GameObject timePanelGO = new GameObject("ImageTimeUIBg");
        timePanelGO.transform.SetParent(canvasGO.transform, false);
        Image timeBg = timePanelGO.AddComponent<Image>();
        timeBg.sprite = timeBgSprite;
        RectTransform timePanelRT = timePanelGO.GetComponent<RectTransform>();
        timePanelRT.anchorMin = new Vector2(0.5f, 1);
        timePanelRT.anchorMax = new Vector2(0.5f, 1);
        timePanelRT.pivot = new Vector2(0.5f, 1);
        timePanelRT.anchoredPosition = new Vector2(0, -20);
        timePanelRT.sizeDelta = new Vector2(164, 30);

        GameObject timeTextGO = new GameObject("TextTimeCounter");
        timeTextGO.transform.SetParent(timePanelGO.transform, false);
        Text timeTxt = timeTextGO.AddComponent<Text>();
        timeTxt.text = "00:00";
        timeTxt.font = standardFont;
        timeTxt.fontSize = 20;
        timeTxt.alignment = TextAnchor.MiddleCenter;
        timeTxt.color = Color.white;
        RectTransform timeTxtRT = timeTextGO.GetComponent<RectTransform>();
        timeTxtRT.anchorMin = Vector2.zero;
        timeTxtRT.anchorMax = Vector2.one;
        timeTxtRT.sizeDelta = Vector2.zero;
        timeTextGO.AddComponent<TimeCounter>();

        // Wire GameManager references
        gm.playButton = playBtnGO;
        gm.playerShip = playerGO;
        gm.enemySpawner = spawnerGO;
        gm.GameOverGO = gameOverGO;
        gm.scoreUITextGO = scoreTextGO;
        gm.TimeCounterGO = timeTextGO;
        gm.GameTitleGO = titleGO;
        gm.ShootButton = shootBtnGO;

        // Save Scene
        EditorSceneManager.SaveScene(newScene, "Assets/Scenes/MainScene.unity");
        Debug.Log("Scene saved to Assets/Scenes/MainScene.unity");

        // Set EditorBuildSettings
        EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene("Assets/Scenes/MainScene.unity", true) };
        Debug.Log("Build settings updated with MainScene.");

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Space Shooter Project Build COMPLETE!");
    }

    [MenuItem("SpaceShooter/Build Standalone")]
    public static void BuildStandalone()
    {
        Debug.Log("Starting Standalone Windows 64 build...");
        string buildDir = "D:/Downloads/SPACE SHOOTER/Build";
        if (!System.IO.Directory.Exists(buildDir))
        {
            System.IO.Directory.CreateDirectory(buildDir);
        }

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = new[] { "Assets/Scenes/MainScene.unity" },
            locationPathName = buildDir + "/SpaceShooter.exe",
            target = BuildTarget.StandaloneWindows64,
            options = BuildOptions.None
        };

        UnityEditor.Build.Reporting.BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        UnityEditor.Build.Reporting.BuildSummary summary = report.summary;

        if (summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded)
        {
            Debug.Log("STANDALONE BUILD SUCCEEDED: " + summary.totalSize + " bytes");
        }
        else
        {
            Debug.LogError("STANDALONE BUILD FAILED: " + summary.result);
        }
    }
}
