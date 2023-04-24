using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

public class Lesson8
{
    [Test]
    public void _ExistFiles()
    {
        var pathFile = Path.Combine("Assets", "Sprites", "Star.png");
        var exists = File.Exists(pathFile);
        Assert.IsTrue(exists,
            "The \"Star\" file is missing!");

        pathFile = Path.Combine("Assets", "Prefabs", "Star.prefab");
        exists = File.Exists(pathFile);
        Assert.IsTrue(exists,
            "The \"Star\" prefab is missing!");

        pathFile = Path.Combine("Assets", "Scripts", "Starpower.cs");
        exists = File.Exists(pathFile);
        Assert.IsTrue(exists,
            "The \"Starpower\" script is missing!");
    }

    [Test]
    public void _Physics2D()
    {
        Assert.IsTrue(Physics2D.GetIgnoreLayerCollision(LayerMask.NameToLayer("Powerups"), LayerMask.NameToLayer("Powerups")),
            "The \"Powerups\" layer and \"Powerups\" layer should be ignored");

        Assert.IsTrue(Physics2D.GetIgnoreLayerCollision(LayerMask.NameToLayer("Powerups"), LayerMask.NameToLayer("Enemy")),
            "The \"Powerups\" layer and \"Enemy\" layer should be ignored");
    }

    [Test]
    public void CheckingSpriteStar()
    {
        var pathFile = Path.Combine("Assets", "Sprites", "Star.png");
        TextureImporter importer = TextureImporter.GetAtPath(pathFile) as TextureImporter;

        Assert.AreEqual(SpriteImportMode.Single, importer.spriteImportMode,
            "The \"Star\" sprite has an incorrect \"Sprite Import Mode\" field");

        Assert.AreEqual(16f, importer.spritePixelsPerUnit,
            "The \"Star\" sprite has an incorrect \"Pixels Per Unit\" field");

        Assert.AreEqual(FilterMode.Point, importer.filterMode,
            "The \"Star\" sprite has an incorrect \"Filter Mode\" field");
    }

    [Test]
    public void CheckingPrefabStar()
    {
        var pathFile = Path.Combine("Assets", "Prefabs", "Star.prefab");
        GameObject objectStar = AssetDatabase.LoadAssetAtPath<GameObject>(pathFile);

        /***************************SpriteRenderer*************************/

        if (!objectStar.TryGetComponent(out SpriteRenderer spriteRenderer))
        {
            Assert.AreEqual(objectStar.AddComponent<SpriteRenderer>(), spriteRenderer,
                $"The \"{objectStar.name}\" prefab does not have a \"Sprite Renderer\" component");
        }

        pathFile = Path.Combine("Assets", "Sprites", "Star.png");
        Sprite spriteStar = AssetDatabase.LoadAssetAtPath<Sprite>(pathFile);

        Assert.AreEqual(spriteStar, spriteRenderer.sprite,
            $"The \"{objectStar.name}\" prefab in the \"Sprite Renderer\" component has an incorrect \"Sprite\" field");

        /***************************BoxCollider2D*************************/

        if (!objectStar.TryGetComponent(out BoxCollider2D boxCollider2D))
        {
            Assert.AreEqual(objectStar.AddComponent<BoxCollider2D>(), boxCollider2D,
                $"The \"{objectStar.name}\" prefab does not have a \"Box Collider 2D\" component");
        }

        Assert.IsTrue(boxCollider2D.isTrigger,
            $"The \"{objectStar.name}\" prefab in the \"Box Collider 2D\" component has an incorrect \"Is Trigger\" field");

        /***************************Enemy*************************/

        if (!objectStar.TryGetComponent(out Starpower starpower))
        {
            Assert.AreEqual(objectStar.AddComponent<Starpower>(), starpower,
                $"The \"{objectStar}\" prefab does not have a \"Starpower\" script");
        }

        /***************************Layer*************************/

        Assert.AreEqual(LayerMask.LayerToName(objectStar.layer), "Powerups",
            $"The \"{objectStar}\" prefab has an incorrect tag");
    }

    [Test]
    public void CheckingObjectStarOnScene()
    {
        GameObject gameObjectStar = GameObject.Find("Star");
        Assert.IsNotNull(gameObjectStar,
            $"There is no \"{gameObjectStar.name}\" object on the scene");

        /***************************SpriteRenderer*************************/

        if (!gameObjectStar.TryGetComponent(out SpriteRenderer spriteRenderer))
        {
            Assert.AreEqual(gameObjectStar.AddComponent<SpriteRenderer>(), spriteRenderer,
                $"The \"{gameObjectStar.name}\" prefab does not have a \"Sprite Renderer\" component");
        }

        var pathFile = Path.Combine("Assets", "Sprites", "Star.png");
        Sprite spriteStar = AssetDatabase.LoadAssetAtPath<Sprite>(pathFile);

        Assert.AreEqual(spriteStar, spriteRenderer.sprite,
            $"The \"{gameObjectStar.name}\" prefab in the \"Sprite Renderer\" component has an incorrect \"Sprite\" field");

        /***************************BoxCollider2D*************************/

        if (!gameObjectStar.TryGetComponent(out BoxCollider2D boxCollider2D))
        {
            Assert.AreEqual(gameObjectStar.AddComponent<BoxCollider2D>(), boxCollider2D,
                $"The \"{gameObjectStar.name}\" prefab does not have a \"Box Collider 2D\" component");
        }

        Assert.IsTrue(boxCollider2D.isTrigger,
            $"The \"{gameObjectStar.name}\" prefab in the \"Box Collider 2D\" component has an incorrect \"Is Trigger\" field");

        /***************************Enemy*************************/

        if (!gameObjectStar.TryGetComponent(out Starpower starpower))
        {
            Assert.AreEqual(gameObjectStar.AddComponent<Starpower>(), starpower,
                $"The \"{gameObjectStar}\" object does not have a \"Starpower\" script");
        }

        /***************************Layer*************************/

        Assert.AreEqual(LayerMask.LayerToName(gameObjectStar.layer), "Powerups",
            $"The \"{gameObjectStar}\" object has an incorrect layer");
    }

    [Test]
    public void CheckingScriptStarpower()
    {
        TestAssistant.TestingMethod(typeof(Starpower), "OnTriggerEnter2D", typeof(void), MethodAttributes.Private, new MyParameterInfo[] { new MyParameterInfo(typeof(Collider2D), "collision") });
    }

    [Test]
    public void CheckingScriptPlayer()
    {
        TestAssistant.TestingField(typeof(Player), "speedCoefficient", typeof(float), FieldAttributes.Private, true);
        TestAssistant.TestingProperty(typeof(Player), "StarPower", typeof(bool), attributesSet: MethodAttributes.Private);

        TestAssistant.TestingMethod(typeof(Player), "StarPowerActive", typeof(void), MethodAttributes.Public, new MyParameterInfo[] { new MyParameterInfo(typeof(float), "duration", 5f) });
        TestAssistant.TestingMethod(typeof(Player), "StarPowerAnimation", typeof(IEnumerator), MethodAttributes.Private, new MyParameterInfo[] { new MyParameterInfo(typeof(float), "duration") });

        GameObject gameObjectEnemy = GameObject.FindGameObjectWithTag("Player");
        Player scriptPlayer = gameObjectEnemy.GetComponent<Player>();

        TestAssistant.TestingFieldValue(typeof(Player), "speedCoefficient", scriptPlayer, 1.2f);
    }
}
