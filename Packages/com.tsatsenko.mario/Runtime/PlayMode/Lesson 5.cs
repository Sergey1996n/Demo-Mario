using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class Lesson5
{

    [UnitySetUp]
    public IEnumerator Setup()
    {
        yield return EditorSceneManager.LoadSceneAsyncInPlayMode("Assets/Scenes/SampleScene.unity", new LoadSceneParameters(LoadSceneMode.Single));
    }

    [UnityTest]
    public IEnumerator CheckingMethodAddCoinObjectPlayer()
    {
        var playerObject = GameObject.Find("Player");
        var player = playerObject.GetComponent<Player>();


        yield return new WaitForEndOfFrame();
        var typePlayer = typeof(Player);
        var bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;

        var fieldScore = typePlayer.GetField("score", bindingFlags);

        var scoreObject = GameObject.Find("Score");
        var text = scoreObject.GetComponent<Text>();

        player.AddCoin(10);

        Assert.AreEqual(10, fieldScore.GetValue(player),
            "In the \"Player\" script, the \"AddCoin\" method incorrectly modifies the \"score\" field!");

        Assert.AreEqual("10", text.text,
            "In the \"Player\" script, the \"AddCoin\" method incorrectly changes the value of the \"text\" field of the \"Text\" component in the \"Score\" object!");

        player.AddCoin(5);

        Assert.AreEqual(15, fieldScore.GetValue(player),
            "In the \"Player\" script, the \"AddCoin\" method incorrectly modifies the \"score\" field!");

        Assert.AreEqual("15", text.text,
            "In the \"Player\" script, the \"AddCoin\" method incorrectly changes the value of the \"Ttext\" field of the \"Text\" component in the \"Score\" object!");
    }

    [UnityTest]
    public IEnumerator CheckingMethodOnTriggerEnter2DObjectCoin()
    {
        var playerObject = GameObject.Find("Player");
        var player = playerObject.GetComponent<Player>();

        var pathFile = Path.Combine("Assets", "Prefabs", "Coin.prefab");
        GameObject coinObject = AssetDatabase.LoadAssetAtPath<GameObject>(pathFile);

        GameObject coinObjectTest = GameObject.Instantiate(coinObject);
        GameObject coinObjectTest2 = GameObject.Instantiate(coinObject);

        playerObject.transform.position = Vector3.one;
        coinObjectTest.transform.position = Vector3.one;
        coinObjectTest2.transform.position = -Vector3.one;

        yield return new WaitForSeconds(.1f);

        var typePlayer = typeof(Player);
        var bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;

        var fieldScore = typePlayer.GetField("score", bindingFlags);

        Assert.AreEqual(10, fieldScore.GetValue(player),
            "The \"OnTriggerEnter2D\" method of the \"Coin\" object incorrectly changes the value of the \"score\" field of the \"Player\" script of the \"Player\" object");

        var scoreObject = GameObject.Find("Score");
        var text = scoreObject.GetComponent<Text>();

        Assert.AreEqual("10", text.text,
            "The \"OnTriggerEnter2D\" method of the \"Coin\" object incorrectly changes the value of the \"text\" field of the \"Text\" component of the \"Score\" object");

        if (coinObjectTest != null)
        {
            Assert.AreEqual(null, coinObjectTest,
                "The \"OnTriggerEnter2D\" method of the \"Coin\" object does not delete the \"Coin\" object");
        }
        
        coinObjectTest2.transform.position = Vector3.one;

        yield return new WaitForSeconds(.1f);

        Assert.AreEqual(20, fieldScore.GetValue(player),
            "The \"OnTriggerEnter2D\" method of the \"Coin\" object incorrectly changes the value of the \"score\" field of the \"Player\" script of the \"Player\" object");

        Assert.AreEqual("20", text.text,
            "The \"OnTriggerEnter2D\" method of the \"Coin\" object incorrectly changes the value of the \"text\" field of the \"Text\" component of the \"Score\" object");

        yield return new WaitForSeconds(.1f);

        if (coinObjectTest2 != null)
        {
            Assert.AreEqual(null, coinObjectTest2,
                "The \"OnTriggerEnter2D\" method of the \"Coin\" object does not delete the \"Coin\" object");
        }

        GameObject coinObjectTest3 = GameObject.Instantiate(coinObject, -Vector3.one, Quaternion.identity);
        GameObject coinObjectTest4 = GameObject.Instantiate(coinObject, -Vector3.one, Quaternion.identity);

        coinObjectTest3.tag = "Untagged";
        coinObjectTest3.AddComponent<Rigidbody2D>();

        yield return new WaitForSeconds(.1f);

        Assert.AreEqual(20, fieldScore.GetValue(player),
            "The \"OnTriggerEnter2D\" method of the \"Coin\" object incorrectly changes the value of the \"score\" field of the \"Player\" script of the \"Player\" object");

        Assert.AreEqual("20", text.text,
            "The \"OnTriggerEnter2D\" method of the \"Coin\" object incorrectly changes the value of the \"text\" field of the \"Text\" component of the \"Score\" object");

        yield return new WaitForSeconds(.1f);

        if (coinObjectTest3 == null)
        {
            Assert.AreEqual(coinObject, coinObjectTest3,
            "The \"OnTriggerEnter2D\" method of the \"Coin\" object deletes the \"Coin\" object");
        }
        
        if (coinObjectTest4 == null)
        {
            Assert.AreEqual(coinObject, coinObjectTest4,
            "The \"OnTriggerEnter2D\" method of the \"Coin\" object deletes the \"Coin\" object");
        }
    }
}
