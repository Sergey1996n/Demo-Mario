using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Lesson6
{
    private string pathScene = Path.Combine("Assets", "Scenes", "Level.unity");

    [Test]
    public void __ExistingDirectoriesAndFiles()
    {
        var pathDirectory = Path.Combine("Assets", "Audio");
        var exists = Directory.Exists(pathDirectory);
        Assert.IsTrue(exists,
            "The \"{0}\" directory does not have the \"{1}\" directory", new object[] { "Assets", "Audio" });

        pathDirectory = Path.Combine("Assets", "Audio", "Sounds");
        exists = Directory.Exists(pathDirectory);
        Assert.IsTrue(exists,
            "The \"{0}\" directory does not have the \"{1}\" directory", new object[] { "Audio", "Sounds" });

        pathDirectory = Path.Combine("Assets", "Audio", "Music");
        exists = Directory.Exists(pathDirectory);
        Assert.IsTrue(exists,
            "The \"{0}\" directory does not have the \"{1}\" directory", new object[] { "Audio", "Music" });

        var pathfile = Path.Combine("Assets", "Audio", "Sounds", "Coin.mp3");
        exists = File.Exists(pathfile);
        Assert.IsTrue(exists,
            "The \"{0}\" directory does not have the \"{1}\" sound", new object[] { "Sounds", "Coin" });

        pathfile = Path.Combine("Assets", "Audio", "Music", "MainMusic.mp3");
        exists = File.Exists(pathfile);
        Assert.IsTrue(exists,
            "The \"{0}\" directory does not have the \"{1}\" music", new object[] { "Music", "MainMusic" });
    }

    [Test]
    public void __ExistingObjectsOnScene()
    {
        GameObject gameObjectPlayer = GameObject.Find("Player");
        Assert.IsNotNull(gameObjectPlayer,
            "There is no \"{0}\" object on the scene", new object[] { "Player" });

        if (!gameObjectPlayer.TryGetComponent(out Player player))
        {
            Assert.AreEqual(gameObjectPlayer.AddComponent<Player>(), player,
                "The \"{0}\" object does not have a \"{1}\" script", new object[] { gameObjectPlayer.name, "Player" });
        }

        GameObject gameObjectCoin = GameObject.Find("Coin");
        Assert.IsNotNull(gameObjectCoin,
            "There is no \"{0}\" object on the scene", new object[] { "Coin" });

        if (!gameObjectCoin.TryGetComponent(out Coin coin))
        {
            Assert.AreEqual(gameObjectCoin.AddComponent<Coin>(), coin,
                "The \"{0}\" object does not have a \"{1}\" script", new object[] { gameObjectCoin.name, "Coin" });
        }
    }

    [Test]
    public void _1CheckingObjectPlayerOnScene()
    {
        GameObject gameObjectPlayer = GameObject.Find("Player");

        /***************************AudioSource*************************/

        string nameComponent = "Audio Source";

        if (!gameObjectPlayer.TryGetComponent(out AudioSource audioSource))
        {
            Assert.AreEqual(gameObjectPlayer.AddComponent<AudioSource>(), audioSource,
                "The \"{0}\" object does not have a \"{1}\" component", new object[] { gameObjectPlayer.name, nameComponent });
        }

        Assert.IsNull(audioSource.clip,
            "The \"{0}\" object in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { gameObjectPlayer.name, nameComponent, "Audio Clip" });

        Assert.IsFalse(audioSource.playOnAwake,
            "The \"{0}\" object in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { gameObjectPlayer.name, nameComponent, "Play On Awake" });

        Assert.AreEqual(0, audioSource.minDistance,
            "The \"{0}\" object in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { gameObjectPlayer.name, nameComponent, "Min Distance" });

        Assert.AreEqual(20, audioSource.maxDistance,
            "The \"{0}\" object in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { gameObjectPlayer.name, nameComponent, "Max Distance" });
    }

    [Test]
    public void _2CheckingScriptCoin()
    {
        Type type = typeof(Coin);
        TestAssistant.TestingField(type, "audioClip", typeof(AudioClip), FieldAttributes.Private, true);
        TestAssistant.TestingField(type, "audioSource", typeof(AudioSource), FieldAttributes.Private);
        TestAssistant.TestingField(type, "player", typeof(GameObject), FieldAttributes.Private);

        TestAssistant.TestingMethod(type, "Start", typeof(void), MethodAttributes.Private);
        TestAssistant.TestingMethod(type, "OnTriggerEnter2D", typeof(void), MethodAttributes.Private, new MyParameterInfo[] { new MyParameterInfo(typeof(Collider2D), "collision") });

        GameObject gameObjectCoin = GameObject.Find("Coin");
        Coin scriptCoin = gameObjectCoin.GetComponent<Coin>();

        string pathFile = Path.Combine("Assets", "Audio", "Sounds", "Coin.mp3");
        AudioClip audioClip = AssetDatabase.LoadAssetAtPath<AudioClip>(pathFile);

        TestAssistant.TestingFieldValue(typeof(Coin), "audioClip", scriptCoin, audioClip);
    }

    [Test]
    public void _3CheckingObjectMusicInObjectMainCamera()
    {
        GameObject gameObjectMusic = GameObject.Find("Music");
        Assert.IsNotNull(gameObjectMusic,
            "There is no \"{0}\" object on the scene", new object[] { "Music" });

        Assert.AreEqual(Camera.main.gameObject, gameObjectMusic.transform.parent.gameObject,
            "The \"{0}\" object has the incorrect parent", new object[] { gameObjectMusic.name });

        /***************************Transform*************************/

        string nameComponent = "Transform";

        if (!gameObjectMusic.TryGetComponent(out Transform transform))
        {
            Assert.AreEqual(gameObjectMusic.AddComponent<Transform>(), transform,
                "The \"{0}\" object does not have a \"{1}\" component", new object[] { gameObjectMusic.name, nameComponent });
        }

        Assert.AreEqual(Vector3.zero, transform.localPosition,
            "The \"{0}\" object in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { gameObjectMusic.name, nameComponent, "Position" });

        /***************************AudioSource*************************/

        nameComponent = "Audio Source";

        if (!gameObjectMusic.TryGetComponent(out AudioSource audioSource))
        {
            Assert.AreEqual(gameObjectMusic.AddComponent<AudioSource>(), audioSource,
                "The \"{0}\" object does not have a \"{1}\" component", new object[] { gameObjectMusic.name, nameComponent });
        }

        var pathFile = Path.Combine("Assets", "Audio", "Music", "MainMusic.mp3");
        AudioClip audioClip = AssetDatabase.LoadAssetAtPath<AudioClip>(pathFile);

        Assert.AreEqual(audioClip, audioSource.clip,
            "The \"{0}\" object in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { gameObjectMusic.name, nameComponent, "Audio Clip" });

        Assert.IsTrue(audioSource.playOnAwake,
            "The \"{0}\" object in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { gameObjectMusic.name, nameComponent, "Play On Awake" });

        Assert.AreEqual(.2f, audioSource.volume,
            "The \"{0}\" object in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { gameObjectMusic.name, nameComponent, "Volume" });

        Assert.IsTrue(audioSource.loop,
            "The \"{0}\" object in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { gameObjectMusic.name, nameComponent, "Loop" });

        Assert.AreEqual(0, audioSource.minDistance,
            "The \"{0}\" object in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { gameObjectMusic.name, nameComponent, "Min Distance" });

        Assert.AreEqual(20, audioSource.maxDistance,
            "The \"{0}\" object in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { gameObjectMusic.name, nameComponent, "Max Distance" });
    }

    [Test]
    public void _4CheckingObjectMainCamera()
    {
        GameObject gameObjectMusic = Camera.main.gameObject;

        /***************************Transform*************************/

        string nameComponent = "Transform";

        if (!gameObjectMusic.TryGetComponent(out Transform transform))
        {
            Assert.AreEqual(gameObjectMusic.AddComponent<Transform>(), transform,
                "The \"{0}\" object does not have a \"{1}\" component", new object[] { gameObjectMusic.name, nameComponent });
        }

        Assert.AreEqual(Vector3.back * 10, transform.position,
            "The \"{0}\" object in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { gameObjectMusic.name, nameComponent, "Position" });
    }

    [Test]
    public void _5InitializingVariablesScriptCoin()
    {
        Type type = typeof(Coin);
        GameObject gameObjectCoin = GameObject.Find("Coin");
        Coin scriptCoin = gameObjectCoin.GetComponent<Coin>();

        var methodStart = TestAssistant.GetMethod(type, "Start");
        methodStart.Invoke(scriptCoin, null);

        AudioSource fieldAudioSource = TestAssistant.GetValueField(type, "audioSource", scriptCoin) as AudioSource;
        GameObject fieldPlayer = TestAssistant.GetValueField(type, "player", scriptCoin) as GameObject;

        GameObject gameObjectPlayer = GameObject.Find("Player");

        Assert.AreEqual(gameObjectPlayer.GetComponent<AudioSource>(), fieldAudioSource,
            "The \"{0}\" method does not work correctly in the \"{1}\" class (there is no reference to the component in the \"{2}\" field)", new object[] { methodStart.Name, scriptCoin.name, "audioSource" });

        Assert.AreEqual(gameObjectPlayer, fieldPlayer,
            "The \"{0}\" method does not work correctly in the \"{1}\" class (there is no reference to the component in the \"{2}\" field)", new object[] { methodStart.Name, scriptCoin.name, "player" });

        EditorSceneManager.OpenScene(pathScene);
    }
}
