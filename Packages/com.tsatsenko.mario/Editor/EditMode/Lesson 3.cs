using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using NUnit.Framework.Internal;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Tilemaps;

/* Перенести тесты Tilemap */
public class Lesson3
{
    [Test]
    public void __ExistingDirectoriesAndFiles()
    {
        var pathDirectory = Path.Combine("Assets", "Sprites");
        var exists = Directory.Exists(pathDirectory);
        Assert.IsTrue(exists,
            "The \"{0}\" directory does not have the \"{1}\" directory", new object[] { "Assets", "Sprites" });

        var pathFile = Path.Combine("Assets", "Sprites", "Player.png");
        exists = File.Exists(pathFile);
        Assert.IsTrue(exists,
            "The \"{0}\" directory does not have the \"{1}\" sprite", new object[] { "Sprites", "Player" });

        pathDirectory = Path.Combine("Assets", "Physics");
        exists = Directory.Exists(pathDirectory);
        Assert.IsTrue(exists,
            "The \"{0}\" directory does not have the \"{1}\" directory", new object[] { "Assets", "Physics" });

        pathFile = Path.Combine("Assets", "Physics", "No Friction.physicsMaterial2D");
        exists = File.Exists(pathFile);
        Assert.IsTrue(exists,
            "The \"{0}\" directory does not have the \"{1}\" physics material", new object[] { "Physics", "No Friction" });

        pathDirectory = Path.Combine("Assets", "Scripts");
        exists = Directory.Exists(pathDirectory);
        Assert.IsTrue(exists,
            "The \"{0}\" directory does not have the \"{1}\" directory", new object[] { "Assets", "Scripts" });

        pathFile = Path.Combine("Assets", "Scripts", "Player.cs");
        exists = File.Exists(pathFile);
        Assert.IsTrue(exists,
            "The \"{0}\" directory does not have the \"{1}\" script", new object[] { "Scripts", "Player" });
    }

    [Test]
    public void _1CheckingSpritePlayer()
    {
        var pathFile = Path.Combine("Assets", "Sprites", "Player.png");
        TextureImporter importer = TextureImporter.GetAtPath(pathFile) as TextureImporter;
        importer.name = "Player";

        Assert.AreEqual(TextureImporterType.Sprite, importer.textureType,
            "The \"{0}\" sprite has an incorrect \"{1}\" field", new object[] { importer.name, "Texture Type" });

        Assert.AreEqual(SpriteImportMode.Multiple, importer.spriteImportMode,
            "The \"{0}\" sprite has an incorrect \"{1}\" field", new object[] { importer.name, "Sprite Import Mode" });

        Assert.AreEqual(16f, importer.spritePixelsPerUnit,
            "The \"{0}\" sprite has an incorrect \"{1}\" field", new object[] { importer.name, "Pixels Per Unit" });

        Assert.AreEqual(FilterMode.Point, importer.filterMode,
            "The \"{0}\" sprite has an incorrect \"{1}\" field", new object[] { importer.name, "Filter Mode" });

        Assert.AreEqual(88, importer.spritesheet.Length,
            "Incorrectly sliced \"{0}\" sprite (incorrect number of spritesheet)", new object[] { importer.name });
    }

    [Test]
    public void _2CheckingObjectStarOnScene()
    {
        GameObject gameObjectPlayer = GameObject.Find("Player");

        Assert.IsNotNull(gameObjectPlayer,
            "There is no \"{0}\" object on the scene", new object[] { "Player" });

        /***************************SpriteRenderer*************************/

        string nameComponent = "Sprite Renderer";

        if (!gameObjectPlayer.TryGetComponent(out SpriteRenderer spriteRenderer))
        {
            Assert.AreEqual(gameObjectPlayer.AddComponent<SpriteRenderer>(), spriteRenderer,
                "The \"{0}\" object does not have a \"{1}\" component", new object[] { gameObjectPlayer.name, nameComponent });
        }

        var pathFile = Path.Combine("Assets", "Sprites", "Player.png");
        Object[] spritesPlayer = AssetDatabase.LoadAllAssetRepresentationsAtPath(pathFile);

        Assert.AreEqual(spritesPlayer[5], spriteRenderer.sprite,
            "The \"{0}\" object in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { gameObjectPlayer.name, nameComponent, "Sprite" });

        /***************************BoxCollider2D*************************/

        nameComponent = "Box Collider 2D";

        if (!gameObjectPlayer.TryGetComponent(out BoxCollider2D boxCollider2D))
        {
            Assert.AreEqual(gameObjectPlayer.AddComponent<BoxCollider2D>(), boxCollider2D,
                "The \"{0}\" object does not have a \"{1}\" component", new object[] { gameObjectPlayer.name, nameComponent });
        }

        /***************************Rigidbody2D*************************/

        nameComponent = "Rigidbody 2D";

        if (!gameObjectPlayer.TryGetComponent(out Rigidbody2D rigidbody2D))
        {
            Assert.AreEqual(gameObjectPlayer.AddComponent<Rigidbody2D>(), rigidbody2D,
                "The \"{0}\" object does not have a \"{1}\" component", new object[] { gameObjectPlayer.name, nameComponent });
        }

        pathFile = Path.Combine("Assets", "Physics", "No Friction.physicsMaterial2D");
        PhysicsMaterial2D physicsMaterial = AssetDatabase.LoadAssetAtPath<PhysicsMaterial2D>(pathFile);

        Assert.AreEqual(physicsMaterial, rigidbody2D.sharedMaterial,
            "The \"{0}\" object in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { gameObjectPlayer.name, nameComponent, "Material" });

        Assert.AreEqual(CollisionDetectionMode2D.Continuous, rigidbody2D.collisionDetectionMode,
            "The \"{0}\" object in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { gameObjectPlayer.name, nameComponent, "Collision Detection" });

        Assert.AreEqual(RigidbodyInterpolation2D.Interpolate, rigidbody2D.interpolation,
            "The \"{0}\" object in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { gameObjectPlayer.name, nameComponent, "Interpolate" });

        Assert.IsTrue(rigidbody2D.constraints.HasFlag(RigidbodyConstraints2D.FreezeRotation),
            "The \"{0}\" object in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { gameObjectPlayer.name, nameComponent, "Freeze Rotation" });

        /***************************Player*************************/

        nameComponent = "Player";

        if (!gameObjectPlayer.TryGetComponent(out Player player))
        {
            Assert.AreEqual(gameObjectPlayer.AddComponent<Player>(), player,
                "The \"{0}\" object does not have a \"{1}\" component", new object[] { gameObjectPlayer.name, nameComponent });
        }

        /***************************LayerAndField*************************/

        Assert.AreEqual("Player", gameObjectPlayer.tag,
            "The \"{0}\" object has an incorrect {1}", new object[] { gameObjectPlayer.name }, "tag");

        Assert.AreEqual("Player", LayerMask.LayerToName(gameObjectPlayer.layer),
            "The \"{0}\" object has an incorrect {1}", new object[] { gameObjectPlayer.name }, "layer");
    }

    [Test]
    public void _3CheckingScriptPlayer()
    {
        TestAssistant.TestingField(typeof(Player), "speed", typeof(float), FieldAttributes.Private, true);
        TestAssistant.TestingField(typeof(Player), "jumpForce", typeof(float), FieldAttributes.Private, true);
        TestAssistant.TestingField(typeof(Player), "velocity", typeof(Vector2), FieldAttributes.Private);
        TestAssistant.TestingField(typeof(Player), "rigidbody2d", typeof(Rigidbody2D), FieldAttributes.Private);
        TestAssistant.TestingField(typeof(Player), "isGrounded", typeof(bool), FieldAttributes.Private);

        TestAssistant.TestingMethod(typeof(Player), "Awake", typeof(void), MethodAttributes.Private, new MyParameterInfo[] { });
        TestAssistant.TestingMethod(typeof(Player), "FixedUpdate", typeof(void), MethodAttributes.Private, new MyParameterInfo[] { });
        TestAssistant.TestingMethod(typeof(Player), "Update", typeof(void), MethodAttributes.Private, new MyParameterInfo[] { });
        TestAssistant.TestingMethod(typeof(Player), "Jump", typeof(void), MethodAttributes.Private, new MyParameterInfo[] { });
        TestAssistant.TestingMethod(typeof(Player), "OnCollisionEnter2D", typeof(void), MethodAttributes.Private, new MyParameterInfo[] { new MyParameterInfo(typeof(Collision2D), "collision") });
        TestAssistant.TestingMethod(typeof(Player), "OnCollisionExit2D", typeof(void), MethodAttributes.Private, new MyParameterInfo[] { new MyParameterInfo(typeof(Collision2D), "collision") });

        GameObject gameObjectPlayer = GameObject.FindGameObjectWithTag("Player");
        Player scriptPlayer = gameObjectPlayer.GetComponent<Player>();

        TestAssistant.TestingFieldValue(typeof(Player), "speed", scriptPlayer, 7f);
        TestAssistant.TestingFieldValue(typeof(Player), "jumpForce", scriptPlayer, 8f);
        TestAssistant.TestingFieldValue(typeof(Player), "velocity", scriptPlayer, Vector2.zero);
        TestAssistant.TestingFieldValue(typeof(Player), "rigidbody2d", scriptPlayer, null);
        TestAssistant.TestingFieldValue(typeof(Player), "isGrounded", scriptPlayer, false);
    }

    [Test]
    public void _4InitializingVariablesScriptPlayer()
    {
        GameObject gameObjectPlayer = GameObject.FindGameObjectWithTag("Player");
        Player scriptPlayer = gameObjectPlayer.GetComponent<Player>();

        var methodAwake = TestAssistant.GetMethod(typeof(Player), "Awake");
        methodAwake.Invoke(scriptPlayer, null);

        Rigidbody2D fieldRigidbody2D = TestAssistant.GetValueField(typeof(Player), "rigidbody2d", scriptPlayer) as Rigidbody2D;

        Assert.AreEqual(gameObjectPlayer.GetComponent<Rigidbody2D>(), fieldRigidbody2D,
            "The \"{0}\" method does not work correctly in the \"{1}\" class (there is no reference to the component in the \"{2}\" field)", new object[] { methodAwake.Name, scriptPlayer.name, "rigidbody2d" });

        EditorSceneManager.OpenScene("Assets/Scenes/SampleScene.unity");

    }

    //[Test]
    //public void ___InitializingVariablesScriptPlayer()
    //{
    //    EditorSceneManager.OpenScene("Assets/Scenes/SampleScene.unity");
    //    GameObject gameObjectPlayer = GameObject.FindGameObjectWithTag("Player");
    //    Player scriptPlayer = gameObjectPlayer.GetComponent<Player>();

    //    Debug.Log(TestAssistant.GetValueField(typeof(Player), "rigidbody2d", scriptPlayer));
    //    var methodAwake = TestAssistant.GetMethod(typeof(Player), "Awake");
    //    methodAwake.Invoke(scriptPlayer, null);
    //    Debug.Log(TestAssistant.GetValueField(typeof(Player), "rigidbody2d", scriptPlayer));
    //}

    //[Test]
    //public void ____InitializingVariablesScriptPlayer()
    //{
    //    EditorSceneManager.OpenScene("Assets/Scenes/SampleScene.unity");
    //    GameObject gameObjectPlayer = GameObject.FindGameObjectWithTag("Player");
    //    Player scriptPlayer = gameObjectPlayer.GetComponent<Player>();

    //    Debug.Log(TestAssistant.GetValueField(typeof(Player), "rigidbody2d", scriptPlayer));
    //    var methodAwake = TestAssistant.GetMethod(typeof(Player), "Awake");
    //    methodAwake.Invoke(scriptPlayer, null);
    //    Debug.Log(TestAssistant.GetValueField(typeof(Player), "rigidbody2d", scriptPlayer));
    //}
}

