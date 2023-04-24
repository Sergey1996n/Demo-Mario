using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class Lesson7
{
    private bool isLoad = false;

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        isLoad = true;
    }

    [UnitySetUp]
    public IEnumerator Setup()
    {
        yield return EditorSceneManager.LoadSceneAsyncInPlayMode("Assets/Scenes/SampleScene.unity", new LoadSceneParameters(LoadSceneMode.Single));
    }

    [UnityTest]
    public IEnumerator _InitializingVariablesScriptEnemy()
    {
        GameObject gameObjectEnemy = GameObject.FindGameObjectWithTag("Enemy"); 
        var scriptEnemy = gameObjectEnemy.GetComponent<Enemy>();
        var rigidbody2dEnemy = gameObjectEnemy.GetComponent<Rigidbody2D>();

        Rigidbody2D fieldRigidbody2d = TestAssistant.GetValueField(typeof(Enemy), "rigidbody2d", scriptEnemy) as Rigidbody2D;

        Assert.AreEqual(rigidbody2dEnemy, fieldRigidbody2d,
            $"There is no reference to the \"{rigidbody2dEnemy}\" component in the \"rigidbody2d\" field!");

        yield return new WaitForEndOfFrame();

        Assert.AreEqual(Vector2.left * 3, fieldRigidbody2d.velocity,
            "The \"Enemy\" object in the \"Enemy\" script in the \"Start\" method has the \"rigidbody2d.velocity\" field set incorrectly");
    }

    [UnityTest]
    public IEnumerator CheckingDestroyEnemy()
    {
        GameObject gameObjectEnemy = GameObject.FindGameObjectWithTag("Enemy");
        GameObject gameObjectPlayer = GameObject.FindGameObjectWithTag("Player");

        gameObjectPlayer.transform.position = -Vector3.one + Vector3.up * 0.5f;
        gameObjectEnemy.transform.position = -Vector3.one;

        float timeout = 1;
        while (gameObjectEnemy != null && timeout > 0)
        {
            yield return null;
            timeout -= Time.deltaTime;
        }

        if (gameObjectEnemy != null)
        {
            Assert.IsNull(gameObjectEnemy,
                "In the \"Enemy\" script in the \"OnCollisionEnter2D\" method, there is an incorrect interaction with the \"Player\" object, since the \"Enemy\" object should be destroyed");
        }
    }

    [UnityTest]
    public IEnumerator CheckingUndestroyEnemy()
    {
        GameObject gameObjectEnemy = GameObject.FindGameObjectWithTag("Enemy");
        GameObject gameObjectPlayer = GameObject.FindGameObjectWithTag("Player");
        gameObjectPlayer.tag = "Untagged";

        gameObjectPlayer.transform.position = -Vector3.one + Vector3.up * 0.5f;
        gameObjectEnemy.transform.position = -Vector3.one;

        float timeout = 1;
        while (gameObjectEnemy != null && timeout > 0)
        {
            yield return null;
            timeout -= Time.deltaTime;
        }

        if (gameObjectEnemy == null)
        {
            Assert.IsNotNull(gameObjectEnemy,
                "In the \"Enemy\" script in the \"OnCollisionEnter2D\" method, the \"Enemy\" object is destroyed not only when interacting with the \"Player\" object");
        }
    }


    [UnityTest]
    public IEnumerator CheckingDestroyPlayer()
    {
        EditorSceneManager.sceneLoaded += OnSceneLoaded;
        GameObject gameObjectEnemy = GameObject.FindGameObjectWithTag("Enemy");
        GameObject gameObjectPlayer = GameObject.FindGameObjectWithTag("Player");

        gameObjectPlayer.transform.position = -Vector3.one;
        gameObjectEnemy.transform.position = -Vector3.one;

        isLoad = false;
        yield return null;
        float timeout = 2;
        while (!isLoad && timeout > 0)
        {
            yield return null;
            timeout -= Time.deltaTime;
        }
        if (!isLoad)
        {
            Assert.Fail("In the \"Enemy\" script in the \"OnCollisionEnter2D\" method, there is an incorrect interaction with the \"Player\" object, because the scene does not restart");
        }

        EditorSceneManager.sceneLoaded -= OnSceneLoaded;
    }

    [UnityTest]
    public IEnumerator CheckingUndestroyPlayer()
    {
        EditorSceneManager.sceneLoaded += OnSceneLoaded;
        GameObject gameObjectEnemy = GameObject.FindGameObjectWithTag("Enemy");
        GameObject gameObjectPlayer = GameObject.FindGameObjectWithTag("Player");
        gameObjectPlayer.tag = "Untagged";

        gameObjectPlayer.transform.position = -Vector3.one;
        gameObjectEnemy.transform.position = -Vector3.one;

        isLoad = false;
        yield return null;
        float timeout = 2;
        while (!isLoad && timeout > 0)
        {
            yield return null;
            timeout -= Time.deltaTime;
        }
        if (isLoad)
        {
            Assert.Fail("In the \"Enemy\" script in the \"OnCollisionEnter2D\" method, the scene is restarted not only when interacting with the Player object");
        }

        EditorSceneManager.sceneLoaded -= OnSceneLoaded;
    }

    [UnityTest]
    public IEnumerator CheckingChangeDirection()
    {
        GameObject gameObjectEnemy = GameObject.FindGameObjectWithTag("Enemy");
        Enemy scriptEnemy = gameObjectEnemy.GetComponent<Enemy>();
        gameObjectEnemy.transform.position = -Vector3.one;

        GameObject gameObjectEnemyTest = MonoBehaviour.Instantiate(gameObjectEnemy, -Vector3.one, Quaternion.identity);
        gameObjectEnemyTest.tag = "Untagged";

        Vector2 fieldDirectionTest = (Vector2)TestAssistant.GetValueField(typeof(Enemy), "direction", scriptEnemy);
        float fieldSpeed = (float)TestAssistant.GetValueField(typeof(Enemy), "speed", scriptEnemy);
        Rigidbody2D fieldRigidbody2D = TestAssistant.GetValueField(typeof(Enemy), "rigidbody2d", scriptEnemy) as Rigidbody2D;


        yield return new WaitForFixedUpdate();
        Assert.AreEqual(-fieldDirectionTest, (Vector2)TestAssistant.GetValueField(typeof(Enemy), "direction", scriptEnemy),
            "In the \"Enemy\" script in the \"OnCollisionEnter2D\" method, the \"Enemy\" object does not change the \"direction\" field");

        Assert.AreEqual(-fieldDirectionTest * fieldSpeed, new Vector2 (fieldRigidbody2D.velocity.x, 0),
            "In the \"Enemy\" script in the \"OnCollisionEnter2D\" method, the \"Enemy\" object does not change the \"rigidbody2d.velocity\" field");
    }

    [UnityTest]
    public IEnumerator CheckingUnchangeDirection()
    {
        GameObject gameObjectEnemy = GameObject.FindGameObjectWithTag("Enemy");
        Enemy scriptEnemy = gameObjectEnemy.GetComponent<Enemy>();
        gameObjectEnemy.transform.position = -Vector3.one;

        GameObject gameObjectEnemyTest = MonoBehaviour.Instantiate(gameObjectEnemy, -Vector3.one, Quaternion.identity);
        gameObjectEnemyTest.tag = "Ground";

        Vector2 fieldDirectionTest = (Vector2)TestAssistant.GetValueField(typeof(Enemy), "direction", scriptEnemy);
        float fieldSpeed = (float)TestAssistant.GetValueField(typeof(Enemy), "speed", scriptEnemy);
        Rigidbody2D fieldRigidbody2D = TestAssistant.GetValueField(typeof(Enemy), "rigidbody2d", scriptEnemy) as Rigidbody2D;


        yield return new WaitForFixedUpdate();
        Assert.AreEqual(fieldDirectionTest, (Vector2)TestAssistant.GetValueField(typeof(Enemy), "direction", scriptEnemy),
            "In the \"Enemy\" script in the \"OnCollisionEnter2D\" method, the \"Enemy\" object does change the \"direction\" field");

        Assert.AreEqual(fieldDirectionTest * fieldSpeed, new Vector2(fieldRigidbody2D.velocity.x, 0),
            "In the \"Enemy\" script in the \"OnCollisionEnter2D\" method, the \"Enemy\" object does change the \"rigidbody2d.velocity\" field");
    }


}
