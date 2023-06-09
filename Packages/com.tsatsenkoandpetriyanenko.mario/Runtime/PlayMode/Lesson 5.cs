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

    private string pathScene = Path.Combine("Assets", "Scenes", "Level.unity");

    [UnitySetUp]
    public IEnumerator Setup()
    {
        yield return EditorSceneManager.LoadSceneAsyncInPlayMode(pathScene, new LoadSceneParameters(LoadSceneMode.Single));
    }

    [UnityTearDown]
    public IEnumerator Tear()
    {
        yield return EditorSceneManager.LoadSceneAsyncInPlayMode(pathScene, new LoadSceneParameters(LoadSceneMode.Single));
    }

    [UnityTest]
    public IEnumerator CheckingMethodOnTriggerEnter2DObjectCoin()
    {
        GameObject gameObjectPlayer = GameObject.Find("Player");
        Player scriptPlayer = gameObjectPlayer.GetComponent<Player>();

        gameObjectPlayer.transform.position = Vector3.up * 5;
        gameObjectPlayer.GetComponent<Rigidbody2D>().gravityScale = 0;

        string pathFile = Path.Combine("Assets", "Prefabs", "Coin.prefab");
        GameObject coinObject = AssetDatabase.LoadAssetAtPath<GameObject>(pathFile);

        GameObject gameObjectCoinTest = GameObject.Instantiate(coinObject, Vector3.up * 5, Quaternion.identity);

        GameObject gameObjectScore = GameObject.Find("Score");
        Text textScore = gameObjectScore.GetComponent<Text>();

        yield return new WaitForFixedUpdate();

        int valieFieldScore = (int)TestAssistant.GetValueField(typeof(Player), "score", scriptPlayer);
        Assert.AreEqual(10, valieFieldScore,
            "In the \"{0}\" script, the \"{1}\" method incorrectly changes the value of the \"{2}\" field of the \"{3}\" script in the \"{4}\" object", 
            new object[] { coinObject.name, "OnTriggerEnter2D", "score", scriptPlayer.name, gameObjectPlayer.name });

        Assert.AreEqual("10", textScore.text,
            "In the \"{0}\" script, the \"{1}\" method incorrectly changes the value of the \"{2}\" field of the \"{3}\" component in the \"{4}\" object", 
            new object[] { coinObject.name, "OnTriggerEnter2D", "text", "Text", gameObjectScore.name });

        yield return new WaitForFixedUpdate();

        if (gameObjectCoinTest != null)
        {
            Assert.IsNull(gameObjectCoinTest,
                "In the \"{0}\" script, the \"{1}\" method does not destroy the {2} object", new object[] { coinObject.name, "OnTriggerEnter2D", coinObject.name });
        }

        GameObject gameObjectCoinTest2 = GameObject.Instantiate(coinObject, Vector3.up * 5, Quaternion.identity);

        yield return new WaitForFixedUpdate();

        valieFieldScore = (int)TestAssistant.GetValueField(typeof(Player), "score", scriptPlayer);
        Assert.AreEqual(20, valieFieldScore,
            "In the \"{0}\" script, the \"{1}\" method incorrectly changes the value of the \"{2}\" field of the \"{3}\" script in the \"{4}\" object",
            new object[] { coinObject.name, "OnTriggerEnter2D", "score", scriptPlayer.name, gameObjectPlayer.name });

        Assert.AreEqual("20", textScore.text,
            "In the \"{0}\" script, the \"{1}\" method incorrectly changes the value of the \"{2}\" field of the \"{3}\" component in the \"{4}\" object",
            new object[] { coinObject.name, "OnTriggerEnter2D", "text", "Text", gameObjectScore.name });

        yield return new WaitForFixedUpdate();

        if (gameObjectCoinTest2 != null)
        {
            Assert.IsNull(gameObjectCoinTest2,
                "In the \"{0}\" script, the \"{1}\" method does not destroy the {2} object", new object[] { coinObject.name, "OnTriggerEnter2D", coinObject.name });
        }

        GameObject gameObjectCoinTest3 = GameObject.Instantiate(coinObject, Vector3.up * 10, Quaternion.identity);

        yield return new WaitForFixedUpdate();

        gameObjectPlayer.tag = "Untagged";
        gameObjectCoinTest3.transform.position = Vector3.up * 5;

        yield return new WaitForFixedUpdate();

        valieFieldScore = (int)TestAssistant.GetValueField(typeof(Player), "score", scriptPlayer);
        Assert.AreEqual(20, valieFieldScore,
            "In the \"{0}\" script, the \"{1}\" method incorrectly changes the value of the \"{2}\" field of the \"{3}\" script in the \"{4}\" object",
            new object[] { coinObject.name, "OnTriggerEnter2D", "score", scriptPlayer.name, gameObjectPlayer.name });

        Assert.AreEqual("20", textScore.text,
            "In the \"{0}\" script, the \"{1}\" method incorrectly changes the value of the \"{2}\" field of the \"{3}\" component in the \"{4}\" object",
            new object[] { coinObject.name, "OnTriggerEnter2D", "text", "Text", gameObjectScore.name });

        yield return new WaitForFixedUpdate();

        if (gameObjectCoinTest3 == null)
        {
            Assert.IsNotNull(gameObjectCoinTest3,
                "In the \"{0}\" script, the \"{1}\" method does destroys the {2} object", new object[] { coinObject.name, "OnTriggerEnter2D", coinObject.name });
        }
    }
}
