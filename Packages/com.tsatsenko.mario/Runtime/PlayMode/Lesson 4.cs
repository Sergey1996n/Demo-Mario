using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class Lesson4
{
    [UnitySetUp]
    public IEnumerator Setup()
    {
        yield return EditorSceneManager.LoadSceneAsyncInPlayMode("Assets/Scenes/SampleScene.unity", new LoadSceneParameters(LoadSceneMode.Single));
    }


    [UnityTest]
    public IEnumerator InitializingVariables()
    {
        var gameObjectPlayer = GameObject.FindGameObjectWithTag("Player");
        var scriptPlayer = gameObjectPlayer.GetComponent<Player>();

        var typePlayer = typeof(Player);
        var bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;

        var fieldSpriteRenderer = typePlayer.GetField("spriteRenderer", bindingFlags);
        var fieldAnimator = typePlayer.GetField("animator", bindingFlags);

        yield return new WaitForEndOfFrame();

        Assert.AreEqual(gameObjectPlayer.GetComponent<SpriteRenderer>(), fieldSpriteRenderer.GetValue(scriptPlayer),
            $"There is no reference to the \"{gameObjectPlayer.GetComponent<SpriteRenderer>()}\" component in the \"{fieldSpriteRenderer.Name}\" field!");

        Assert.AreEqual(gameObjectPlayer.GetComponent<Animator>(), fieldAnimator.GetValue(scriptPlayer),
            $"There is no reference to the \"{gameObjectPlayer.GetComponent<Animator>()}\" component in the \"{fieldAnimator.Name}\" field!");
    }
}
