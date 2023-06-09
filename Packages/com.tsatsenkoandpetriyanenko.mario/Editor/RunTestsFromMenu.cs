using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.IO.MemoryMappedFiles;
using System.Linq;
using UnityEditor;
using UnityEditor.TestTools.TestRunner.Api;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class RunTestsFromMenu : ScriptableObject, ICallbacks
{
    private const string TEXT_COMPLETE_LESSON = "Tests passed successfully!";
    private const string TEXT_COMPLETE_PROJECT = "The whole project has been successfully completed!";

    private TestMode mode;
    private int numberLesson = -1;
    // Попробовать сделать через null
    private bool isAll = false;

    [MenuItem("LearningUnity/Check Lesson &c", false, 1)]
    private static void DoRunTests()
    {
        ErrorWindow.InitWindow();
        ErrorWindow.TextResultTests = "";

        CreateInstance<RunTestsFromMenu>().StartTestRunEditMode();
    }
    private void StartTestRunEditMode()
    {
        if (TryReadFileToMemory(out string message))
        {
            string[] messages = message.Split('\n');

            DirectoryInfo directoryInfo = new DirectoryInfo(Application.dataPath);
            string path = directoryInfo.Parent.Parent.FullName;

            if (path != messages[1])
            {
                ErrorWindow.TextResultTests = "You have opened the wrong project";
                return;
            }

            if (int.TryParse(messages[0], out int number))
            {
                numberLesson = number;
            }

            if (bool.TryParse(messages[2], out bool result))
            {
                isAll = result;
            }
        }

        if (numberLesson == -1 && !isAll)
        {
            ErrorWindow.TextResultTests = "Run \"Leaning Unity\" to check the tests";
            return;
        }

        hideFlags = HideFlags.HideAndDontSave;
        mode = TestMode.EditMode;

        //numberLesson = 1;

        Filter filter;

        if (!isAll)
        {
            filter = new Filter()
            {
                testMode = TestMode.EditMode,
                testNames = new string[] { "Lesson" + (numberLesson + 1) }
            };
        }
        else
        {
            filter = new Filter()
            {
                testMode = TestMode.EditMode
            };
        }

        CreateInstance<TestRunnerApi>().Execute(new ExecutionSettings(filter));
    }
    private void StartTestRunPlayMode()
    {
        hideFlags = HideFlags.HideAndDontSave;
        mode = TestMode.PlayMode;

        Filter filter;

        if (!isAll)
        {
            filter = new Filter()
            {
                testMode = TestMode.PlayMode,
                testNames = new string[] { "Lesson" + (numberLesson + 1) }
            };
        }
        else
        {
            filter = new Filter()
            {
                testMode = TestMode.PlayMode
            };
        }

        CreateInstance<TestRunnerApi>().Execute(new ExecutionSettings(filter));
    }

    public void OnEnable() { CreateInstance<TestRunnerApi>().RegisterCallbacks(this); }

    public void OnDisable() { CreateInstance<TestRunnerApi>().UnregisterCallbacks(this); }
    public void RunFinished(ITestResultAdaptor results)
    {
        if (mode == TestMode.EditMode)
        {
            if (results.FailCount == 0)
            {
                StartTestRunPlayMode();
            }
            else
            {
                DestroyImmediate(this);
            }
            return;
        }

        if (mode == TestMode.PlayMode)
        {
            if (results.FailCount == 0)
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(Application.dataPath);
                string path = directoryInfo.Parent.Parent.FullName;
                string message = string.Format("{0}\n{1}", numberLesson, path);
                if (isAll)
                {
                    ErrorWindow.TextResultTests = string.Format("<color=#00E678>{0}</color>", TEXT_COMPLETE_PROJECT);
                }
                else
                {
                    ErrorWindow.TextResultTests = string.Format("<color=#00E678>Lesson {0}\n{1}</color>", numberLesson + 1, TEXT_COMPLETE_LESSON);
                }
                TryWriteFileToMemory(message);
                SaveAssets();
            }
            DestroyImmediate(this);
        }
    }

    public void RunStarted(ITestAdaptor testsToRun) { }

    public void TestStarted(ITestAdaptor test) { }

    public void TestFinished(ITestResultAdaptor result)
    {
        if (!result.HasChildren && result.ResultState != "Passed")
        {
            if (ErrorWindow.TextResultTests == "")
            {
                ErrorWindow.TextResultTests = string.Format("<color=red>   {0}\n{1}</color>", result.Test.Parent.Name, result.Message);
            }
        }
    }

    private bool TryReadFileToMemory(out string result)
    {
        try
        {
            char[] message;
            int size;

            ErrorWindow.SharedMemory = MemoryMappedFile.OpenExisting("lesson");

            using (MemoryMappedViewAccessor reader = ErrorWindow.SharedMemory.CreateViewAccessor(0, 4, MemoryMappedFileAccess.Read))
            {
                size = reader.ReadInt32(0);
            }

            using (MemoryMappedViewAccessor reader = ErrorWindow.SharedMemory.CreateViewAccessor(4, size * 2, MemoryMappedFileAccess.Read))
            {
                //Массив символов сообщения
                message = new char[size];
                reader.ReadArray<char>(0, message, 0, size);
            }
            result = new string(message);
            return true;

        }
        catch (Exception) { }

        result = null;
        return false;
    }

    private bool TryWriteFileToMemory(string text)
    {
        try
        {
            char[] message = text.ToCharArray();
            int size = message.Length;
            ErrorWindow.SharedMemory = MemoryMappedFile.CreateOrOpen("test", size * 2 + 4);
            using (MemoryMappedViewAccessor writer = ErrorWindow.SharedMemory.CreateViewAccessor(0, size * 2 + 4))
            {
                writer.Write(0, size);
                writer.WriteArray<char>(4, message, 0, message.Length);
            }
            return true;
        }
        catch (Exception) { }
        return false;
    }

    private void SaveAssets()
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(Application.dataPath);
        string pathZip = directoryInfo.Parent.FullName + ".zip";
        if (File.Exists(pathZip))
        {
            File.Delete(pathZip);
        }
        ZipFile.CreateFromDirectory(directoryInfo.FullName, pathZip);
    }

    [MenuItem("LearningUnity/Cancel Actions/Cancel Current Lesson", false, 1)]
    private static void CancelLesson()
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(Application.dataPath);
        string pathZip = directoryInfo.Parent.FullName + ".zip";
        Directory.Delete(Application.dataPath, true);
        ZipFile.ExtractToDirectory(pathZip, directoryInfo.FullName);
        AssetDatabase.Refresh();
    }
	
	[MenuItem("LearningUnity/Cancel Actions/Cancel Current Lesson", true, 1)]
    public static bool CancelLessonValidate()
    {
		DirectoryInfo directoryInfo = new DirectoryInfo(Application.dataPath);
        string pathZip = directoryInfo.Parent.FullName + ".zip";
        return File.Exists(pathZip);
    }

    [MenuItem("LearningUnity/Cancel Actions/Cancel Entire Course", false, 2)]
    private static void CancelCourse()
    {
        if (EditorUtility.DisplayDialog("Warning!",
                "Do you really want to reset the progress of this course?",
                "Yes", "No"))
        {
            Process processLeaningUnity = Process.GetProcessesByName("Learning Unity").FirstOrDefault();
            if (processLeaningUnity == null)
            {
                /********************************************************/
                ErrorWindow.InitWindow();
                ErrorWindow.TextResultTests = "Run \"Learning Unity\"";
                return;
            }

            string directory = Path.GetDirectoryName(processLeaningUnity.MainModule.FileName);
            string nameProject = Directory.GetParent(Application.dataPath).Name;
            string pathCourse = Directory.EnumerateFiles(directory, nameProject,
                SearchOption.AllDirectories).FirstOrDefault();

            if (pathCourse == null)
            {
                /********************************************************/
                ErrorWindow.InitWindow();
                ErrorWindow.TextResultTests = string.Format("Could not find the file with the course {0}", nameProject);
                return;
            }

            ErrorWindow.TextResultTests = "";
            AssetBundle loadedAssetBundle = null;
            try
            {
                loadedAssetBundle = AssetBundle.LoadFromFile(pathCourse);
                var project = loadedAssetBundle.LoadAsset<TextAsset>("project");

                string archiveFilePath = Path.Combine(Application.temporaryCachePath, "Project.zip");
                string toDir = Directory.GetParent(Application.dataPath).FullName;

                File.WriteAllBytes(archiveFilePath, project.bytes);
                if (File.Exists(archiveFilePath))
                {
                    ZipFile.ExtractToDirectory(archiveFilePath, toDir);
                    DirectoryInfo directoryInfo = new DirectoryInfo(Application.dataPath);
                    string path = directoryInfo.Parent.Parent.FullName;
                    var instance = CreateInstance<RunTestsFromMenu>();
                    instance.TryWriteFileToMemory(string.Format("{0}\n{1}", path, true));
                    DestroyImmediate(instance);
                }

            }
            catch (Exception )
            {
                
            }
            finally
            {
                if (loadedAssetBundle != null)
                {
                    loadedAssetBundle.Unload(false);
                }
            }
        }
    }

}

[Serializable]
public class ErrorWindow : EditorWindow
{
    // По хорошему надо text разделить на несколько переменных 
    // для лучшей визуальной настройки теста
    private static string text = "";
    private Vector2 scrollPos = Vector2.zero;
    internal static MemoryMappedFile SharedMemory { get; set; }

    internal static string TextResultTests
    {
        get => text;
        set => text = value;
    }

    //[MenuItem("Window/General/Check Work", false, 201)]
    internal static void InitWindow()
    {
        EditorWindow window = GetWindow<ErrorWindow>(title: "Window Errors", desiredDockNextTo: typeof(Editor).Assembly.GetType("UnityEditor.ConsoleWindow"));
        window.Show();
        window.SaveChanges();
    }

    private void OnGUI()
    {
        GUIStyle myStyle = new GUIStyle();
        myStyle.fontSize = 14;
        myStyle.normal.textColor = Color.white;

        EditorGUILayout.Space(5f);
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        GUILayout.Label(text, myStyle);
        EditorGUILayout.EndScrollView();
        this.Repaint();
    }
}