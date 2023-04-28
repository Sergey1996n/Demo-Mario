using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class Lesson9
{
    private string pathSceneLevel = Path.Combine("Assets", "Scenes", "Level.unity");
    private string pathSceneStartingScene = Path.Combine("Assets", "Scenes", "StartingScene.unity");
    private bool isLoad = false;

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        isLoad = true;
    }

    [UnityTest]
    public IEnumerator CheckingOnClickButtonStart()
    {
        yield return EditorSceneManager.LoadSceneAsyncInPlayMode(pathSceneStartingScene, new LoadSceneParameters(LoadSceneMode.Single));
        EditorSceneManager.sceneLoaded += OnSceneLoaded;

        GameObject gameObjectButton = GameObject.Find("Start");
        gameObjectButton.GetComponent<Button>().onClick.Invoke();

        yield return TestAssistant.WaitUntilForSeconds(() => isLoad, 1,
            "When you click on the \"Start\" button, it does not go to another scene");

        Assert.AreEqual("Level", EditorSceneManager.GetActiveScene().name,
            "When you click on the \"Start\" button, it goes to another scene");

        isLoad = false;
        EditorSceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
