using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

public class Lesson6
{
    [Test]
    public void ExistDirectoresAndFiles()
    {
        var pathDirectory = Path.Combine("Assets", "Audio");
        var exists = Directory.Exists(pathDirectory);
        Assert.IsTrue(exists,
            "The \"Audio\" directory is missing!");

        pathDirectory = Path.Combine("Assets", "Audio", "Sounds");
        exists = Directory.Exists(pathDirectory);
        Assert.IsTrue(exists,
            "The \"Sounds\" directory is missing!");

        pathDirectory = Path.Combine("Assets", "Audio", "Musics");
        exists = Directory.Exists(pathDirectory);
        Assert.IsTrue(exists,
            "The \"Musics\" directory is missing!");

        var pathfile = Path.Combine("Assets", "Audio", "Sounds", "Coin.mp3");
        exists = File.Exists(pathfile);
        Assert.IsTrue(exists,
            "There is no sound \"Coin\" in the directory \"Sounds\"!");

        pathfile = Path.Combine("Assets", "Audio", "Musics", "MainMusic.mp3");
        exists = File.Exists(pathfile);
        Assert.IsTrue(exists,
            "There is no music \"MainMusic\" in the directory \"Musics\"!");
    }

    [Test]
    public void ExistComponentsObjectPlayer()
    {
        var player = GameObject.FindGameObjectWithTag("Player");

        /***************************AudioSource*************************/

        if (!player.TryGetComponent(out AudioSource audioSource))
        {
            Assert.AreEqual(player.AddComponent<AudioSource>(), audioSource,
                "The \"Player\" object does not have a AudioSource component");
        }

        Assert.IsFalse(audioSource.playOnAwake,
            "The \"Player\" object in the \"AudioSource\" component has an incorrect \"Play On Awake\" field");

        Assert.AreEqual(0, audioSource.minDistance,
            "The \"Player\" object in the \"AudioSource\" component has an incorrect \"Min Distance\" field");

        Assert.AreEqual(12, audioSource.maxDistance,
            "The \"Player\" object in the \"AudioSource\" component has an incorrect \"Max Distance\" field");
    }

    [Test]
    public void CheckingScriptPlayer()
    {
        var nameScript = "Coin";
        var pathFile = Path.Combine("Assets", "Prefabs", nameScript + ".prefab");
        var exists = File.Exists(pathFile);
        Assert.IsTrue(exists,
            $"The \"{nameScript}\" prefab is missing!");

        GameObject coinObject = AssetDatabase.LoadAssetAtPath<GameObject>(pathFile);
        var coin = coinObject.GetComponent<Coin>();

        pathFile = Path.Combine("Assets", "Audio", "Sounds", "Coin.mp3");
        AudioClip audioClip = AssetDatabase.LoadAssetAtPath<AudioClip>(pathFile);


        TestAssistant.TestingFields(typeof(Coin), "audioClip", "AudioClip", FieldAttributes.Private, true);
        TestAssistant.TestingFieldValue(typeof(Coin), "audioClip", coin, audioClip);
        TestAssistant.TestingFields(typeof(Coin), "audioSource", "AudioSource", FieldAttributes.Private);
        TestAssistant.TestingFields(typeof(Coin), "player", "GameObject", FieldAttributes.Private);
    }

    [Test]
    public void ExistOnSceneObjectMusicInObjectMainCamera()
    {
        GameObject musicObject = GameObject.Find("Music");
        Assert.IsNotNull(musicObject,
            "The \"Music\" object does not exist on the scene");

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        Assert.AreEqual(playerObject, musicObject.transform.parent.gameObject,
            "The \"Music\" object is not inside the \"Player\" object");

        /***************************AudioSource*************************/

        if (!musicObject.TryGetComponent(out AudioSource audioSource))
        {
            Assert.AreEqual(musicObject.AddComponent<AudioSource>(), audioSource,
                "The \"Coin\" object does not have a Coin component");
        }

        var pathFile = Path.Combine("Assets", "Audio", "Musics", "MainMusic.mp3");
        AudioClip audioClip = AssetDatabase.LoadAssetAtPath<AudioClip>(pathFile);

        Assert.AreEqual(audioClip, audioSource.clip,
            "The \"Music\" object in the \"AudioSource\" component has an incorrect \"Audio Clip\" field");

        Assert.IsTrue(audioSource.playOnAwake,
            "The \"Music\" object in the \"AudioSource\" component has an incorrect \"Play On Awake\" field");

        Assert.AreEqual(.2f, audioSource.volume,
            "The \"Music\" object in the \"AudioSource\" component has an incorrect \"Volume\" field");

        Assert.IsTrue(audioSource.loop,
            "The \"Music\" object in the \"AudioSource\" component has an incorrect \"Loop\" field");

    }

}
