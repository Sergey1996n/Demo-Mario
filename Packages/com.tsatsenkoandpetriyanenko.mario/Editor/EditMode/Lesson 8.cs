using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

public class Lesson8
{
    private string pathScene = Path.Combine("Assets", "Scenes", "Level.unity");

    [Test]
    public void __ExistingDirectoriesAndFiles()
    {
        var pathFile = Path.Combine("Assets", "Sprites", "Star.png");
        var exists = File.Exists(pathFile);
        Assert.IsTrue(exists,
            "The \"{0}\" directory does not have the \"{1}\" sprite", new object[] { "Sprites", "Star" });

        pathFile = Path.Combine("Assets", "Prefabs", "Star.prefab");
        exists = File.Exists(pathFile);
        Assert.IsTrue(exists,
            "The \"{0}\" directory does not have the \"{1}\" prefab", new object[] { "Prefabs", "Star" });

        pathFile = Path.Combine("Assets", "Scripts", "Starpower.cs");
        exists = File.Exists(pathFile);
        Assert.IsTrue(exists,
            "The \"{0}\" directory does not have the \"{1}\" script", new object[] { "Scripts", "Starpower" });
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
    }

    [Test]
    public void _1CheckingSpriteStar()
    {
        var pathFile = Path.Combine("Assets", "Sprites", "Star.png");
        TextureImporter importer = TextureImporter.GetAtPath(pathFile) as TextureImporter;
        importer.name = "Star";

        Assert.AreEqual(TextureImporterType.Sprite, importer.textureType,
            "The \"{0}\" sprite has an incorrect \"{1}\" field", new object[] { importer.name, "Texture Type" });

        Assert.AreEqual(SpriteImportMode.Single, importer.spriteImportMode,
            "The \"{0}\" sprite has an incorrect \"{1}\" field", new object[] { importer.name, "Sprite Import Mode" });

        Assert.AreEqual(16f, importer.spritePixelsPerUnit,
            "The \"{0}\" sprite has an incorrect \"{1}\" field", new object[] { importer.name, "Pixels Per Unit" });

        Assert.AreEqual(FilterMode.Point, importer.filterMode,
            "The \"{0}\" sprite has an incorrect \"{1}\" field", new object[] { importer.name, "Filter Mode" });
    }

    [Test]
    public void _2CheckingPrefabStar()
    {
        var pathFile = Path.Combine("Assets", "Prefabs", "Star.prefab");
        GameObject objectStar = AssetDatabase.LoadAssetAtPath<GameObject>(pathFile);

        /***************************Transform*************************/

        string nameComponent = "Transform";

        if (!objectStar.TryGetComponent(out Transform transform))
        {
            Assert.AreEqual(objectStar.AddComponent<Transform>(), transform,
                "The \"{0}\" prefab does not have a \"{1}\" component", new object[] { objectStar.name, nameComponent });
        }

        Assert.AreEqual(Vector3.zero, transform.position,
            "The \"{0}\" prefab in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { objectStar.name, nameComponent, "Position" });

        Assert.AreEqual(Vector3.zero, transform.rotation.eulerAngles,
            "The \"{0}\" prefab in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { objectStar.name, nameComponent, "Rotation" });

        Assert.AreEqual(Vector3.one, transform.localScale,
            "The \"{0}\" prefab in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { objectStar.name, nameComponent, "Scale" });

        /***************************SpriteRenderer*************************/

        nameComponent = "Sprite Renderer";

        if (!objectStar.TryGetComponent(out SpriteRenderer spriteRenderer))
        {
            Assert.AreEqual(objectStar.AddComponent<SpriteRenderer>(), spriteRenderer,
                "The \"{0}\" prefab does not have a \"{1}\" component", new object[] { objectStar.name, nameComponent });
        }

        pathFile = Path.Combine("Assets", "Sprites", "Star.png");
        Sprite spriteStar = AssetDatabase.LoadAssetAtPath<Sprite>(pathFile);

        Assert.AreEqual(spriteStar, spriteRenderer.sprite,
            "The \"{0}\" prefab in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { objectStar.name, nameComponent, "Sprite" });

        /***************************BoxCollider2D*************************/

        nameComponent = "Box Collider 2D";

        if (!objectStar.TryGetComponent(out BoxCollider2D boxCollider2D))
        {
            Assert.AreEqual(objectStar.AddComponent<BoxCollider2D>(), boxCollider2D,
                "The \"{0}\" prefab does not have a \"{1}\" component", new object[] { objectStar.name, nameComponent });
        }

        Assert.IsTrue(boxCollider2D.isTrigger,
            "The \"{0}\" prefab in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { objectStar.name, nameComponent, "Is Trigger" });

        /***************************Starpower*************************/

        nameComponent = "Starpower";

        if (!objectStar.TryGetComponent(out Starpower starpower))
        {
            Assert.AreEqual(objectStar.AddComponent<Starpower>(), starpower,
                "The \"{0}\" prefab does not have a \"{1}\" script", new object[] { objectStar.name, nameComponent });
        }

        /***************************Layer*************************/

        Assert.AreEqual("Powerups", LayerMask.LayerToName(objectStar.layer),
            "The \"{0}\" prefab has an incorrect {1}", new object[] { objectStar.name , "layer" });
    }

    [Test]
    public void _3CheckingObjectStarOnScene()
    {
        GameObject gameObjectStar = GameObject.Find("Star");
        Assert.IsNotNull(gameObjectStar,
            "There is no \"{0}\" object on the scene", new object[] { "Star" });

        /***************************SpriteRenderer*************************/

        string nameComponent = "Sprite Renderer";

        if (!gameObjectStar.TryGetComponent(out SpriteRenderer spriteRenderer))
        {
            Assert.AreEqual(gameObjectStar.AddComponent<SpriteRenderer>(), spriteRenderer,
                "The \"{0}\" object does not have a \"{1}\" component", new object[] { gameObjectStar.name, nameComponent });
        }

        var pathFile = Path.Combine("Assets", "Sprites", "Star.png");
        Sprite spriteStar = AssetDatabase.LoadAssetAtPath<Sprite>(pathFile);

        Assert.AreEqual(spriteStar, spriteRenderer.sprite,
            "The \"{0}\" object in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { gameObjectStar.name, nameComponent, "Sprite" });

        /***************************BoxCollider2D*************************/

        nameComponent = "Box Collider 2D";

        if (!gameObjectStar.TryGetComponent(out BoxCollider2D boxCollider2D))
        {
            Assert.AreEqual(gameObjectStar.AddComponent<BoxCollider2D>(), boxCollider2D,
                "The \"{0}\" object does not have a \"{1}\" component", new object[] { gameObjectStar.name, nameComponent });
        }

        Assert.IsTrue(boxCollider2D.isTrigger,
            "The \"{0}\" object in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { gameObjectStar.name, nameComponent, "Is Trigger" });

        /***************************Enemy*************************/

        nameComponent = "Box Collider 2D";

        if (!gameObjectStar.TryGetComponent(out Starpower starpower))
        {
            Assert.AreEqual(gameObjectStar.AddComponent<Starpower>(), starpower,
                "The \"{0}\" object does not have a \"{1}\" script", new object[] { gameObjectStar.name, nameComponent });
        }

        /***************************Layer*************************/

        Assert.AreEqual("Powerups", LayerMask.LayerToName(gameObjectStar.layer),
            "The \"{0}\" object has an incorrect {1}", new object[] { gameObjectStar.name, "layer" });
    }

    [Test]
    public void _4CheckingPhysics2D()
    {
        Assert.IsTrue(Physics2D.GetIgnoreLayerCollision(LayerMask.NameToLayer("Powerups"), LayerMask.NameToLayer("Powerups")),
            "The \"Powerups\" layer and \"Powerups\" layer should be ignored");

        Assert.IsTrue(Physics2D.GetIgnoreLayerCollision(LayerMask.NameToLayer("Powerups"), LayerMask.NameToLayer("Enemy")),
            "The \"Powerups\" layer and \"Enemy\" layer should be ignored");
    }

    [Test]
    public void _5CheckingScriptStarpower()
    {
        TestAssistant.TestingMethod(typeof(Starpower), "OnTriggerEnter2D", typeof(void), MethodAttributes.Private, new MyParameterInfo[] { new MyParameterInfo(typeof(Collider2D), "collision") });
    }

    [Test]
    public void _6CheckingScriptPlayer()
    {
        Type type = typeof(Player);
        TestAssistant.TestingField(type, "speedCoefficient", typeof(float), FieldAttributes.Private, true);
        TestAssistant.TestingProperty(type, "StarPower", typeof(bool), attributesSet: MethodAttributes.Private);

        TestAssistant.TestingMethod(type, "StarPowerActive", typeof(void), MethodAttributes.Public, new MyParameterInfo[] { new MyParameterInfo(typeof(float), "duration", 5f) });
        TestAssistant.TestingMethod(type, "StarPowerAnimation", typeof(IEnumerator), MethodAttributes.Private, new MyParameterInfo[] { new MyParameterInfo(typeof(float), "duration") });

        GameObject gameObjectEnemy = GameObject.Find("Player");
        Player scriptPlayer = gameObjectEnemy.GetComponent<Player>();

        TestAssistant.TestingFieldValue(typeof(Player), "speedCoefficient", scriptPlayer, 1.2f);
        TestAssistant.TestingPropertyValue(typeof(Player), "StarPower", scriptPlayer, false);
    }
}
