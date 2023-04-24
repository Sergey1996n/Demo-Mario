using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class Lesson6
{
    [UnitySetUp]
    public IEnumerator Setup()
    {
        yield return EditorSceneManager.LoadSceneAsyncInPlayMode("Assets/Scenes/SampleScene.unity", new LoadSceneParameters(LoadSceneMode.Single));
    }

    [UnityTest]
    public IEnumerator InitializingVariablesScriptCoin()
    {
        var pathFile = Path.Combine("Assets", "Prefabs", "Coin.prefab");
        var objectCoin = AssetDatabase.LoadAssetAtPath<GameObject>(pathFile);

        GameObject gameObjectCoin = GameObject.Instantiate(objectCoin, -Vector3.one, Quaternion.identity);
        var coin = gameObjectCoin.GetComponent<Coin>();

        var typeCoin = typeof(Coin);
        var bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;

        var fieldPlayer = typeCoin.GetField("player", bindingFlags);
        var fieldAudioSource = typeCoin.GetField("audioSource", bindingFlags);

        GameObject gameObjectPlayer = GameObject.FindGameObjectWithTag("Player");

        Assert.IsNotNull(gameObjectPlayer);

        yield return null;

        Assert.AreEqual(gameObjectPlayer, fieldPlayer.GetValue(coin),
            $"There is no reference to the \"{gameObjectPlayer}\" object in the \"{fieldPlayer.Name}\" field!");

        Assert.AreEqual(gameObjectPlayer.GetComponent<AudioSource>(), fieldAudioSource.GetValue(coin),
            $"There is no reference to the \"{gameObjectPlayer.GetComponent<AudioSource>()}\" component in the \"{fieldAudioSource.Name}\" field!");
    }

    [UnityTest]
    public IEnumerator CheckingMethodOnTriggerEnter2DObjectCoin()
    {
        GameObject gameObjectPlayer = GameObject.Find("Player");

        var pathFile = Path.Combine("Assets", "Prefabs", "Coin.prefab");
        GameObject objectCoin = AssetDatabase.LoadAssetAtPath<GameObject>(pathFile);

        Assert.IsNotNull(objectCoin);

        var typeCoin = typeof(Coin);
        var bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;

        var fieldAudioSource = typeCoin.GetField("audioSource", bindingFlags);
        var fieldClip = typeCoin.GetField("audioClip", bindingFlags);
        var fieldPlayer = typeCoin.GetField("player", bindingFlags);

        GameObject coinObjectTest = GameObject.Instantiate(objectCoin, -Vector3.one, Quaternion.identity);
        Coin coin = coinObjectTest.GetComponent<Coin>();

        yield return new WaitUntil(() => fieldAudioSource.GetValue(coin) != null);

        AudioClip audioClip = fieldClip.GetValue(coin) as AudioClip;
        AudioSource audioSource = fieldAudioSource.GetValue(coin) as AudioSource;

        bool isSound = false;
        float[] samplesLSource = new float[1024];

        gameObjectPlayer.transform.position = -Vector3.one;

        for (float i = 0; i < audioClip.length; i += Time.deltaTime)
        {
            audioSource.GetOutputData(samplesLSource, 0);

            isSound = samplesLSource.Any(s => s != 0);
            if (isSound) break;
            yield return null;
        }

        Assert.IsTrue(isSound,
            "The \"OnTriggerEnter2D\" method of the \"Coin\" object does not start the sound");

    }
}
