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
    private TestMode mode;
    private int numberLesson = -1;

    [MenuItem("LearningUnity/Check Lesson &c")]
    private static void DoRunTests()
    {
        ErrorWindow.Init();
        ErrorWindow.Text = "";

        CreateInstance<RunTestsFromMenu>().StartTestRunEditMode();
    }
    private void StartTestRunEditMode()
    {
        if (TryReadFileToMemory(out string message))
        {
            string[] messages = message.Split("\n");

            DirectoryInfo directoryInfo = new DirectoryInfo(Application.dataPath);
            string path = directoryInfo.Parent.Parent.FullName;

            if (path != messages[1])
            {
                ErrorWindow.Text = "You have opened the wrong project";
                return;
            }

            if (int.TryParse(messages[0], out int result))
            {
                numberLesson = result;
            }
        }

        if (numberLesson == -1)
        {
            ErrorWindow.Text = "Run \"Leaning Unity\" to check the tests";
            return;
        }

        hideFlags = HideFlags.HideAndDontSave;
        mode = TestMode.EditMode;
        CreateInstance<TestRunnerApi>().Execute(new ExecutionSettings
        (new Filter()
        {
            testMode = TestMode.EditMode,
            testNames = new string[] { "Lesson" + (numberLesson + 1) }
        }
        ));
    }
    private void StartTestRunPlayMode()
    {
        hideFlags = HideFlags.HideAndDontSave;
        mode = TestMode.PlayMode;
        CreateInstance<TestRunnerApi>().Execute(new ExecutionSettings
        (new Filter()
        {
            testMode = TestMode.PlayMode,
            testNames = new string[] { "Lesson" + (numberLesson + 1) }
        }
        ));
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
                ErrorWindow.Text = "<color=#00E678>Tests passed successfully!</color>";
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
            if (ErrorWindow.Text == "")
            {
                ErrorWindow.Text = result.Message;
            }
        }
    }

    private bool TryReadFileToMemory(out string result)
    {
        try
        {
            char[] message;
            int size;

            ErrorWindow.sharedMemory = MemoryMappedFile.OpenExisting("lesson");

            using (MemoryMappedViewAccessor reader = ErrorWindow.sharedMemory.CreateViewAccessor(0, 4, MemoryMappedFileAccess.Read))
            {
                size = reader.ReadInt32(0);
            }

            using (MemoryMappedViewAccessor reader = ErrorWindow.sharedMemory.CreateViewAccessor(4, size * 2, MemoryMappedFileAccess.Read))
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
            ErrorWindow.sharedMemory = MemoryMappedFile.CreateOrOpen("test", size * 2 + 4);
            using (MemoryMappedViewAccessor writer = ErrorWindow.sharedMemory.CreateViewAccessor(0, size * 2 + 4))
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

    [MenuItem("LearningUnity/Cancel Actions/Cancel Current Lesson")]
    private static void CancelLesson()
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(Application.dataPath);
        string pathZip = directoryInfo.Parent.FullName + ".zip";
        Directory.Delete(Application.dataPath, true);
        ZipFile.ExtractToDirectory(pathZip, directoryInfo.FullName);
        AssetDatabase.Refresh();
    }

    [MenuItem("LearningUnity/Cancel Actions/Cancel Entire Course")]
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
                ErrorWindow.Init();
                ErrorWindow.Text = "Запустите \"Learning Unity\"";
                return;
            }

            string directory = Path.GetDirectoryName(processLeaningUnity.MainModule.FileName);
            string nameProject = Directory.GetParent(Application.dataPath).Name;
            Debug.Log(processLeaningUnity.MainModule.FileName);
            Debug.Log(directory);
            string pathCourse = Directory.EnumerateFiles(directory, nameProject,
                SearchOption.AllDirectories).FirstOrDefault();

            if (pathCourse == null)
            {
                /********************************************************/
                ErrorWindow.Init();
                ErrorWindow.Text = string.Format("Не удалось найти файл с курсом {0}", nameProject);
                return;
            }

            ErrorWindow.Text = "";
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
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
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
    private static string text = "";
    private Vector2 scrollPos = Vector2.zero;
    internal static MemoryMappedFile sharedMemory;

    internal static string Text
    {
        get => text;
        set => text = value;
    }

    [MenuItem("Window/General/Check Work", false, 201)]
    internal static void Init()
    {
        EditorWindow window = GetWindow<ErrorWindow>(title: "Window Errors", desiredDockNextTo: typeof(Editor).Assembly.GetType("UnityEditor.ConsoleWindow"));
        window.Show();
        window.SaveChanges();
    }

    void OnGUI()
    {
        GUIStyle myStyle = new GUIStyle();
        myStyle.fontSize = 14;
        myStyle.normal.textColor = Color.white;


        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        GUILayout.Label(text, myStyle);
        EditorGUILayout.EndScrollView();
        this.Repaint();
    }
}