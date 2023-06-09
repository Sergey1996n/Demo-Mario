using System.Collections;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class Lesson7
{
    private string pathScene = Path.Combine("Assets", "Scenes", "Level.unity");
    private bool isLoad = false;

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        isLoad = true;
    }

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
    public IEnumerator CheckingDestroyEnemy()
    {
        GameObject gameObjectPlayer = GameObject.Find("Player");
        GameObject gameObjectEnemy = GameObject.Find("Enemy");

        gameObjectPlayer.transform.position = Vector3.up * 10 + Vector3.up * 0.5f;
        gameObjectEnemy.transform.position = Vector3.up * 10;

        yield return TestAssistant.WaitUntilForSeconds(() => gameObjectEnemy == null, 1,
            "In the \"Enemy\" script in the \"OnCollisionEnter2D\" method, there is an incorrect interaction with the \"Player\" object, since the \"Enemy\" object should be destroyed");
    }

    [UnityTest]
    public IEnumerator CheckingUndestroyEnemy()
    {
        GameObject gameObjectPlayer = GameObject.Find("Player");
        GameObject gameObjectEnemy = GameObject.Find("Enemy");
        gameObjectPlayer.tag = "Untagged";

        gameObjectPlayer.transform.position = Vector3.up * 10 + Vector3.up * 0.5f;
        gameObjectEnemy.transform.position = Vector3.up * 10;

        yield return new WaitForSeconds(1);
        if (gameObjectEnemy == null)
        {
            Assert.Fail("In the \"Enemy\" script in the \"OnCollisionEnter2D\" method, the \"Enemy\" object is destroyed not only when interacting with the \"Player\" object");
        }
    }


    [UnityTest]
    public IEnumerator CheckingDestroyPlayer()
    {
        EditorSceneManager.sceneLoaded += OnSceneLoaded;
        GameObject gameObjectPlayer = GameObject.Find("Player");
        GameObject gameObjectEnemy = GameObject.Find("Enemy");

        gameObjectPlayer.transform.position = Vector3.up * 10;
        gameObjectEnemy.transform.position = Vector3.up * 10;

        yield return TestAssistant.WaitUntilForSeconds(() => isLoad, 2,
            "In the \"Enemy\" script in the \"OnCollisionEnter2D\" method, there is an incorrect interaction with the \"Player\" object, because the scene does not restart");

        isLoad = false;
        EditorSceneManager.sceneLoaded -= OnSceneLoaded;
    }

    [UnityTest]
    public IEnumerator CheckingUndestroyPlayer()
    {
        EditorSceneManager.sceneLoaded += OnSceneLoaded;
        GameObject gameObjectPlayer = GameObject.Find("Player");
        GameObject gameObjectEnemy = GameObject.Find("Enemy");
        gameObjectPlayer.tag = "Untagged";

        gameObjectPlayer.transform.position = Vector3.up * 10;
        gameObjectEnemy.transform.position = Vector3.up * 10;

        yield return new WaitForSeconds(1);
        if (isLoad)
        {
            Assert.Fail("In the \"Enemy\" script in the \"OnCollisionEnter2D\" method, the scene is restarted not only when interacting with the Player object");
        }

        isLoad = false;
        EditorSceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private GameObject GetGameObject()
    {
        string pathFile = Path.Combine("Assets", "Prefabs", "Enemy.prefab");
        GameObject objectEnemy = AssetDatabase.LoadAssetAtPath<GameObject>(pathFile);

        Enemy scriptEnemyTest = objectEnemy.GetComponent<Enemy>();
        objectEnemy.layer = LayerMask.NameToLayer("Ground");
        TestAssistant.SetValueField(typeof(Enemy), "speed", scriptEnemyTest, 0);

        pathFile = Path.Combine("Assets", "Sprites", "Tileset.png");
        Object[] spritesTileset = AssetDatabase.LoadAllAssetRepresentationsAtPath(pathFile);
        objectEnemy.GetComponent<SpriteRenderer>().sprite = spritesTileset[2] as Sprite;

        return objectEnemy;
    }

    [UnityTest]
    public IEnumerator CheckingChangeDirection()
    {
        GameObject gameObjectEnemy = GameObject.Find("Enemy");
        Enemy scriptEnemy = gameObjectEnemy.GetComponent<Enemy>();
        gameObjectEnemy.transform.position = Vector3.up * 5;

        yield return new WaitForFixedUpdate();

        yield return new WaitUntil(() => gameObjectEnemy.GetComponent<Rigidbody2D>().velocity.y > -0.1);

        GameObject gameObjectEnemyTest = MonoBehaviour.Instantiate(GetGameObject(), gameObjectEnemy.transform.position + Vector3.left, Quaternion.identity);

        Vector2 fieldDirectionTest = (Vector2)TestAssistant.GetValueField(typeof(Enemy), "direction", scriptEnemy);
        float fieldSpeed = (float)TestAssistant.GetValueField(typeof(Enemy), "speed", scriptEnemy);
        Rigidbody2D fieldRigidbody2D = TestAssistant.GetValueField(typeof(Enemy), "rigidbody2d", scriptEnemy) as Rigidbody2D;

        yield return new WaitForSeconds(1);

        Assert.AreEqual(-fieldDirectionTest, (Vector2)TestAssistant.GetValueField(typeof(Enemy), "direction", scriptEnemy),
            "In the \"Enemy\" script in the \"OnCollisionEnter2D\" method, the \"Enemy\" object does not change the \"direction\" field");

        Assert.AreEqual(-fieldDirectionTest.x * fieldSpeed, fieldRigidbody2D.velocity.x, 0.001d,
            "In the \"Enemy\" script, the \"OnCollisionEnter2D\" method incorrectly changes the \"rigidbody2d field.velocity\" on the X axis");
    }
}
