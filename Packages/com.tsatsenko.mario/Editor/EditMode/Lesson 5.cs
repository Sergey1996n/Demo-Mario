using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class Lesson5
{
    [Test]
    public void ExistsDirectoryPrefabs()
    {
        var pathDirectory = Path.Combine("Assets", "Prefabs");
        var exists = Directory.Exists(pathDirectory);
        Assert.IsTrue(exists,
            "The \"Prefabs\" directory is missing!");
    }

    [Test]
    public void ExistsFileCoinInPrefabs()
    {
        var pathFile = Path.Combine("Assets", "Prefabs", "Coin.prefab");
        var exists = File.Exists(pathFile);
        Assert.IsTrue(exists,
            "There is no prefab \"Coin\" in the directory \"Prefabs\"!");

        GameObject coinObject = AssetDatabase.LoadAssetAtPath<GameObject>(pathFile);

        /***********************Sprite Renderer*********************/

        SpriteRenderer spriteRenderer = coinObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Assert.AreEqual(coinObject.AddComponent<SpriteRenderer>(), spriteRenderer,
                "The \"Coin\" object does not have a Sprite Renderer component");
        }

        Assert.AreEqual("Tileset_48", spriteRenderer.sprite.name,
            "The \"Coin\" object in the \"Sprite Renderer\" component has an incorrect \"Sprite\" field");

        /***********************Box Collider*********************/

        BoxCollider2D boxCollider2D = coinObject.GetComponent<BoxCollider2D>();
        if (boxCollider2D == null)
        {
            Assert.AreEqual(coinObject.AddComponent<BoxCollider2D>(), boxCollider2D,
                "The \"Coin\" object does not have a BoxCollider2D component");
        }

        Assert.IsTrue(boxCollider2D.isTrigger,
            "The \"Coin\" object in the \"Box Collider 2D\" component has an incorrect \"Size\" field");

        Assert.AreEqual(new Vector2(0.625f, 0.878f), boxCollider2D.size,
            "The \"Coin\" object in the \"Box Collider 2D\" component has an incorrect \"Size\" field");
    }

    [Test]
    public void CheckingScriptPlayer()
    {
        var nameScript = "Player";
        var pathFile = Path.Combine("Assets", "Scripts", nameScript + ".cs");
        var exists = File.Exists(pathFile);
        Assert.IsTrue(exists,
            $"The \"{nameScript}\" script is missing!");

        TestAssistant.TestingFields(typeof(Player), "scoreText", "Text", FieldAttributes.Private, true);

        TestAssistant.TestingMethods(typeof(Player), "AddCoin", "Void", MethodAttributes.Public | MethodAttributes.HideBySig, new MyParameterInfo[] { new MyParameterInfo(typeof(int), "count")});
    }

    public void CheckingObjectPlayerValueSciptPlayer()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        var p = player.GetComponent<Player>();
        if (p == null)
        {
            Assert.AreEqual(player.AddComponent<Player>(), p,
                "The \"Player\" object does not have a Player script");
        }

        TestAssistant.TestingFieldValue(typeof(Player), "scoreText", p, GameObject.Find("Score"));
    }

    [Test]
    public void ExistsCanvasOnScene()
    {
        var canvasObject = GameObject.Find("Canvas");

        /***************************Canvas*************************/

        var canvas = canvasObject.GetComponent<Canvas>();
        if (canvas == null)
        {
            Assert.AreEqual(canvasObject.AddComponent<Canvas>(), canvas,
                "The \"Canvas\" object does not have a \"Canvas\" component");
        }

        Assert.AreEqual(RenderMode.ScreenSpaceCamera, canvas.renderMode,
            "The \"Canvas\" object in the \"Canvas\" component has an incorrect \"Render Mode\" field");
        
        Assert.AreEqual(Camera.main, canvas.worldCamera,
            "The \"Canvas\" object in the \"Canvas\" component has an incorrect \"Render Camera\" field");

        /***************************CanvasScaler*************************/

        var canvasScaler = canvasObject.GetComponent<CanvasScaler>();
        if (canvasScaler == null)
        {
            Assert.AreEqual(canvasObject.AddComponent<CanvasScaler>(), canvasScaler,
                "The \"Canvas\" object does not have a \"CanvasScaler\" component");
        }

        Assert.AreEqual(CanvasScaler.ScaleMode.ScaleWithScreenSize, canvasScaler.uiScaleMode,
            "The \"Canvas\" object in the \"Canvas Scaler\" component has an incorrect \"Reference Resolution\" field");

        Assert.AreEqual(new Vector2(1366, 768), canvasScaler.referenceResolution,
            "The \"Canvas\" object in the \"Canvas Scaler\" component has an incorrect \"Reference Resolution\" field");

        Assert.AreEqual(0.5f, canvasScaler.matchWidthOrHeight,
            "The \"Canvas\" object in the \"Canvas Scaler\" component has an incorrect \"Match\" field");

        /***************************GraphicRaycaster*************************/

        var graphicRaycaster = canvasObject.GetComponent<GraphicRaycaster>();
        if (graphicRaycaster == null)
        {
            Assert.AreEqual(canvasObject.AddComponent<GraphicRaycaster>(), graphicRaycaster,
                "The \"Canvas\" object does not have a \"Graphic Raycaster\" component");
        }
    }

    [Test]
    public void ExistsTextOnScene()
    {
        var scoreObject = GameObject.Find("Score");

        Assert.IsNotNull(scoreObject,
            "There is no \"Score\" object on the scene");

        /***************************RectTransform*************************/

        var rectTransform = scoreObject.GetComponent<RectTransform>();

        Assert.AreEqual(0, rectTransform.anchorMin.x,
            "The \"Score\" object in the \"Rect Transform\" component has an incorrect \"Anchors Min X\" field");

        Assert.AreEqual(1, rectTransform.anchorMin.y,
            "The \"Score\" object in the \"Rect Transform\" component has an incorrect \"Anchors Min Y\" field");

        Assert.AreEqual(0, rectTransform.anchorMax.x,
            "The \"Score\" object in the \"Rect Transform\" component has an incorrect \"Anchors Max X\" field");

        Assert.AreEqual(1, rectTransform.anchorMax.y,
            "The \"Score\" object in the \"Rect Transform\" component has an incorrect \"Anchors Max Y\" field");

        Assert.AreEqual(0, rectTransform.pivot.x,
            "The \"Score\" object in the \"Rect Transform\" component has an incorrect \"Pivot X\" field");

        Assert.AreEqual(1, rectTransform.pivot.y,
            "The \"Score\" object in the \"Rect Transform\" component has an incorrect \"Pivot Y\" field");

        Assert.AreEqual(16, rectTransform.anchoredPosition.x,
            "The \"Score\" object in the \"Rect Transform\" component has an incorrect \"Pos X\" field");

        Assert.AreEqual(-16, rectTransform.anchoredPosition.y,
            "The \"Score\" object in the \"Rect Transform\" component has an incorrect \"Pos Y\" field");

        Assert.AreEqual(120, rectTransform.sizeDelta.x,
            "The \"Score\" object in the \"Rect Transform\" component has an incorrect \"Width\" field");

        Assert.AreEqual(40, rectTransform.sizeDelta.y,
            "The \"Score\" object in the \"Rect Transform\" component has an incorrect \"Height\" field");

        /***************************CanvasRenderer*************************/

        if (!scoreObject.TryGetComponent<CanvasRenderer>(out var canvasRenderer))
        {
            Assert.AreEqual(scoreObject.AddComponent<CanvasRenderer>(), canvasRenderer,
                "The \"Score\" object does not have a \"Canvas Renderer\" component");
        }

        /***************************Text*************************/

        if (!scoreObject.TryGetComponent<Text>(out var text))
        {
            Assert.AreEqual(scoreObject.AddComponent<Text>(), text,
                "The \"Score\" object does not have a \"Canvas Renderer\" component");
        }

        Assert.AreEqual("0", text.text,
            "The \"Score\" object in the \"Text\" component has an incorrect \"Text\" field");

        Assert.AreEqual(35, text.fontSize,
            "The \"Score\" object in the \"Text\" component has an incorrect \"Font Size\" field");

        Assert.AreEqual(Color.white, text.color,
            "The \"Score\" object in the \"Text\" component has an incorrect \"Color\" field");

    }

    [Test]
    public void CheckingScriptCoin()
    {
        var nameScript = "Coin";
        var pathFile = Path.Combine("Assets", "Scripts", nameScript + ".cs");
        var exists = File.Exists(pathFile);
        Assert.IsTrue(exists,
            $"The \"{nameScript}\" script is missing!");

        TestAssistant.TestingFields(typeof(Coin), "count", "Int32", FieldAttributes.Private, true);

        TestAssistant.TestingMethods(typeof(Coin), "OnTriggerEnter2D", "Void", MethodAttributes.Private | MethodAttributes.HideBySig, new MyParameterInfo[] { new MyParameterInfo(typeof(Collider2D), "collision") });
    }

    [Test]
    public void ExistsComponentCoinInObjectCoin()
    {
        var pathFile = Path.Combine("Assets", "Prefabs", "Coin.prefab");
        var exists = File.Exists(pathFile);
        Assert.IsTrue(exists,
            "There is no prefab \"Coin\" in the directory \"Prefabs\"!");

        GameObject coinObject = AssetDatabase.LoadAssetAtPath<GameObject>(pathFile);

        /*************************Coin***********************/

        if (!coinObject.TryGetComponent<Coin>(out Coin coin))
        {
            Assert.AreEqual(coinObject.AddComponent<Coin>(), coin,
                "The \"Coin\" object does not have a Coin component");
        }

        TestAssistant.TestingFieldValue(typeof(Coin), "count", coin, 10);
    }
}
