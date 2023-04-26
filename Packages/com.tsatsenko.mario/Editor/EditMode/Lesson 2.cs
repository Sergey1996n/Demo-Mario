using NUnit.Framework;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

[ExecuteInEditMode]
public class Lesson2
{
    private string pathSceneLevel = Path.Combine("Assets", "Scenes", "Level.unity");

    [Test]
    public void __ExistingDirectoriesAndFiles()
    {
        var pathDirectory = Path.Combine("Assets", "Sprites");
        var exists = Directory.Exists(pathDirectory);
        Assert.IsTrue(exists,
            "The \"{0}\" directory does not have the \"{1}\" directory", new object[] { "Assets" , "Sprites" });

        var pathFile = Path.Combine("Assets", "Sprites", "Tileset.png");
        exists = File.Exists(pathFile);
        Assert.IsTrue(exists,
            "The \"{0}\" directory does not have the \"{1}\" sprite", new object[] { "Sprites", "Tileset" });

        pathDirectory = Path.Combine("Assets", "Sprites", "Palettes");
        exists = Directory.Exists(pathDirectory);
        Assert.IsTrue(exists,
            "The \"{0}\" directory does not have the \"{1}\" directory", new object[] { "Sprites", "Palettes" });

        pathFile = Path.Combine("Assets", "Sprites", "Palettes", "Level.prefab");
        exists = File.Exists(pathFile);
        Assert.IsTrue(exists,
            "The \"{0}\" directory does not have the \"{1}\" palette", new object[] { "Palettes", "Level" });

        pathDirectory = Path.Combine("Assets", "Sprites", "Tiles");
        exists = Directory.Exists(pathDirectory);
        Assert.IsTrue(exists,
            "The \"{0}\" directory does not have the \"{1}\" directory", new object[] { "Sprites", "Tiles" });

        pathFile = Path.Combine("Assets", "Sprites", "Tiles", "Bonus.asset");
        exists = File.Exists(pathFile);
        Assert.IsTrue(exists,
            "The \"{0}\" directory does not have the \"{1}\" asset", new object[] { "Tiles", "Bonus" });

        pathFile = Path.Combine("Assets", "Sprites", "Tiles", "Bricks.asset");
        exists = File.Exists(pathFile);
        Assert.IsTrue(exists,
            "The \"{0}\" directory does not have the \"{1}\" asset", new object[] { "Tiles", "Bricks" });

        pathFile = Path.Combine("Assets", "Sprites", "Tiles", "Ground.asset");
        exists = File.Exists(pathFile);
        Assert.IsTrue(exists,
            "The \"{0}\" directory does not have the \"{1}\" asset", new object[] { "Tiles", "Ground" });

        pathFile = Path.Combine("Assets", "Sprites", "Tiles", "LeftBush.asset");
        exists = File.Exists(pathFile);
        Assert.IsTrue(exists,
            "The \"{0}\" directory does not have the \"{1}\" asset", new object[] { "Tiles", "LeftBush" });

        pathFile = Path.Combine("Assets", "Sprites", "Tiles", "CenterBush.asset");
        exists = File.Exists(pathFile);
        Assert.IsTrue(exists,
            "The \"{0}\" directory does not have the \"{1}\" asset", new object[] { "Tiles", "CenterBush" });

        pathFile = Path.Combine("Assets", "Sprites", "Tiles", "RightBush.asset");
        exists = File.Exists(pathFile);
        Assert.IsTrue(exists,
            "The \"{0}\" directory does not have the \"{1}\" asset", new object[] { "Tiles", "RightBush" });

        if (SceneManager.GetActiveScene().name != "Level")
        {
            EditorSceneManager.OpenScene(pathSceneLevel);
        }
    }

    [Test]
    public void _1CheckingSpriteTileset()
    {
        var pathFile = Path.Combine("Assets", "Sprites", "Tileset.png");
        TextureImporter importer = TextureImporter.GetAtPath(pathFile) as TextureImporter;

        Assert.AreEqual(TextureImporterType.Sprite, importer.textureType,
            "The \"{0}\" sprite has an incorrect \"{1}\" field", new object[] { "Tileset", "Texture Type" });

        Assert.AreEqual(SpriteImportMode.Multiple, importer.spriteImportMode,
            "The \"{0}\" sprite has an incorrect \"{1}\" field", new object[] { "Tileset", "Sprite Import Mode" });

        Assert.AreEqual(16f, importer.spritePixelsPerUnit,
            "The \"{0}\" sprite has an incorrect \"{1}\" field", new object[] { "Tileset", "Pixels Per Unit" });

        Assert.AreEqual(FilterMode.Point, importer.filterMode,
            "The \"{0}\" sprite has an incorrect \"{1}\" field", new object[] { "Tileset", "Filter Mode" });

        Assert.Greater(importer.spritesheet.Length, 1,
            "The \"{0}\" sprite is not sliced", new object[] { "Tileset" });
    }

    [Test]
    public void _2CheckingTilesAsset()
    {
        var pathFile = Path.Combine("Assets", "Sprites", "Tileset.png");
        Object[] spritesPlayer = AssetDatabase.LoadAllAssetRepresentationsAtPath(pathFile);

        pathFile = Path.Combine("Assets", "Sprites", "Tiles", "Bonus.asset");
        Tile tile = AssetDatabase.LoadAssetAtPath<Tile>(pathFile);
        
        Assert.AreEqual(spritesPlayer[23], tile.sprite,
            "The \"{0}\" tile has an incorrect \"{1}\" field", new object[] { "Bonus", "Sprite" });

        pathFile = Path.Combine("Assets", "Sprites", "Tiles", "Ground.asset");
        tile = AssetDatabase.LoadAssetAtPath<Tile>(pathFile);

        Assert.AreEqual(spritesPlayer[0], tile.sprite,
            "The \"{0}\" tile has an incorrect \"{1}\" field", new object[] { "Ground", "Sprite" });

        pathFile = Path.Combine("Assets", "Sprites", "Tiles", "LeftBush.asset");
        tile = AssetDatabase.LoadAssetAtPath<Tile>(pathFile);

        Assert.AreEqual(spritesPlayer[251], tile.sprite,
            "The \"{0}\" tile has an incorrect \"{1}\" field", new object[] { "LeftBush", "Sprite" });

        pathFile = Path.Combine("Assets", "Sprites", "Tiles", "CenterBush.asset");
        tile = AssetDatabase.LoadAssetAtPath<Tile>(pathFile);

        Assert.AreEqual(spritesPlayer[252], tile.sprite,
            "The \"{0}\" tile has an incorrect \"{1}\" field", new object[] { "CenterBush", "Sprite" });

        pathFile = Path.Combine("Assets", "Sprites", "Tiles", "RightBush.asset");
        tile = AssetDatabase.LoadAssetAtPath<Tile>(pathFile);

        Assert.AreEqual(spritesPlayer[253], tile.sprite,
            "The \"{0}\" tile has an incorrect \"{1}\" field", new object[] { "RightBush", "Sprite" });
    }

    [Test]
    public void _3CheckingObjectTilemapOnScene()
    {
        GameObject gameObjectTilemap = GameObject.Find("Tilemap");

        Assert.IsNotNull(gameObjectTilemap,
            "There is no \"{0}\" object on the scene", new object[] { "Tilemap" });
    }

    [Test]
    public void _4CheckingObjectTilemap1OnScene()
    {
        GameObject gameObjectTilemap = GameObject.Find("Tilemap (1)");

        Assert.IsNotNull(gameObjectTilemap,
            "There is no \"{0}\" object on the scene", new object[] { "Tilemap (1)" });

        /***************************TilemapRenderer*************************/

        if (!gameObjectTilemap.TryGetComponent(out TilemapRenderer tilemapRenderer))
        {
            Assert.AreEqual(gameObjectTilemap.AddComponent<TilemapRenderer>(), tilemapRenderer,
                "The \"{0}\" object does not have a \"{1}\" component", new object[] { gameObjectTilemap.name, "Tilemap Renderer" });
        }

        Assert.AreEqual(-10, tilemapRenderer.sortingOrder,
            $"The \"{0}\" object in the \"{1}\" component has an incorrect \"{2}\" field", new object[] { gameObjectTilemap.name, "Tilemap Renderer", "Sorting Order" });
    }

    [Test]
    public void _5EditingSizeTilemaps()
    {
        Tilemap[] tilemaps = GameObject.FindObjectsOfType<Tilemap>();
        foreach (var tilemap in tilemaps)
        {
            tilemap.RefreshAllTiles();
            tilemap.CompressBounds();
            tilemap.ResizeBounds();
        }
    }

    [Test]
    public void _6ExistsTilesInTilemap()
    {
        GameObject gameObjectTilemap = GameObject.Find("Tilemap");
        Tilemap tilemap = gameObjectTilemap.GetComponent<Tilemap>();

        var position = tilemap.cellBounds.position;
        var size = tilemap.cellBounds.size;

        int countTileGround = 0; // 10
        int countTileBricks = 0; // 5
        int countTileBonus = 0; // 2

        for (int x = position.x; x < position.x + size.x; x++)
        {
            for (int y = position.y; y < position.y + size.y; y++)
            {
                var tile = tilemap.GetTile(new Vector3Int(x, y, 0));
                if (tile != null)
                {
                    switch (tile.name)
                    {
                        case "Ground":
                            countTileGround++;
                            break;
                        case "Bricks":
                            countTileBricks++;
                            break;
                        case "Bonus":
                            countTileBonus++;
                            break;
                        default:
                            if (tile.name.Contains("Bush"))
                            {
                                Assert.Fail("In the {0} object there should be no {1} tile", new object[] { "Tilemap", tile.name });
                            }
                            break;
                    }
                }
            }
        }

        Assert.LessOrEqual(10, countTileGround,
            "In the \"{0}\" object incorrect number of \"{1}\" tiles", new object[] {"Tilemap", "Ground"});

        Assert.LessOrEqual(5, countTileBricks,
            "In the \"{0}\" object incorrect number of \"{1}\" tiles", new object[] {"Tilemap", "Bricks"});

        Assert.LessOrEqual(2, countTileBonus,
            "In the \"{0}\" object incorrect number of \"{1}\" tiles", new object[] {"Tilemap", "Bonus"});
    }

    [Test]
    public void _7ExistsTileBushInTilemap1()
    {
        GameObject gameObjectTilemap = GameObject.Find("Tilemap (1)");
        Tilemap tilemap = gameObjectTilemap.GetComponent<Tilemap>();

        var position = tilemap.cellBounds.position;
        var size = tilemap.cellBounds.size;

        bool isExist = false;
        for (int x = position.x; x < position.x + size.x && !isExist; x++)
        {
            for (int y = position.y; y < position.y + size.y; y++)
            {
                var tileMain = tilemap.GetTile(new Vector3Int(x, y, 0));
                switch (tileMain?.name)
                {
                    case "LeftBush":
                        var tile = tilemap.GetTile(new Vector3Int(x + 1, y, 0));

                        Assert.IsNotNull(tile,
                            "In the \"{0}\" object, the right of the \"{1}\" tile there should be a \"{2}\" tile", new object[] { "Tilemap (1)", "LeftBush", "CenterBush"});

                        Assert.AreEqual("CenterBush", tile.name,
                            "In the \"{0}\" object, the right of the \"{1}\" tile there should be a \"{2}\" tile", new object[] { "Tilemap (1)", "LeftBush", "CenterBush"});
                    break;
                    case "CenterBush":
                        tile = tilemap.GetTile(new Vector3Int(x - 1, y, 0));

                        Assert.IsNotNull(tile,
                            "In the \"{0}\" object, the left of the \"{1}\" tile there should be a \"{2}\" tile", new object[] { "Tilemap (1)", "CenterBush", "LeftBush"});

                        Assert.AreEqual("LeftBush", tile.name,
                            "In the \"{0}\" object, the left of the \"{1}\" tile there should be a \"{2}\" tile", new object[] { "Tilemap (1)", "CenterBush", "LeftBush"});

                        tile = tilemap.GetTile(new Vector3Int(x + 1, y, 0));

                        Assert.IsNotNull(tile,
                            "In the \"{0}\" object, the right of the \"{1}\" tile there should be a \"{2}\" tile", new object[] { "Tilemap (1)", "CenterBush", "RightBush"});

                        Assert.AreEqual("RightBush", tile.name,
                            "In the \"{0}\" object, the right of the \"{1}\" tile there should be a \"{2}\" tile", new object[] { "Tilemap (1)", "CenterBush", "RightBush"});
                    break;
                    case "RightBush":
                        tile = tilemap.GetTile(new Vector3Int(x - 1, y, 0));

                        Assert.IsNotNull(tile,
                            "In the \"{0}\" object, the left of the \"{1}\" tile there should be a \"{2}\" tile", new object[] { "Tilemap (1)", "RightBush", "CenterBush"});

                        Assert.AreEqual("CenterBush", tile.name,
                            "In the \"{0}\" object, the left of the \"{1}\" tile there should be a \"{2}\" tile", new object[] { "Tilemap (1)", "RightBush", "CenterBush"});
                    break;
                    default:
                        break;
                }

                if (tilemap.GetTile(new Vector3Int(x, y, 0))?.name == "LeftBush" &&
                    tilemap.GetTile(new Vector3Int(x + 1, y, 0))?.name == "CenterBush" &&
                    tilemap.GetTile(new Vector3Int(x + 2, y, 0))?.name == "RightBush")
                {
                    isExist = true;
                    break;
                }
            }
        }
        Assert.IsTrue(isExist,
            "In the \"{0}\" object has not \"{1}\" object", new object[] {"Tilemap (1)", "Bush"});
    }

    [Test]
    public void _8ExistsObjectCloundInScene()
    {
        GameObject gameObjectClouds = GameObject.Find("Clouds");

        Assert.IsNotNull(gameObjectClouds,
            "There is no \"{0}\" object on the scene", new object[] { "Clouds" });

        GameObject[] gameObjectsClound = GameObject.FindObjectsOfType<GameObject>().Where(g => g.name == "Cloud").ToArray();

        Assert.Greater(gameObjectsClound.Length, 0,
            "There is no \"{0}\" object on the scene", new object[] { "Cloud" });

        foreach (var item in gameObjectsClound)
        {
            Assert.AreEqual(item.transform.parent.name, "Clouds",
                "The \"{0}\" object is located outside the parent \"{1}\" object", new object[] {"Cloud", "Clouds"});
        }
    }

    [Test]
    public void _9CheckingObjectMainCameraInScene()
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
            "The \"{0}\" object in the \"{1}\" component has an incorrect value in the \"{2}\" field in the red channel", new object[] {gameObjectMainCamera.name, "Camera", "Background"});

        Assert.AreEqual(136, (int)(camera.backgroundColor.g * 255), 
            "The \"{0}\" object in the \"{1}\" component has an incorrect value in the \"{2}\" field in the green channel", new object[] {gameObjectMainCamera.name, "Camera", "Background"});
        
        Assert.AreEqual(251, (int)(camera.backgroundColor.b * 255), 
            "The \"{0}\" object in the \"{1}\" component has an incorrect value in the \"{2}\" field in the blue channel", new object[] {gameObjectMainCamera.name, "Camera", "Background"});
    }
}
