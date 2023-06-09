using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Lesson9
{
    private string pathSceneLevel = Path.Combine("Assets", "Scenes", "Level.unity");
    private string pathSceneStartingScene = Path.Combine("Assets", "Scenes", "StartingScene.unity");

    [Test]
    public void __ExistingDirectoriesAndFiles()
    {
        var exists = File.Exists(pathSceneLevel);
        Assert.IsTrue(exists,
            "The \"{0}\" directory does not have the \"{1}\" scene", new object[] { "Scenes", "Level" });

        exists = File.Exists(pathSceneStartingScene);
        Assert.IsTrue(exists,
            "The \"{0}\" directory does not have the \"{1}\" scene", new object[] { "Scenes", "StartingScene" });

        if (SceneManager.GetActiveScene().name != "StartingScene")
        {
            EditorSceneManager.OpenScene(pathSceneStartingScene);
        }
    }

    [Test]
    public void _1CheckingObjectMainCameraInScene()
    {
        GameObject gameObjectMainCamera = GameObject.Find("Main Camera");
        Assert.IsNotNull(gameObjectMainCamera,
            "There is no \"{0}\" object on the scene", new object[] { "Main Camera" });

        /***************************Camera*************************/

        if (!gameObjectMainCamera.TryGetComponent(out Camera camera))
        {
            Assert.AreEqual(gameObjectMainCamera.AddComponent<Camera>(), camera,
                "The \"{0}\" object does not have a \"{1}\" component", new object[] { gameObjectMainCamera.name, "Camera" });
        }

        Assert.AreEqual(99, (int)(camera.backgroundColor.r * 255),
            "The \"{0}\" object in the \"{1}\" component has an incorrect value in the \"{2}\" field in the red channel", new object[] { gameObjectMainCamera.name, "Camera", "Background" });

        Assert.AreEqual(136, (int)(camera.backgroundColor.g * 255),
            "The \"{0}\" object in the \"{1}\" component has an incorrect value in the \"{2}\" field in the green channel", new object[] { gameObjectMainCamera.name, "Camera", "Background" });

        Assert.AreEqual(251, (int)(camera.backgroundColor.b * 255),
            "The \"{0}\" object in the \"{1}\" component has an incorrect value in the \"{2}\" field in the blue channel", new object[] { gameObjectMainCamera.name, "Camera", "Background" });
    }

    [Test]
    public void _2CheckingObjectCanvasOnScene()
    {
        GameObject gameObjectCanvas = GameObject.Find("Canvas");
        Assert.IsNotNull(gameObjectCanvas,
            "There is no \"{0}\" object on the scene", new object[] { "Canvas" });

        /***************************Canvas*************************/

        string nameComponent = "Canvas";

        if (!gameObjectCanvas.TryGetComponent(out Canvas canvas))
        {
            Assert.AreEqual(gameObjectCanvas.AddComponent<Canvas>(), canvas,
                "The \"{0}\" object does not have a \"{1}\" component", new object[] { gameObjectCanvas.name, nameComponent });
        }

        Assert.AreEqual(RenderMode.ScreenSpaceCamera, canvas.renderMode,
            "The \"{0}\" object in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { gameObjectCanvas.name, nameComponent, "Render Mode" });

        Assert.AreEqual(Camera.main, canvas.worldCamera,
            "The \"{0}\" object in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { gameObjectCanvas.name, nameComponent, "Render Camera" });

        /***************************CanvasScaler*************************/

        nameComponent = "Canvas Scaler";

        if (!gameObjectCanvas.TryGetComponent(out CanvasScaler canvasScaler))
        {
            Assert.AreEqual(gameObjectCanvas.AddComponent<CanvasScaler>(), canvasScaler,
                "The \"{0}\" object does not have a \"{1}\" component", new object[] { gameObjectCanvas.name, nameComponent });
        }

        Assert.AreEqual(CanvasScaler.ScaleMode.ScaleWithScreenSize, canvasScaler.uiScaleMode,
            "The \"{0}\" object in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { gameObjectCanvas.name, nameComponent, "UI Scale Mode" });

        Assert.AreEqual(new Vector2(1366, 768), canvasScaler.referenceResolution,
            "The \"{0}\" object in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { gameObjectCanvas.name, nameComponent, "Reference Resolution" });

        Assert.AreEqual(0.5f, canvasScaler.matchWidthOrHeight,
            "The \"{0}\" object in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { gameObjectCanvas.name, nameComponent, "Match" });

        /***************************GraphicRaycaster*************************/

        nameComponent = "Graphic Raycaster";

        if (!gameObjectCanvas.TryGetComponent(out GraphicRaycaster graphicRaycaster))
        {
            Assert.AreEqual(gameObjectCanvas.AddComponent<GraphicRaycaster>(), graphicRaycaster,
                "The \"{0}\" object does not have a \"{1}\" component", new object[] { gameObjectCanvas.name, nameComponent });
        }
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
    public void _4CheckingObjectPanelOnScene()
    {
        GameObject gameObjectPanel = GameObject.Find("Panel");
        Assert.IsNotNull(gameObjectPanel,
            "There is no \"{0}\" object on the scene", new object[] { "Panel" });
    }

    [Test]
    public void _5CheckingObjectTitleOnScene()
    {
        GameObject gameObjectPanel = GameObject.Find("Title");
        Assert.IsNotNull(gameObjectPanel,
            "There is no \"{0}\" object on the scene", new object[] { "Title" });

        /***************************TextMeshProUGUI*************************/

        string nameComponent = "TextMeshPro - Text (UI)";

        if (!gameObjectPanel.TryGetComponent(out TextMeshProUGUI textMeshProUGUI))
        {
            Assert.AreEqual(gameObjectPanel.AddComponent<TextMeshProUGUI>(), textMeshProUGUI,
                "The \"{0}\" object does not have a \"{1}\" component", new object[] { gameObjectPanel.name, nameComponent });
        }

        Assert.AreEqual("Demo Mario", textMeshProUGUI.text,
            "The \"{0}\" object in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { gameObjectPanel.name, nameComponent, "Text" });
    }

    [Test]
    public void _6CheckingScriptStartGame()
    {
        Type type = typeof(StartGame);

        TestAssistant.TestingMethod(type, "StartTheGame", typeof(void), MethodAttributes.Public);
    }

    [Test]
    public void _7CheckingObjectStartOnScene()
    {
        GameObject gameObjectStart = GameObject.Find("Start");
        Assert.IsNotNull(gameObjectStart,
            "There is no \"{0}\" object on the scene", new object[] { "Start" });

        /***************************Button*************************/

        string nameComponent = "Button";

        if (!gameObjectStart.TryGetComponent(out Button button))
        {
            Assert.AreEqual(gameObjectStart.AddComponent<Button>(), button,
                "The \"{0}\" object does not have a \"{1}\" component", new object[] { gameObjectStart.name, nameComponent });
        }

        Assert.AreEqual(1, button.onClick.GetPersistentEventCount(),
            "The \"{0}\" object in the \"{1}\" component the \"{2}\" event has incorrect number of methods", new object[] { gameObjectStart.name, nameComponent, "OnClick" });

        Assert.AreEqual("StartTheGame", button.onClick.GetPersistentMethodName(0),
            "The \"{0}\" object in the \"{1}\" component the \"{2}\" event has an incorrect method", new object[] { gameObjectStart.name, nameComponent, "OnClick" });

        /***************************ChildrenText*************************/

        GameObject gameObjectText = gameObjectStart.transform.GetChild(0).gameObject;
        Assert.IsNotNull(gameObjectText,
            "There is no \"{0}\" object has no children", new object[] { gameObjectStart.name });

        /***************************TextMeshProUGUI*************************/

        nameComponent = "TextMeshPro - Text (UI)";

        if (!gameObjectText.TryGetComponent(out TextMeshProUGUI textMeshProUGUI))
        {
            Assert.AreEqual(gameObjectText.AddComponent<TextMeshProUGUI>(), textMeshProUGUI,
                "The \"{0}\" object does not have a \"{1}\" component", new object[] { gameObjectText.name, nameComponent });
        }

        Assert.AreEqual("START", textMeshProUGUI.text,
            "The \"{0}\" object in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { gameObjectText.name, nameComponent, "Text" });
    }

    [Test]
    public void _8CheckingScenesInBuild()
    {
        var scnesBuild = EditorBuildSettings.scenes;

        Assert.AreEqual(2, scnesBuild.Length,
            "Incorrect number of scenes in \"Scene in Build\"");

        SceneAsset sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(scnesBuild[0].path);
        Assert.AreEqual("StartingScene", sceneAsset.name,
            "Incorrect name of the first scene");

        sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(scnesBuild[1].path);
        Assert.AreEqual("Level", sceneAsset.name,
            "Incorrect name of the second scene");
    }
}
