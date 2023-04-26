using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using NUnit.Framework.Internal;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Lesson7
{
    private string pathScene = Path.Combine("Assets", "Scenes", "Level.unity");

    [Test]
    public void __ExistingDirectoriesAndFiles()
    {
        var pathFile = Path.Combine("Assets", "Sprites", "Enemies.png");
        var exists = File.Exists(pathFile);
        Assert.IsTrue(exists,
            "The \"{0}\" directory does not have the \"{1}\" sprite", new object[] { "Sprites", "Enemies" });

        pathFile = Path.Combine("Assets", "Prefabs", "Enemy.prefab");
        exists = File.Exists(pathFile);
        Assert.IsTrue(exists,
            "The \"{0}\" directory does not have the \"{1}\" prefab", new object[] { "Prefabs", "Enemy" });

        pathFile = Path.Combine("Assets", "Physics", "No Friction.physicsMaterial2D");
        exists = File.Exists(pathFile); 
        Assert.IsTrue(exists,
            "The \"{0}\" directory does not have the \"{1}\" material", new object[] { "Physics", "No Friction" });

        pathFile = Path.Combine("Assets", "Scripts", "Enemy.cs");
        exists = File.Exists(pathFile);
        Assert.IsTrue(exists,
            "The \"{0}\" directory does not have the \"{1}\" script", new object[] { "Scripts", "Enemy" });
    }

    [Test]
    public void _1CheckingSpriteEnemies()
    {
        var pathFile = Path.Combine("Assets", "Sprites", "Enemies.png");
        TextureImporter importer = TextureImporter.GetAtPath(pathFile) as TextureImporter;
        importer.name = "Enemies";

        Assert.AreEqual(TextureImporterType.Sprite, importer.textureType,
            "The \"{0}\" sprite has an incorrect \"{1}\" field", new object[] { importer.name, "Texture Type" });

        Assert.AreEqual(SpriteImportMode.Multiple, importer.spriteImportMode,
            "The \"{0}\" sprite has an incorrect \"{1}\" field", new object[] { importer.name, "Sprite Import Mode" });

        Assert.AreEqual(16f, importer.spritePixelsPerUnit,
            "The \"{0}\" sprite has an incorrect \"{1}\" field", new object[] { importer.name, "Pixels Per Unit" });

        Assert.AreEqual(FilterMode.Point, importer.filterMode,
            "The \"{0}\" sprite has an incorrect \"{1}\" field", new object[] { importer.name, "Filter Mode" });

        Assert.AreEqual(125, importer.spritesheet.Length,
            "Incorrectly sliced \"{0}\" sprite (incorrect number of spritesheet)", new object[] { importer.name });
    }

    [Test]
    public void _2CheckingPrefabEnemy()
    {
        var pathFile = Path.Combine("Assets", "Prefabs", "Enemy.prefab");
        GameObject objectEnemy = AssetDatabase.LoadAssetAtPath<GameObject>(pathFile);

        /***************************Transform*************************/

        string nameComponent = "Transform";

        if (!objectEnemy.TryGetComponent(out Transform transform))
        {
            Assert.AreEqual(objectEnemy.AddComponent<Transform>(), transform,
                "The \"{0}\" prefab does not have a \"{1}\" component", new object[] { objectEnemy.name, nameComponent });
        }

        Assert.AreEqual(Vector3.zero, transform.position,
            "The \"{0}\" prefab in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { objectEnemy.name, nameComponent, "Position" });

        Assert.AreEqual(Vector3.zero, transform.rotation.eulerAngles,
            "The \"{0}\" prefab in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { objectEnemy.name, nameComponent, "Rotation" });

        Assert.AreEqual(Vector3.one, transform.localScale,
            "The \"{0}\" prefab in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { objectEnemy.name, nameComponent, "Scale" });

        /***************************SpriteRenderer*************************/

        nameComponent = "Sprite Renderer";

        if (!objectEnemy.TryGetComponent(out SpriteRenderer spriteRenderer))
        {
            Assert.AreEqual(objectEnemy.AddComponent<SpriteRenderer>(), spriteRenderer,
                "The \"{0}\" prefab does not have a \"{1}\" component", new object[] { objectEnemy.name, nameComponent });
        }

        pathFile = Path.Combine("Assets", "Sprites", "Enemies.png");
        UnityEngine.Object[] spritesEnemies = AssetDatabase.LoadAllAssetRepresentationsAtPath(pathFile);

        Assert.AreEqual(spritesEnemies[0], spriteRenderer.sprite,
            "The \"{0}\" prefab in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { objectEnemy.name, nameComponent, "Sprite" });

        /***************************BoxCollider2D*************************/

        nameComponent = "Box Collider 2D";

        if (!objectEnemy.TryGetComponent(out BoxCollider2D boxCollider2D))
        {
            Assert.AreEqual(objectEnemy.AddComponent<BoxCollider2D>(), boxCollider2D,
                "The \"{0}\" prefab does not have a \"{1}\" component", new object[] { objectEnemy.name, nameComponent });
        }

        /***************************Rigidbody2D*************************/

        nameComponent = "Rigidbody 2D";

        if (!objectEnemy.TryGetComponent(out Rigidbody2D rigidbody2D))
        {
            Assert.AreEqual(objectEnemy.AddComponent<Rigidbody2D>(), rigidbody2D,
                "The \"{0}\" prefab does not have a \"{1}\" component", new object[] { objectEnemy.name, nameComponent });
        }

        pathFile = Path.Combine("Assets", "Physics", "No Friction.physicsMaterial2D");
        PhysicsMaterial2D physicsMaterial = AssetDatabase.LoadAssetAtPath<PhysicsMaterial2D>(pathFile);

        Assert.AreEqual(physicsMaterial, rigidbody2D.sharedMaterial,
            "The \"{0}\" prefab in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { objectEnemy.name, nameComponent, "Material" });

        Assert.AreEqual(CollisionDetectionMode2D.Continuous, rigidbody2D.collisionDetectionMode,
            "The \"{0}\" prefab in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { objectEnemy.name, nameComponent, "Collision Detection" });

        Assert.IsTrue(rigidbody2D.constraints.HasFlag(RigidbodyConstraints2D.FreezeRotation),
            "The \"{0}\" prefab in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { objectEnemy.name, nameComponent, "Freeze Rotation" });

        /***************************Enemy*************************/

        nameComponent = "Enemy";

        if (!objectEnemy.TryGetComponent(out Enemy enemy))
        {
            Assert.AreEqual(objectEnemy.AddComponent<Enemy>(), enemy,
                "The \"{0}\" prefab does not have a \"{1}\" script", new object[] { objectEnemy.name, nameComponent });
        }

        /***************************LayerAndField*************************/

        Assert.AreEqual("Enemy", objectEnemy.tag,
            "The \"{0}\" prefab has an incorrect {1}", new object[] { objectEnemy.name }, "tag");

        Assert.AreEqual("Enemy", LayerMask.LayerToName(objectEnemy.layer),
            "The \"{0}\" prefab has an incorrect {1}", new object[] { objectEnemy.name }, "layer");

    }

    [Test]
    public void _3CheckingObjectEnemyOnScene()
    {
        GameObject gameObjectEnemy = GameObject.Find("Enemy");
        Assert.IsNotNull(gameObjectEnemy,
            "There is no \"{0}\" object on the scene", new object[] { "Enemy" });

        /***************************SpriteRenderer*************************/

        string nameComponent = "Sprite Renderer";

        if (!gameObjectEnemy.TryGetComponent(out SpriteRenderer spriteRenderer))
        {
            Assert.AreEqual(gameObjectEnemy.AddComponent<SpriteRenderer>(), spriteRenderer,
                "The \"{0}\" object does not have a \"{1}\" component", new object[] { gameObjectEnemy.name, nameComponent });
        }

        string pathFile = Path.Combine("Assets", "Sprites", "Enemies.png");
        UnityEngine.Object[] spritesEnemies = AssetDatabase.LoadAllAssetRepresentationsAtPath(pathFile);

        Assert.AreEqual(spritesEnemies[0], spriteRenderer.sprite,
            "The \"{0}\" object in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { gameObjectEnemy.name, nameComponent, "Sprite" });

        /***************************BoxCollider2D*************************/

        nameComponent = "Box Collider 2D";

        if (!gameObjectEnemy.TryGetComponent(out BoxCollider2D boxCollider2D))
        {
            Assert.AreEqual(gameObjectEnemy.AddComponent<BoxCollider2D>(), boxCollider2D,
                "The \"{0}\" object does not have a \"{1}\" component", new object[] { gameObjectEnemy.name, nameComponent });
        }

        /***************************Rigidbody2D*************************/

        nameComponent = "Rigidbody 2D";

        if (!gameObjectEnemy.TryGetComponent(out Rigidbody2D rigidbody2D))
        {
            Assert.AreEqual(gameObjectEnemy.AddComponent<Rigidbody2D>(), rigidbody2D,
                "The \"{0}\" object does not have a \"{1}\" component", new object[] { gameObjectEnemy.name, nameComponent });
        }

        pathFile = Path.Combine("Assets", "Physics", "No Friction.physicsMaterial2D");
        PhysicsMaterial2D physicsMaterial = AssetDatabase.LoadAssetAtPath<PhysicsMaterial2D>(pathFile);

        Assert.AreEqual(physicsMaterial, rigidbody2D.sharedMaterial,
            "The \"{0}\" object in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { gameObjectEnemy.name, nameComponent, "Material" });

        Assert.AreEqual(CollisionDetectionMode2D.Continuous, rigidbody2D.collisionDetectionMode,
            "The \"{0}\" object in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { gameObjectEnemy.name, nameComponent, "Collision Detection" });

        Assert.IsTrue(rigidbody2D.constraints.HasFlag(RigidbodyConstraints2D.FreezeRotation),
            "The \"{0}\" object in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { gameObjectEnemy.name, nameComponent, "Freeze Rotation" });

        /***************************Enemy*************************/

        nameComponent = "Enemy";

        if (!gameObjectEnemy.TryGetComponent(out Enemy enemy))
        {
            Assert.AreEqual(gameObjectEnemy.AddComponent<Enemy>(), enemy,
                "The \"{0}\" object does not have a \"{1}\" script", new object[] { gameObjectEnemy.name, nameComponent });
        }

        /***************************LayerAndField*************************/

        Assert.AreEqual("Enemy", gameObjectEnemy.tag,
            "The \"{0}\" object has an incorrect {1}", new object[] { gameObjectEnemy.name }, "tag");

        Assert.AreEqual("Enemy", LayerMask.LayerToName(gameObjectEnemy.layer),
            "The \"{0}\" object has an incorrect {1}", new object[] { gameObjectEnemy.name }, "layer");
    }

    [Test]
    public void _4CheckingPhysics2D()
    {
        Assert.IsTrue(Physics2D.GetIgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Enemy")),
            "The \"Enemy\" layer and \"Enemy\" layer should be ignored");
    }

    [Test]
    public void _5CheckingScriptEnemy()
    {
        Type type = typeof(Enemy);
        TestAssistant.TestingField(type, "speed", typeof(float), FieldAttributes.Private, true);
        TestAssistant.TestingField(type, "rigidbody2d", typeof(Rigidbody2D), FieldAttributes.Private);
        TestAssistant.TestingField(type, "direction", typeof(Vector2), FieldAttributes.Private);

        TestAssistant.TestingMethod(type, "Awake", typeof(void), MethodAttributes.Private);
        TestAssistant.TestingMethod(type, "Start", typeof(void), MethodAttributes.Private);
        TestAssistant.TestingMethod(type, "OnCollisionEnter2D", typeof(void), MethodAttributes.Private, new MyParameterInfo[] { new MyParameterInfo(typeof(Collision2D), "collision") });

        GameObject gameObjectEnemy = GameObject.Find("Enemy");
        Enemy scriptEnemy = gameObjectEnemy.GetComponent<Enemy>();

        TestAssistant.TestingFieldValue(typeof(Enemy), "speed", scriptEnemy, 3f);
        TestAssistant.TestingFieldValue(typeof(Enemy), "direction", scriptEnemy, Vector2.left);
        TestAssistant.TestingFieldValue(typeof(Enemy), "rigidbody2d", scriptEnemy, null);
    }

    [Test]
    public void _6InitializingVariablesScriptEnemy()
    {
        GameObject gameObjectEnemy = GameObject.Find("Enemy");
        Enemy scriptEnemy = gameObjectEnemy.GetComponent<Enemy>();

        Type type = typeof(Enemy);
        var methodAwake = TestAssistant.GetMethod(type, "Awake");
        methodAwake.Invoke(scriptEnemy, null);

        Rigidbody2D fieldRigidbody2D = TestAssistant.GetValueField(type, "rigidbody2d", scriptEnemy) as Rigidbody2D;

        Assert.AreEqual(gameObjectEnemy.GetComponent<Rigidbody2D>(), fieldRigidbody2D,
            "The \"{0}\" method does not work correctly in the \"{1}\" class (there is no reference to the component in the \"{2}\" field)", new object[] { methodAwake.Name, scriptEnemy.name, "rigidbody2d" });

        methodAwake = TestAssistant.GetMethod(type, "Start");
        methodAwake.Invoke(scriptEnemy, null);

        Vector2 fieldDirection = (Vector2)TestAssistant.GetValueField(type, "direction", scriptEnemy);
        float fieldSpeed = (float)TestAssistant.GetValueField(type, "speed", scriptEnemy);

        Assert.AreEqual(fieldDirection * fieldSpeed, fieldRigidbody2D.velocity,
            "The \"{0}\" method does not work correctly in the \"{1}\" class (incorrect value in the \"{2}\" field)", new object[] { methodAwake.Name, scriptEnemy.name, "rigidbody2d.velocity" });

        EditorSceneManager.OpenScene(pathScene);
    }
}
