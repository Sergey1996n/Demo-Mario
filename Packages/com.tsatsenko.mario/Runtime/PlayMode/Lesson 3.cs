using System.Collections;
using System.Reflection;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using System.IO;
using System;

public class Lesson3
{
    /* Перенести в TestAssistent */
    private IEnumerator WaitUntilForSeconds(Func<bool> checker, float maxTime, string message, params object[] args)
    {
        float timer = 0;
        while (timer < maxTime)
        {
            bool check = checker.Invoke();
            if (check)
            {
                yield break;
            }
            yield return null;
            timer += Time.deltaTime;
        }
        
        Assert.Fail(message, args);
    }
    [UnitySetUp]
    public IEnumerator Setup()
    {
        string pathScene = Path.Combine("Assets", "Scenes", "SampleScene.unity");
        yield return EditorSceneManager.LoadSceneAsyncInPlayMode(pathScene, new LoadSceneParameters(LoadSceneMode.Single));
    }

    [UnityTest]
    public IEnumerator CheckingJumpPlayer()
    {
        GameObject gameObjectPlayer = GameObject.FindGameObjectWithTag("Player");
        Player scriptPlayer = gameObjectPlayer.GetComponent<Player>();

        bool fieldIsGrounded = (bool)TestAssistant.GetValueField(typeof(Player), "isGrounded", scriptPlayer);
        MethodInfo methodJump = TestAssistant.GetMethod(typeof(Player), "Jump");

        if (!fieldIsGrounded)
        {
            yield return WaitUntilForSeconds(() => (bool)TestAssistant.GetValueField(typeof(Player), "isGrounded", scriptPlayer), 10,
                "In the \"{0}\" script, the \"{1}\" method works incorrectly", new object[] { "Player", "OnCollisionEnter2D" });
            //yield return new WaitUntilForSeconds(), 10, "CheckingJumpPlayer");
            //yield return new WaitUntil(() => (bool)TestAssistant.GetValueField(typeof(Player), "isGrounded", scriptPlayer)/* || MaxTime(10)*/);
        }

        var position = gameObjectPlayer.transform.position;

        methodJump.Invoke(scriptPlayer, new object[] { });
        yield return new WaitForSeconds(0.2f);

        Assert.Greater(gameObjectPlayer.transform.position.y, position.y,
            $"The \"Jump\" method does not increase the position by Y!");
    }
}
