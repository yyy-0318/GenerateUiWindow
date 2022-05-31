using UnityEditor;
using UnityEngine;

public class CreateScriptEditor : EditorWindow
{
    [MenuItem("GameObject/导出界面脚本", priority = 49)]
    public static void Open()
    {
        var window = (CreateScriptEditor)EditorWindow.GetWindow(typeof(CreateScriptEditor), false, "打包脚本");
        window.Show();
    }
    public void Show()
    {
        GameObject[] gameObjects = Selection.gameObjects;
        //保证只有一个对象
        if (gameObjects.Length == 1&& CreateWindowsScript.CanCreate(gameObjects[0]))
        {
            go = gameObjects[0];
            objName = go.name;
            this.minSize = this.maxSize = new Vector2(500, 120);
            base.Show();
        }
        else
        {
            EditorUtility.DisplayDialog("警告", "当前选择对象不可创建脚本", "确定");
            base.Close();
        }
       
    }
    string objName;
    GameObject go;
    static string patchPath;
    private void OnGUI()
    {
        GUILayout.BeginVertical();
        GUILayout.Space(5);
        GUILayout.EndVertical();
        GUILayout.BeginHorizontal();
        GUILayout.Space(5);
        GUILayout.Label($"要生成脚本的界面预制：{objName}", GUILayout.Width(300));
        GUILayout.Space(10);
        GUILayout.EndHorizontal();

        GUILayout.BeginVertical();
        GUILayout.Space(10);
        GUILayout.EndVertical();

        GUILayout.BeginHorizontal();
        GUILayout.Label("脚本导出路径：", GUILayout.Width(90));
        patchPath = EditorGUILayout.TextField("", patchPath);
        if (GUILayout.Button("...", GUILayout.Width(40)))
            patchPath = SelectPatchPath();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Space(80);
        if (GUILayout.Button("导出脚本", GUILayout.Width(350), GUILayout.Height(30)))
        {
            if (string.IsNullOrEmpty(patchPath))
            {
                UnityEditor.EditorUtility.DisplayDialog("提示", "未填脚本导出路径", "确定");
                return;
            }
            CreateWindowsScript.CreateScriptAction(go, patchPath);
        }
        GUILayout.EndHorizontal();
    }
    //选择补丁路径
    string SelectPatchPath()
    {
        string tmpSelectPath = EditorUtility.OpenFolderPanel("选择导出路径", "", "");
        tmpSelectPath += "/";
        return tmpSelectPath;
    }
}
