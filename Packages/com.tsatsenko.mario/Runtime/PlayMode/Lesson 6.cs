using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class Lesson6
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
    public IEnumerator CheckingMethodOnTriggerEnter2DObjectCoinSoundOn()
    {
        GameObject gameObjectPlayer = GameObject.Find("Player");

        gameObjectPlayer.transform.position = Vector3.up * 5;
        gameObjectPlayer.GetComponent<Rigidbody2D>().gravityScale = 0;

        string pathFile = Path.Combine("Assets", "Prefabs", "Coin.prefab");
        GameObject coinObject = AssetDatabase.LoadAssetAtPath<GameObject>(pathFile);

        GameObject gameObjectCoinTest = GameObject.Instantiate(coinObject, Vector3.up * 5, Quaternion.identity);
        Coin scriptCoin = gameObjectCoinTest.GetComponent<Coin>();

        yield return new WaitForFixedUpdate();

        AudioClip valueAudioClip = TestAssistant.GetValueField(typeof(Coin), "audioClip", scriptCoin) as AudioClip;
        AudioSource valueAudioSource = TestAssistant.GetValueField(typeof(Coin), "audioSource", scriptCoin) as AudioSource;

        float[] samplesLSource = new float[1024];

        yield return TestAssistant.WaitUntilForSeconds(() =>
            {
                valueAudioSource.GetOutputData(samplesLSource, 0);
                return samplesLSource.Any(s => s != 0);
             
            }, 
            valueAudioClip.length,
            "In the \"{0}\" script, the \"{1}\" method does not start the sound", coinObject.name, "OnTriggerEnter2D");
    }

    [UnityTest]
    public IEnumerator CheckingMethodOnTriggerEnter2DObjectCoinSoundOff()
    {
        GameObject gameObjectPlayer = GameObject.Find("Player");

        gameObjectPlayer.transform.position = Vector3.up * 5;
        gameObjectPlayer.GetComponent<Rigidbody2D>().gravityScale = 0;

        string pathFile = Path.Combine("Assets", "Prefabs", "Coin.prefab");
        GameObject coinObject = AssetDatabase.LoadAssetAtPath<GameObject>(pathFile);

        GameObject gameObjectCoinTest = GameObject.Instantiate(coinObject);
        Coin scriptCoin = gameObjectCoinTest.GetComponent<Coin>();

        yield return new WaitForFixedUpdate();

        gameObjectPlayer.tag = "Untagged";
        gameObjectCoinTest.transform.position = Vector3.up * 5;

        yield return new WaitForFixedUpdate();

        AudioClip valueAudioClip = TestAssistant.GetValueField(typeof(Coin), "audioClip", scriptCoin) as AudioClip;
        AudioSource valueAudioSource = TestAssistant.GetValueField(typeof(Coin), "audioSource", scriptCoin) as AudioSource;

        float[] samplesLSource = new float[1024];

        float time = 0;
        yield return new WaitUntil(() =>
        {
            valueAudioSource.GetOutputData(samplesLSource, 0);
            if (time > valueAudioClip.length)
            {
                return true;
            }
            time += Time.deltaTime;
            return samplesLSource.Any(s => s != 0);
        });

        if (time < valueAudioClip.length)
        {
            Assert.Fail("In the \"{0}\" script, the \"{1}\" method does not start the sound", coinObject.name, "OnTriggerEnter2D");
        }
    }

    [UnityTest]
    public IEnumerator CheckingMainMusic()
    {
        GameObject gameObjectMusic = GameObject.Find("Music");
        if (gameObjectMusic.TryGetComponent(out AudioSource audioSource))
        {
            float[] samplesLSource = new float[1024];
            yield return TestAssistant.WaitUntilForSeconds(() =>
                {
                    audioSource.GetOutputData(samplesLSource, 0);
                    return samplesLSource.Any(s => s != 0);
                },
                10f,
                "The \"{0}\" object does not start music", gameObjectMusic.name);
        }
    }
}
