using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class Lesson8
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
    public IEnumerator CheckingDestroyStar()
    {
        GameObject gameObjectStar = GameObject.Find("Star");
        GameObject gameObjectPlayer = GameObject.Find("Player");

        gameObjectPlayer.transform.position = Vector3.up * 5;
        gameObjectStar.transform.position = Vector3.up * 5;

        yield return TestAssistant.WaitUntilForSeconds(() => gameObjectStar == null, 1,
            $"In the \"Starpower\" script in the \"OnTriggerEnter2D\" method, there is an incorrect interaction with the \"{gameObjectPlayer.name}\" object, since the \"{gameObjectStar.name}\" object should be destroyed");
    }

    [UnityTest]
    public IEnumerator CheckingUndestroyStar()
    {
        GameObject gameObjectStar = GameObject.Find("Star");
        GameObject gameObjectPlayer = GameObject.Find("Player");
        gameObjectPlayer.tag = "Untagged";

        gameObjectPlayer.transform.position = Vector3.up * 5;
        gameObjectStar.transform.position = Vector3.up * 5;

        yield return new WaitForSeconds(1);

        if (gameObjectStar == null)
        {
            Assert.IsNotNull(gameObjectStar,
                $"In the \"Starpower\" script in the \"OnTriggerEnter2D\" method, the \"{gameObjectStar.name}\" object is destroyed not only when interacting with the \"{gameObjectPlayer.name}\" object");
        }
    }

    [UnityTest]
    public IEnumerator CheckingMethodStarPowerAnimationInClassPlayer()
    {
        GameObject gameObjectPlayer = GameObject.Find("Player");
        Player scriptPlayer = gameObjectPlayer.GetComponent<Player>();

        var methodStarPowerAnimation = TestAssistant.GetMethod(typeof(Player), "StarPowerAnimation");

        float fieldSpeed = (float)TestAssistant.GetValueField(typeof(Player), "speed", scriptPlayer);
        float fieldSpeedCoefficient = (float)TestAssistant.GetValueField(typeof(Player), "speedCoefficient", scriptPlayer);
        SpriteRenderer fieldSpriteRenderer = TestAssistant.GetValueField(typeof(Player), "spriteRenderer", scriptPlayer) as SpriteRenderer;

        Coroutine coroutineTest = scriptPlayer.StartCoroutine(methodStarPowerAnimation.Name, 3);
        DateTime now = DateTime.Now;
        yield return null;

        Assert.IsTrue(scriptPlayer.StarPower,
            $"In the \"{scriptPlayer.name}\" script the \"{methodStarPowerAnimation.Name}\" method incorrectly modifies the \"StarPower\" property");

        Assert.AreEqual(fieldSpeed * fieldSpeedCoefficient, (float)TestAssistant.GetValueField(typeof(Player), "speed", scriptPlayer),
            $"In the \"{scriptPlayer.name}\" script the \"{methodStarPowerAnimation.Name}\" method incorrectly modifies the \"speed\" field");

        for (int i = 0; i < 4; i++)
        {
            yield return null;
        }

        Assert.AreNotEqual(Color.white, fieldSpriteRenderer.color,
            $"In the \"{scriptPlayer.name}\" script the \"{methodStarPowerAnimation.Name}\" method incorrectly modifies the \"spriteRenderer.color\" field");

        yield return coroutineTest;

        Assert.IsFalse(scriptPlayer.StarPower,
            $"In the \"{scriptPlayer.name}\" script the \"{methodStarPowerAnimation.Name}\" method incorrectly modifies the \"StarPower\" property");

        Assert.AreEqual(fieldSpeed, (float)TestAssistant.GetValueField(typeof(Player), "speed", scriptPlayer),
            $"In the \"{scriptPlayer.name}\" script the \"{methodStarPowerAnimation.Name}\" method incorrectly modifies the \"speed\" field");

        Assert.AreEqual(Color.white, fieldSpriteRenderer.color,
            $"In the \"{scriptPlayer.name}\" script the \"{methodStarPowerAnimation.Name}\" method incorrectly modifies the \"spriteRenderer.color\" field");

        Assert.AreEqual(3d, (DateTime.Now - now).TotalSeconds, 3 * Time.deltaTime,
            $"In the script \"{scriptPlayer.name}\" the execution time of the method \"{methodStarPowerAnimation.Name}\" is incorrect");

        coroutineTest = scriptPlayer.StartCoroutine(methodStarPowerAnimation.Name, 1);
        now = DateTime.Now;
        yield return coroutineTest;
        Assert.AreEqual(1d, (DateTime.Now - now).TotalSeconds, 3 * Time.deltaTime,
            $"In the script \"{scriptPlayer.name}\" the execution time of the method \"{methodStarPowerAnimation.Name}\" is incorrect");
    }

    [UnityTest]
    public IEnumerator CheckingMethodStarPowerActiveInClassPlayer()
    {
        GameObject gameObjectPlayer = GameObject.FindGameObjectWithTag("Player");
        Player scriptPlayer = gameObjectPlayer.GetComponent<Player>();
        DateTime now = DateTime.Now;

        scriptPlayer.StarPowerActive();

        yield return new WaitUntil(() => !scriptPlayer.StarPower);

        Assert.AreEqual(5d, (DateTime.Now - now).TotalSeconds, 0.1d,
            $"In the script \"{scriptPlayer.name}\" method \"StarPowerActive\" incorrectly invokes the \"StarPowerAnimation\" method");

        now = DateTime.Now;
        scriptPlayer.StarPowerActive(2);

        yield return new WaitUntil(() => !scriptPlayer.StarPower);

        Assert.AreEqual(2d, (DateTime.Now - now).TotalSeconds, 0.1d,
            $"In the script \"{scriptPlayer.name}\" method \"StarPowerActive\" incorrectly invokes the \"StarPowerAnimation\" method");
    }

    [UnityTest]
    public IEnumerator CheckingMethodOnTriggerEnter2DInClassStarpower()
    {
        GameObject gameObjectStar = GameObject.Find("Star");

        GameObject gameObjectPlayer = GameObject.FindGameObjectWithTag("Player");
        Player scriptPlayer = gameObjectPlayer.GetComponent<Player>();

        gameObjectStar.transform.position = Vector3.up * 5;
        gameObjectPlayer.transform.position = Vector3.up * 5;
        yield return new WaitForFixedUpdate();

        DateTime now = DateTime.Now;

        yield return new WaitUntil(() => !scriptPlayer.StarPower);

        Assert.AreEqual(3d, (DateTime.Now - now).TotalSeconds, 0.1d,
            $"In the script \"Starpower\" method \"OnTriggerEnter2D\" incorrectly invokes the \"StarPowerActive\" method");
    }
}
