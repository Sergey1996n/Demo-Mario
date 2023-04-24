using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class Lesson8
{

    [UnitySetUp]
    public IEnumerator Setup()
    {
        yield return EditorSceneManager.LoadSceneAsyncInPlayMode("Assets/Scenes/SampleScene.unity", new LoadSceneParameters(LoadSceneMode.Single));
    }

    //[UnityTest]
    //public IEnumerator _InitializingVariablesScriptPlayer()
    //{
    //    GameObject gameObjectEnemy = GameObject.FindGameObjectWithTag("Player");
    //    var scriptEnemy = gameObjectEnemy.GetComponent<Enemy>();
    //    var rigidbody2dEnemy = gameObjectEnemy.GetComponent<Rigidbody2D>();

    //    Rigidbody2D fieldRigidbody2d = TestAssistant.GetValueField(typeof(Enemy), "rigidbody2d", scriptEnemy) as Rigidbody2D;

    //    Assert.AreEqual(rigidbody2dEnemy, fieldRigidbody2d,
    //        $"There is no reference to the \"{rigidbody2dEnemy}\" component in the \"rigidbody2d\" field!");

    //    yield return new WaitForEndOfFrame();

    //    Assert.AreEqual(Vector2.left * 3, fieldRigidbody2d.velocity,
    //        "The \"Enemy\" object in the \"Enemy\" script in the \"Start\" method has the \"rigidbody2d.velocity\" field set incorrectly");
    //}

    [UnityTest]
    public IEnumerator CheckingDestroyStar()
    {
        GameObject gameObjectStar = GameObject.Find("Star");
        GameObject gameObjectPlayer = GameObject.FindGameObjectWithTag("Player");

        gameObjectPlayer.transform.position = -Vector3.one;
        gameObjectStar.transform.position = -Vector3.one;

        float timeout = 1;
        while (gameObjectStar != null && timeout > 0)
        {
            yield return null;
            timeout -= Time.deltaTime;
        }

        if (gameObjectStar != null)
        {
            Assert.IsNull(gameObjectStar,
                $"In the \"Starpower\" script in the \"OnTriggerEnter2D\" method, there is an incorrect interaction with the \"{gameObjectPlayer.name}\" object, since the \"{gameObjectStar.name}\" object should be destroyed");
        }
    }

    [UnityTest]
    public IEnumerator CheckingUndestroyStar()
    {
        GameObject gameObjectStar = GameObject.Find("Star");
        GameObject gameObjectPlayer = GameObject.FindGameObjectWithTag("Player");
        gameObjectPlayer.tag = "Untagged";

        gameObjectPlayer.transform.position = -Vector3.one;
        gameObjectStar.transform.position = -Vector3.one;

        float timeout = 1;
        while (gameObjectStar != null && timeout > 0)
        {
            yield return null;
            timeout -= Time.deltaTime;
        }

        if (gameObjectStar == null)
        {
            Assert.IsNotNull(gameObjectStar,
                $"In the \"Starpower\" script in the \"OnTriggerEnter2D\" method, the \"{gameObjectStar.name}\" object is destroyed not only when interacting with the \"{gameObjectPlayer.name}\" object");
        }
    }

    [UnityTest]
    public IEnumerator CheckingMethodStarPowerAnimationInClassPlayer()
    {
        GameObject gameObjectPlayer = GameObject.FindGameObjectWithTag("Player");
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

        gameObjectStar.transform.position = -Vector3.one;
        gameObjectPlayer.transform.position = -Vector3.one;
        yield return new WaitForFixedUpdate();

        DateTime now = DateTime.Now;

        yield return new WaitUntil(() => !scriptPlayer.StarPower);

        Assert.AreEqual(3d, (DateTime.Now - now).TotalSeconds, 0.1d,
            $"In the script \"Starpower\" method \"OnTriggerEnter2D\" incorrectly invokes the \"StarPowerActive\" method");
    }
}
