using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Lesson5
{
    [Test]
    public void __ExistingDirectoriesAndFiles()
    {
        var pathDirectory = Path.Combine("Assets", "Prefabs");
        var exists = Directory.Exists(pathDirectory);
        Assert.IsTrue(exists,
            "The \"{0}\" directory does not have the \"{1}\" directory", new object[] { "Assets", "Prefabs" });

        var pathFile = Path.Combine("Assets", "Prefabs", "Coin.prefab");
        exists = File.Exists(pathFile);
        Assert.IsTrue(exists,
            "The \"{0}\" directory does not have the \"{1}\" prefab", new object[] { "Prefabs", "Coin" });
    }
    [Test]
    public void __ExistingObjectsOnScene()
    {
        GameObject gameObjectPlayer = GameObject.Find("Player");
        Assert.IsNotNull(gameObjectPlayer,
            "There is no \"{0}\" object on the scene", new object[] { "Player" });

        if (!gameObjectPlayer.TryGetComponent(out Player player))
        {
            Assert.AreEqual(gameObjectPlayer.AddComponent<Player>(), player,
                "The \"{0}\" object does not have a \"{1}\" script", new object[] { gameObjectPlayer.name, "Player" });
        }
    }

    [Test]
    public void _1CheckingPrefabCoin()
    {
        string pathFile = Path.Combine("Assets", "Prefabs", "Coin.prefab");
        GameObject coinObject = AssetDatabase.LoadAssetAtPath<GameObject>(pathFile);

        /***************************Transform*************************/

        string nameComponent = "Transform";

        if (!coinObject.TryGetComponent(out Transform transform))
        {
            Assert.AreEqual(coinObject.AddComponent<Transform>(), transform,
                "The \"{0}\" prefab does not have a \"{1}\" component", new object[] { coinObject.name, nameComponent });
        }

        Assert.AreEqual(Vector3.zero, transform.position,
            "The \"{0}\" prefab in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { coinObject.name, nameComponent, "Position" });

        Assert.AreEqual(Vector3.zero, transform.rotation.eulerAngles,
            "The \"{0}\" prefab in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { coinObject.name, nameComponent, "Rotation" });

        Assert.AreEqual(Vector3.one, transform.localScale,
            "The \"{0}\" prefab in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { coinObject.name, nameComponent, "Scale" });

        /***********************Sprite Renderer*********************/

        nameComponent = "Sprite Renderer";

        if (!coinObject.TryGetComponent(out SpriteRenderer spriteRenderer))
        {
            Assert.AreEqual(coinObject.AddComponent<SpriteRenderer>(), spriteRenderer,
                "The \"{0}\" prefab does not have a \"{1}\" component", new object[] { coinObject.name, nameComponent });
        }

        pathFile = Path.Combine("Assets", "Sprites", "Tileset.png");
        UnityEngine.Object[] spritesTileset = AssetDatabase.LoadAllAssetRepresentationsAtPath(pathFile);

        Assert.AreEqual(spritesTileset[48], spriteRenderer.sprite,
            "The \"{0}\" prefab in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { coinObject.name, nameComponent, "Sprite" });

        /***********************Box Collider*********************/

        nameComponent = "Box Collider 2D";

        if (!coinObject.TryGetComponent(out BoxCollider2D boxCollider2D))
        {
            Assert.AreEqual(coinObject.AddComponent<BoxCollider2D>(), boxCollider2D,
                "The \"{0}\" prefab does not have a \"{1}\" component", new object[] { coinObject.name, nameComponent });
        }

        Assert.IsTrue(boxCollider2D.isTrigger,
            "The \"{0}\" prefab in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { coinObject.name, nameComponent, "Is Trigger" });

        Assert.AreEqual(new Vector2(0.625f, 0.878f), boxCollider2D.size,
            "The \"{0}\" prefab in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { coinObject.name, nameComponent, "Size" });

        /***************************Coin*************************/

        nameComponent = "Coin";

        if (!coinObject.TryGetComponent(out Coin coin))
        {
            Assert.AreEqual(coinObject.AddComponent<Player>(), coin,
                "The \"{0}\" prefab does not have a \"{1}\" script", new object[] { coinObject.name, nameComponent });
        }
    }

    [Test]
    public void _2CheckingObjectCoinOnScene()
    {
        GameObject gameObjectCoin = GameObject.Find("Coin");
        Assert.IsNotNull(gameObjectCoin,
            "There is no \"{0}\" object on the scene", new object[] { "Coin" });

        /***********************Sprite Renderer*********************/

        string nameComponent = "Sprite Renderer";

        if (!gameObjectCoin.TryGetComponent(out SpriteRenderer spriteRenderer))
        {
            Assert.AreEqual(gameObjectCoin.AddComponent<SpriteRenderer>(), spriteRenderer,
                "The \"{0}\" object does not have a \"{1}\" component", new object[] { gameObjectCoin.name, nameComponent });
        }

        var pathFile = Path.Combine("Assets", "Sprites", "Tileset.png");
        UnityEngine.Object[] spritesTileset = AssetDatabase.LoadAllAssetRepresentationsAtPath(pathFile);

        Assert.AreEqual(spritesTileset[48], spriteRenderer.sprite,
            "The \"{0}\" object in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { gameObjectCoin.name, nameComponent, "Sprite" });

        /***********************Box Collider*********************/

        nameComponent = "Box Collider 2D";

        if (!gameObjectCoin.TryGetComponent(out BoxCollider2D boxCollider2D))
        {
            Assert.AreEqual(gameObjectCoin.AddComponent<BoxCollider2D>(), boxCollider2D,
                "The \"{0}\" object does not have a \"{1}\" component", new object[] { gameObjectCoin.name, nameComponent });
        }

        Assert.IsTrue(boxCollider2D.isTrigger,
            "The \"{0}\" object in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { gameObjectCoin.name, nameComponent, "Is Trigger" });

        Assert.AreEqual(new Vector2(0.625f, 0.878f), boxCollider2D.size,
            "The \"{0}\" object in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { gameObjectCoin.name, nameComponent, "Size" });

        /***************************Coin*************************/

        nameComponent = "Coin";

        if (!gameObjectCoin.TryGetComponent(out Coin coin))
        {
            Assert.AreEqual(gameObjectCoin.AddComponent<Player>(), coin,
                "The \"{0}\" object does not have a \"{1}\" script", new object[] { gameObjectCoin.name, nameComponent });
        }
    }

    [Test]
    public void _3CheckingScriptCoin()
    {
        Type type = typeof(Coin);
        TestAssistant.TestingField(type, "count", typeof(int), FieldAttributes.Private, true);

        TestAssistant.TestingMethod(type, "OnTriggerEnter2D", typeof(void), MethodAttributes.Private, new MyParameterInfo[] { new MyParameterInfo(typeof(Collider2D), "collision") });

        GameObject gameObjectCoin = GameObject.Find("Coin");
        Coin scriptCoin = gameObjectCoin.GetComponent<Coin>();

        TestAssistant.TestingFieldValue(type, "count", scriptCoin, 10);
    }

    [Test]
    public void _4CheckingObjectCanvasOnScene()
    {
        GameObject gameObjectCanvas = GameObject.Find("Canvas");
        Assert.IsNotNull(gameObjectCanvas,
            "There is no \"{0}\" object on the scene", new object[] { "Canvas" });

        /***************************Canvas*************************/

        string nameComponent = "Canvas";

        if (!gameObjectCanvas.TryGetComponent(out Canvas canvas))
        {
            Assert.AreEqual(gameObjectCanvas.AddComponent<Canvas>(), canvas,
                "The \"{0}\" object does not have a \"{1}\" component", new object[] { gameObjectCanvas.name, nameComponent });
        }

        Assert.AreEqual(RenderMode.ScreenSpaceCamera, canvas.renderMode,
            "The \"{0}\" object in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { gameObjectCanvas.name, nameComponent, "Render Mode" });

        Assert.AreEqual(Camera.main, canvas.worldCamera,
            "The \"{0}\" object in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { gameObjectCanvas.name, nameComponent, "Render Camera" });

        /***************************CanvasScaler*************************/

        nameComponent = "Canvas Scaler";

        if (!gameObjectCanvas.TryGetComponent(out CanvasScaler canvasScaler))
        {
            Assert.AreEqual(gameObjectCanvas.AddComponent<CanvasScaler>(), canvasScaler,
                "The \"{0}\" object does not have a \"{1}\" component", new object[] { gameObjectCanvas.name, nameComponent });
        }

        Assert.AreEqual(CanvasScaler.ScaleMode.ScaleWithScreenSize, canvasScaler.uiScaleMode,
            "The \"{0}\" object in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { gameObjectCanvas.name, nameComponent, "UI Scale Mode" });

        Assert.AreEqual(new Vector2(1366, 768), canvasScaler.referenceResolution,
            "The \"{0}\" object in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { gameObjectCanvas.name, nameComponent, "Reference Resolution" });

        Assert.AreEqual(0.5f, canvasScaler.matchWidthOrHeight,
            "The \"{0}\" object in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { gameObjectCanvas.name, nameComponent, "Match" });

        /***************************GraphicRaycaster*************************/

        nameComponent = "Graphic Raycaster";

        if (!gameObjectCanvas.TryGetComponent(out GraphicRaycaster graphicRaycaster))
        {
            Assert.AreEqual(gameObjectCanvas.AddComponent<GraphicRaycaster>(), graphicRaycaster,
                "The \"{0}\" object does not have a \"{1}\" component", new object[] { gameObjectCanvas.name, nameComponent });
        }
    }

    [Test]
    public void _5ExistsTextOnScene()
    {
        GameObject gameObjectScore = GameObject.Find("Score");

        Assert.IsNotNull(gameObjectScore,
            "There is no \"{0}\" object on the scene", new object[] { "Score" });

        /***************************RectTransform*************************/

        string nameComponent = "Rect Transform";

        if (!gameObjectScore.TryGetComponent(out RectTransform rectTransform))
        {
            Assert.AreEqual(gameObjectScore.AddComponent<RectTransform>(), rectTransform,
                "The \"{0}\" object does not have a \"{1}\" component", new object[] { gameObjectScore.name, nameComponent });
        }

        Assert.AreEqual(0, rectTransform.anchorMin.x,
            "The \"{0}\" object in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { gameObjectScore.name, nameComponent, "Anchors Min X" });

        Assert.AreEqual(1, rectTransform.anchorMin.y,
            "The \"{0}\" object in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { gameObjectScore.name, nameComponent, "Anchors Min Y" });

        Assert.AreEqual(0, rectTransform.anchorMax.x,
            "The \"{0}\" object in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { gameObjectScore.name, nameComponent, "Anchors Max X" });

        Assert.AreEqual(1, rectTransform.anchorMax.y,
            "The \"{0}\" object in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { gameObjectScore.name, nameComponent, "Anchors Max Y" });

        Assert.AreEqual(0, rectTransform.pivot.x,
            "The \"{0}\" object in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { gameObjectScore.name, nameComponent, "Pivot X" });

        Assert.AreEqual(1, rectTransform.pivot.y,
            "The \"{0}\" object in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { gameObjectScore.name, nameComponent, "Pivot Y" });

        Assert.AreEqual(16, rectTransform.anchoredPosition.x,
            "The \"{0}\" object in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { gameObjectScore.name, nameComponent, "Pos X" });

        Assert.AreEqual(-16, rectTransform.anchoredPosition.y,
            "The \"{0}\" object in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { gameObjectScore.name, nameComponent, "Pos Y" });

        Assert.AreEqual(120, rectTransform.sizeDelta.x,
            "The \"{0}\" object in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { gameObjectScore.name, nameComponent, "Width" });

        Assert.AreEqual(40, rectTransform.sizeDelta.y,
            "The \"{0}\" object in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { gameObjectScore.name, nameComponent, "Height" });

        /***************************CanvasRenderer*************************/

        nameComponent = "Canvas Renderer";

        if (!gameObjectScore.TryGetComponent<CanvasRenderer>(out var canvasRenderer))
        {
            Assert.AreEqual(gameObjectScore.AddComponent<CanvasRenderer>(), canvasRenderer,
                "The \"{0}\" object does not have a \"{1}\" component", new object[] { gameObjectScore.name, nameComponent });
        }

        /***************************Text*************************/

        nameComponent = "Text";

        if (!gameObjectScore.TryGetComponent<Text>(out var text))
        {
            Assert.AreEqual(gameObjectScore.AddComponent<Text>(), text,
                "The \"{0}\" object does not have a \"{1}\" component", new object[] { gameObjectScore.name, nameComponent });
        }

        Assert.AreEqual("0", text.text,
            "The \"{0}\" object in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { gameObjectScore.name, nameComponent, "Text" });

        Assert.AreEqual(35, text.fontSize,
            "The \"{0}\" object in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { gameObjectScore.name, nameComponent, "Font Size" });

        Assert.AreEqual(Color.white, text.color,
            "The \"{0}\" object in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { gameObjectScore.name, nameComponent, "Color" });
    }

    [Test]
    public void _6CheckingScriptPlayer()
    {
        Type type = typeof(Player);
        TestAssistant.TestingField(type, "scoreText", typeof(Text), FieldAttributes.Private, true);
        TestAssistant.TestingField(type, "score", typeof(int), FieldAttributes.Private);

        TestAssistant.TestingMethod(type, "AddCoin", typeof(void), MethodAttributes.Public, new MyParameterInfo[] { new MyParameterInfo(typeof(int), "count") });

        GameObject gameObjectPlayer = GameObject.Find("Player");
        Player scriptPlayer = gameObjectPlayer.GetComponent<Player>();
        GameObject gameObjectScore = GameObject.Find("Score");
        Text textScore = gameObjectScore.GetComponent<Text>();

        TestAssistant.TestingFieldValue(type, "scoreText", scriptPlayer, textScore);
        TestAssistant.TestingFieldValue(type, "score", scriptPlayer, 0);

        scriptPlayer.AddCoin(10);

        int valieFieldScore = (int)TestAssistant.GetValueField(typeof(Player), "score", scriptPlayer);
        Assert.AreEqual(10, valieFieldScore,
            "In the \"{0}\" script, the \"{1}\" method incorrectly changes the value of the \"{2}\" field", new object[] {gameObjectPlayer.name, "AddCoin", "score"});

        Assert.AreEqual("10", textScore.text,
            "In the \"{0}\" script, the \"{1}\" method incorrectly changes the value of the \"{2}\" field of the \"{3}\" component in the \"{4}\" object", new object[] {gameObjectPlayer.name, "AddCoin", "text", "Text", "Score"});

        scriptPlayer.AddCoin(5);

        valieFieldScore = (int)TestAssistant.GetValueField(typeof(Player), "score", scriptPlayer);
        Assert.AreEqual(15, valieFieldScore,
            "In the \"{0}\" script, the \"{1}\" method incorrectly changes the value of the \"{2}\" field", new object[] {gameObjectPlayer.name, "AddCoin", "score"});

        Assert.AreEqual("15", textScore.text,
            "In the \"{0}\" script, the \"{1}\" method incorrectly changes the value of the \"{2}\" field of the \"{3}\" component in the \"{4}\" object", new object[] {gameObjectPlayer.name, "AddCoin", "text", "Text", "Score"});
    }
}
