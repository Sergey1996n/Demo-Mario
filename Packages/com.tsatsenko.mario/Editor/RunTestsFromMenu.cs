using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEditor.TestTools.TestRunner;
using UnityEditor.TestTools.TestRunner.Api;
using UnityEngine;

public class RunTestsFromMenu : ScriptableObject, ICallbacks
{
    private TestMode mode;
    public static int numberLesson = -1;

    private static MemoryMappedFile sharedMemory;

    [MenuItem("LearningUnity/Check Lesson &c")]
    private static void DoRunTests()
    {
        ErrorWindow.Init();
        CreateInstance<RunTestsFromMenu>().StartTestRunEditMode();
        //CreateInstance<RunTestsFromMenu>().StartTestRunPlayMode();
        ErrorWindow.Text = numberLesson.ToString();
    }
    private void StartTestRunEditMode()
    {
        if (TryReadFileToMemory(out var message))
        {
            if (int.TryParse(message, out var result))
            {
                numberLesson = result;
            }
        }

        if (numberLesson == -1)
        {
            ErrorWindow.Init();
            ErrorWindow.Text = "Запустите \"Unity leaning\" для проверки тестов";
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
        if (TryReadFileToMemory(out var message))
        {
            if (int.TryParse(message, out var result))
            {
                numberLesson = result;
            }
        }

        Debug.Log("numberLesson: " + numberLesson);
        if (numberLesson == -1)
        {
            ErrorWindow.Init();
            ErrorWindow.Text = "Запустите \"Unity leaning\" для проверки тестов";
            return;
        }

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
        
        Debug.Log(results.Output);
        Debug.Log(results.Children.First());
        //Debug.Log("Количество тестов: " + results.AssertCount);
        //Debug.Log("Количество тестов: " + results.FailCount);
        //Debug.Log("Количество тестов: " + results.InconclusiveCount);
        //Debug.Log("Количество тестов: " + results.PassCount);
        //Debug.Log("Количество тестов: " + results.SkipCount);
        //Debug.Log("Количество тестов: " + numberLesson);

        //if (mode == TestMode.PlayMode && results.FailCount == 0)
        //{

        //    TryWriteFileToMemory(numberLesson.ToString());
        //}

        if (mode == TestMode.EditMode && results.FailCount == 0)
        {
            //CreateInstance<RunTestsFromMenu>().StartTestRunPlayMode();
        }

        
        //Debug.Log(results.Name);
        //Debug.Log(results.FullName);
        //Debug.Log("Lesson" + (numberLesson + 1));
        if (results.FailCount == 0)
        {
            
            TryWriteFileToMemory(numberLesson.ToString());
            ErrorWindow.Text = "<color=#00E678>Tests passed successfully!</color>";
        }
        DestroyImmediate(this);

        //Application.Quit(results.FailCount > 0 ? 1 : 0);

    }

    public void RunStarted(ITestAdaptor testsToRun) { }

    public void TestStarted(ITestAdaptor test) { }

    public void TestFinished(ITestResultAdaptor result)
    {
        if (!result.HasChildren && result.ResultState != "Passed")
        {
            ErrorWindow.Text = result.Message;
        }
    }

    private static bool TryReadFileToMemory(out string result)
    {
        try
        {
            char[] message;
            int size;

            sharedMemory = MemoryMappedFile.OpenExisting("lesson");

            using (MemoryMappedViewAccessor reader = sharedMemory.CreateViewAccessor(0, 4, MemoryMappedFileAccess.Read))
            {
                size = reader.ReadInt32(0);
            }

            using (MemoryMappedViewAccessor reader = sharedMemory.CreateViewAccessor(4, size * 2, MemoryMappedFileAccess.Read))
            {
                //Массив символов сообщения
                message = new char[size];
                reader.ReadArray<char>(0, message, 0, size);
            }
            result = new string(message);
            return true;

        }
        catch (Exception e) 
        {
            Debug.Log(e);
        }

        result = null;
        return false;
    }

    private static bool TryWriteFileToMemory(string text)
    {
        try
        {
            char[] message = text.ToCharArray();
            int size = message.Length;
            sharedMemory = MemoryMappedFile.CreateOrOpen("test", size * 2 + 4);
            using (MemoryMappedViewAccessor writer = sharedMemory.CreateViewAccessor(0, size * 2 + 4))
            {
                writer.Write(0, size);
                writer.WriteArray<char>(4, message, 0, message.Length);
            }
            return true;
        }
        catch (Exception) { }
        return false;
    }
}

[Serializable]
public class ErrorWindow : EditorWindow
{
    private static string text = "";
    private Vector2 scrollPos = Vector2.zero;

    internal static string Text
    {
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