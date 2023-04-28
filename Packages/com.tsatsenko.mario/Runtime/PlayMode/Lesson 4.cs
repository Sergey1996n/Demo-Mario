using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class Lesson4
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
    public IEnumerator _1CheckingAnimatorStateObjectPlayer()
    {
        GameObject gameObjectPlayer = GameObject.Find("Player");
        Player scriptPlayer = gameObjectPlayer.GetComponent<Player>();
        Animator animator = gameObjectPlayer.GetComponent<Animator>();
        
        yield return null;

        var animatorClip = animator.GetCurrentAnimatorClipInfo(0);
        Assert.AreEqual("Idle", animatorClip.First().clip.name,
            "The \"{0}\" object has an incorrect animation by default", new object[] { gameObjectPlayer.name });

        bool fieldIsGrounded = (bool)TestAssistant.GetValueField(typeof(Player), "isGrounded", scriptPlayer);
        MethodInfo methodJump = TestAssistant.GetMethod(typeof(Player), "Jump");

        if (!fieldIsGrounded)
        {
            yield return TestAssistant.WaitUntilForSeconds(() => (bool)TestAssistant.GetValueField(typeof(Player), "isGrounded", scriptPlayer), 10,
                "In the \"{0}\" script, the \"{1}\" method works incorrectly", new object[] { "Player", "OnCollisionEnter2D" });
        }

        methodJump.Invoke(scriptPlayer, new object[] { });

        yield return TestAssistant.WaitUntilForSeconds(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"), 1,
            "The \"{0}\" object has an incorrect animation when executing the \"{1}\" method", new object[] { gameObjectPlayer.name, methodJump.Name });
    }
}
