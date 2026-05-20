using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JamSceneSetup : EditorWindow
{
    [MenuItem("Gemini Tools/Setup Crane Game Scene")]
    public static void SetupScene()
    {
        // 1. ScoreTextManagerの作成
        GameObject scoreManagerObj = new GameObject("ScoreTextManager");
        ScoreTextManager scoreManager = scoreManagerObj.AddComponent<ScoreTextManager>();

        // 2. クレーンの作成
        GameObject craneObj = new GameObject("Crane");
        CraneController crane = craneObj.AddComponent<CraneController>();
        Rigidbody2D craneRb = craneObj.GetComponent<Rigidbody2D>();
        craneRb.isKinematic = true;

        // キャッチポイント（子オブジェクト）
        GameObject catchPoint = new GameObject("CatchPoint");
        catchPoint.transform.SetParent(craneObj.transform);
        catchPoint.transform.localPosition = new Vector3(0, -1, 0);

        // クレーンのコライダ（アーム先端想定）
        BoxCollider2D craneCol = craneObj.AddComponent<BoxCollider2D>();
        craneCol.isTrigger = true;
        craneCol.size = new Vector2(0.5f, 0.5f);

        // 3. UIの作成 (Canvas)
        GameObject canvasObj = new GameObject("Canvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();

        // EventSystem
        if (Object.FindFirstObjectByType<EventSystem>() == null)
        {
            GameObject esObj = new GameObject("EventSystem");
            esObj.AddComponent<EventSystem>();
            esObj.AddComponent<StandaloneInputModule>();
        }

        // テキストUIの配置 (ScoreA, ScoreB)
        Text scoreTextA = CreateTextUI(canvasObj.transform, "ScoreText_1P", "SCORE: 0", new Vector2(-200, 150));
        Text scoreTextB = CreateTextUI(canvasObj.transform, "MoneyText_2P", "SPENT: ¥0", new Vector2(200, 150));

        // 4. ゴールの作成
        GameObject goalObj = new GameObject("GoalPlatform");
        goalObj.transform.position = new Vector3(0, -4, 0);
        BoxCollider2D goalCol = goalObj.AddComponent<BoxCollider2D>();
        goalCol.isTrigger = true;
        goalCol.size = new Vector2(3, 1);
        goalObj.AddComponent<GoalPlatform>();

        // 5. 参照の紐付け (ScoreTextManager)
        var smSerialized = new SerializedObject(scoreManager);
        smSerialized.FindProperty("_scoreTextA").objectReferenceValue = scoreTextA;
        smSerialized.FindProperty("_scoreTextB").objectReferenceValue = scoreTextB;
        smSerialized.ApplyModifiedProperties();

        // 6. 参照の紐付け (CraneController)
        var craneSerialized = new SerializedObject(crane);
        craneSerialized.FindProperty("_scoreTextManager").objectReferenceValue = scoreManager;
        craneSerialized.FindProperty("_catchPoint").objectReferenceValue = catchPoint.transform;
        craneSerialized.ApplyModifiedProperties();

        // 7. メインカメラの調整
        Camera mainCam = Camera.main;
        if (mainCam != null)
        {
            mainCam.orthographic = true;
            mainCam.orthographicSize = 5;
            mainCam.transform.position = new Vector3(0, 0, -10);
        }

        Debug.Log("最新のコード構成に基づいた、クレーンゲームのシーンセットアップが完了しました！");
    }

    private static Text CreateTextUI(Transform parent, string name, string initialText, Vector2 pos)
    {
        GameObject textObj = new GameObject(name);
        textObj.transform.SetParent(parent);
        
        Text t = textObj.AddComponent<Text>();
        t.text = initialText;
        t.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        t.fontSize = 24;
        t.alignment = TextAnchor.MiddleCenter;
        t.color = Color.white;

        RectTransform rect = textObj.GetComponent<RectTransform>();
        rect.anchoredPosition = pos;
        rect.sizeDelta = new Vector2(300, 50);

        return t;
    }
}
