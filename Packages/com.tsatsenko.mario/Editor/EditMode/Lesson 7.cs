using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using NUnit.Framework.Internal;
using UnityEditor;
using UnityEditor.Sprites;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.U2D;

public class Lesson7
{
    [Test]
    public void _ExistFiles()
    {
        var pathFile = Path.Combine("Assets", "Sprites", "Enemies.png");
        var exists = File.Exists(pathFile);
        Assert.IsTrue(exists,
            "The \"Enemies\" file is missing!");

        pathFile = Path.Combine("Assets", "Prefabs", "Enemy.prefab");
        exists = File.Exists(pathFile);
        Assert.IsTrue(exists,
            "The \"Enemy\" prefab is missing!");

        pathFile = Path.Combine("Assets", "Physics", "No Friction.physicsMaterial2D");
        exists = File.Exists(pathFile); 
        Assert.IsTrue(exists,
            "The \"No Friction\" physicsMaterial is missing!");

        pathFile = Path.Combine("Assets", "Scripts", "Enemy.cs");
        exists = File.Exists(pathFile);
        Assert.IsTrue(exists,
            "The \"Enemy\" script is missing!");
    }

    [Test]
    public void _Physics2D()
    {
        Assert.IsTrue(Physics2D.GetIgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Enemy")),
            "The \"Enemy\" layer and \"Enemy\" layer should be ignored");
    }

    [Test]
    public void CheckingPrefabEnemy()
    {
        var pathFile = Path.Combine("Assets", "Prefabs", "Enemy.prefab");
        GameObject objectEnemy = AssetDatabase.LoadAssetAtPath<GameObject>(pathFile);

        /***************************SpriteRenderer*************************/

        if (!objectEnemy.TryGetComponent(out SpriteRenderer spriteRenderer))
        {
            Assert.AreEqual(objectEnemy.AddComponent<SpriteRenderer>(), spriteRenderer,
                "The \"Enemy\" prefab does not have a SpriteRenderer component");
        }

        Assert.AreEqual("Enemies_0", spriteRenderer.sprite.name,
            "The \"Enemy\" prefab in the \"SpriteRenderer\" component has an incorrect \"Sprite\" field");

        /***************************BoxCollider2D*************************/

        if (!objectEnemy.TryGetComponent(out BoxCollider2D boxCollider2D))
        {
            Assert.AreEqual(objectEnemy.AddComponent<BoxCollider2D>(), boxCollider2D,
                "The \"Enemy\" prefab does not have a BoxCollider2D component");
        }

        /***************************Rigidbody2D*************************/

        if (!objectEnemy.TryGetComponent(out Rigidbody2D rigidbody2D))
        {
            Assert.AreEqual(objectEnemy.AddComponent<Rigidbody2D>(), rigidbody2D,
                "The \"Enemy\" prefab does not have a Rigidbody2D component");
        }

        pathFile = Path.Combine("Assets", "Physics", "No Friction.physicsMaterial2D");
        PhysicsMaterial2D physicsMaterial = AssetDatabase.LoadAssetAtPath<PhysicsMaterial2D>(pathFile);

        Assert.AreEqual(physicsMaterial, rigidbody2D.sharedMaterial,
            "The \"Enemy\" prefab in the \"Rigidbody2D\" component has an incorrect \"Material\" field");

        Assert.IsTrue(rigidbody2D.collisionDetectionMode.HasFlag(CollisionDetectionMode2D.Continuous),
            "The \"Enemy\" prefab in the \"Rigidbody2D\" component has an incorrect \"Collision Detection\" field");

        Assert.IsTrue(rigidbody2D.constraints.HasFlag(RigidbodyConstraints2D.FreezeRotation),
            "The \"Enemy\" prefab in the \"Rigidbody2D\" component has an incorrect \"Freeze Rotation\" field");

        /***************************Tag*************************/
        Assert.AreEqual("Enemy", objectEnemy.tag,
            "The \"Enemy\" prefab has the incorrect tag");

        /***************************Enemy*************************/

        if (!objectEnemy.TryGetComponent(out Enemy enemy))
        {
            Assert.AreEqual(objectEnemy.AddComponent<Enemy>(), enemy,
                "The \"Enemy\" object does not have a Enemy script");
        }
    }

    [Test]
    public void CheckingObjectEnemyOnScene()
    {
        GameObject gameObjectEnemy = GameObject.Find("Enemy");
        Assert.IsNotNull(gameObjectEnemy,
            "There is no \"Enemy\" object on the scene");

        /***************************SpriteRenderer*************************/

        if (!gameObjectEnemy.TryGetComponent(out SpriteRenderer spriteRenderer))
        {
            Assert.AreEqual(gameObjectEnemy.AddComponent<SpriteRenderer>(), spriteRenderer,
                "The \"Enemy\" object does not have a SpriteRenderer component");
        }

        Assert.AreEqual("Enemies_0", spriteRenderer.sprite.name,
            "The \"Enemy\" object in the \"SpriteRenderer\" component has an incorrect \"Sprite\" field");

        /***************************BoxCollider2D*************************/

        if (!gameObjectEnemy.TryGetComponent(out BoxCollider2D boxCollider2D))
        {
            Assert.AreEqual(gameObjectEnemy.AddComponent<BoxCollider2D>(), boxCollider2D,
                "The \"Enemy\" object does not have a BoxCollider2D component");
        }

        /***************************Rigidbody2D*************************/

        if (!gameObjectEnemy.TryGetComponent(out Rigidbody2D rigidbody2D))
        {
            Assert.AreEqual(gameObjectEnemy.AddComponent<Rigidbody2D>(), rigidbody2D,
                "The \"Enemy\" object does not have a Rigidbody2D component");
        }

        string pathFile = Path.Combine("Assets", "Physics", "No Friction.physicsMaterial2D");
        PhysicsMaterial2D physicsMaterial = AssetDatabase.LoadAssetAtPath<PhysicsMaterial2D>(pathFile);

        Assert.AreEqual(physicsMaterial, rigidbody2D.sharedMaterial,
            "The \"Enemy\" prefab in the \"Rigidbody2D\" component has an incorrect \"Material\" field");

        Assert.IsTrue(rigidbody2D.collisionDetectionMode.HasFlag(CollisionDetectionMode2D.Continuous),
            "The \"Enemy\" prefab in the \"Rigidbody2D\" component has an incorrect \"Collision Detection\" field");

        Assert.IsTrue(rigidbody2D.constraints.HasFlag(RigidbodyConstraints2D.FreezeRotation),
            "The \"Enemy\" prefab in the \"Rigidbody2D\" component has an incorrect \"Freeze Rotation\" field");

        /***************************Tag*************************/

        Assert.AreEqual("Enemy", gameObjectEnemy.tag,
            "The \"Enemy\" object has the incorrect tag");

        /***************************Enemy*************************/

        if (!gameObjectEnemy.TryGetComponent(out Enemy enemy))
        {
            Assert.AreEqual(gameObjectEnemy.AddComponent<Enemy>(), enemy,
                "The \"Enemy\" object does not have a Enemy script");
        }
    }

    [Test]
    public void CheckingScriptEnemy()
    {
        TestAssistant.TestingFields(typeof(Enemy), "speed", "Single", FieldAttributes.Private, true);
        TestAssistant.TestingFields(typeof(Enemy), "rigidbody2d", "Rigidbody2D", FieldAttributes.Private);
        TestAssistant.TestingFields(typeof(Enemy), "direction", "Vector2", FieldAttributes.Private);

        TestAssistant.TestingMethods(typeof(Enemy), "Awake", "Void", MethodAttributes.Private | MethodAttributes.HideBySig, new MyParameterInfo[] { });
        TestAssistant.TestingMethods(typeof(Enemy), "Start", "Void", MethodAttributes.Private | MethodAttributes.HideBySig, new MyParameterInfo[] { });
        TestAssistant.TestingMethods(typeof(Enemy), "OnCollisionEnter2D", "Void", MethodAttributes.Private | MethodAttributes.HideBySig, new MyParameterInfo[] { new MyParameterInfo(typeof(Collision2D), "collision") });

        var pathFile = Path.Combine("Assets", "Prefabs", "Enemy.prefab");
        GameObject objectEnemy = AssetDatabase.LoadAssetAtPath<GameObject>(pathFile);
        Enemy scriptEnemy = objectEnemy.GetComponent<Enemy>();

        TestAssistant.TestingFieldValue(typeof(Enemy), "speed", scriptEnemy, 3f);
        TestAssistant.TestingFieldValue(typeof(Enemy), "direction", scriptEnemy, Vector2.left);
        TestAssistant.TestingFieldValue(typeof(Enemy), "rigidbody2d", scriptEnemy, null);
    }
}
