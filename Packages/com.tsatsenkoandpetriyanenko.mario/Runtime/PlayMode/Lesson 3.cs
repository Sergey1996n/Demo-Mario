using System.Collections;
using System.Reflection;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using System.IO;

public class Lesson3
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
    public IEnumerator CheckingJumpPlayer()
    {
        GameObject gameObjectPlayer = GameObject.FindGameObjectWithTag("Player");
        Player scriptPlayer = gameObjectPlayer.GetComponent<Player>();

        bool fieldIsGrounded = (bool)TestAssistant.GetValueField(typeof(Player), "isGrounded", scriptPlayer);
        MethodInfo methodJump = TestAssistant.GetMethod(typeof(Player), "Jump");

        if (!fieldIsGrounded)
        {
            yield return TestAssistant.WaitUntilForSeconds(() => (bool)TestAssistant.GetValueField(typeof(Player), "isGrounded", scriptPlayer), 10,
                "In the \"{0}\" script, the \"{1}\" method works incorrectly", new object[] { "Player", "OnCollisionEnter2D" });
        }

        var position = gameObjectPlayer.transform.position;

        methodJump.Invoke(scriptPlayer, new object[] { });
        yield return new WaitForSeconds(0.2f);

        Assert.Greater(gameObjectPlayer.transform.position.y, position.y,
            "In the \"{0}\" script, the \"{1}\" method works incorrectly", new object[] { "Player", "Jump" });
    }
}
