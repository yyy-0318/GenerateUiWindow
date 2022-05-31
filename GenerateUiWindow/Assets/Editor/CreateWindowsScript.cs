using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CreateWindowsScript
{
    private static GameObject CurGo;
    public static Dictionary<string, string> objTypeMap = new Dictionary<string, string>()
    {
        { "Image", typeof(Image).Name },//图片       名字_Image
        { "Text", typeof(Text).Name },//文本         名字_Text
        { "Btn", typeof(GameObject).Name },//按钮    名字_Btn
        { "Go", typeof(GameObject).Name},//游戏对象    名字_Go
        { "Toggle", typeof(Toggle).Name},//开关组件          名字_Toggle
        { "Slider", typeof(Slider).Name},//滑动组件         名字_Slider
        { "InputField", typeof(InputField).Name},//输入框          名字_InputField
        { "ScrollRect", typeof(ScrollRect).Name},//Scroll View 普通滚动视图       名字_ScrollRect
        //{ "LoopHorizontal", typeof(LoopHorizontalScrollRect).Name},//水平循环列表    名字_LoopHorizontal
        //{ "LoopVertical", typeof(LoopVerticalScrollRect).Name},//垂直循环列表         名字_LoopVertical
        //{ "Grid", "UIGridTool"}, //自定义格子工具    名字_Grid
    };

    public static Dictionary<string, string> wndTypeMap = new Dictionary<string, string>()
    {
        { "Main", "EWindowType.Main" },
        { "Normal", "EWindowType.Normal" },
        { "Popup", "EWindowType.Popup" },
        { "Tips", "EWindowType.Tips"},
        { "Loading", "EWindowType.Loading"},
        { "SystemNotice", "EWindowType.SystemNotice"},
    };
    private static CreateScriptUnit info;
    public static bool CanCreate(GameObject go)
    {
        string[] typeArr = go.name.Split('_');
        if (typeArr.Length <= 1)
        {
            EditorUtility.DisplayDialog("警告", "当前选择对象不是界面或命名规范错误", "确定");
            return false;
        }
        if (typeArr[0].Contains("Wnd"))
        {
            string type = typeArr[0].Replace("Wnd", "").Trim();
            if (wndTypeMap.ContainsKey(type))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }
    [MenuItem("GameObject/导出界面脚本至当前路径", priority = 49)]
    public static void CreateScript()
    {
        GameObject[] gameObjects = Selection.gameObjects;
        //保证只有一个对象
        if (gameObjects.Length == 1 && CanCreate(gameObjects[0]))
        {
            CreateScriptAction(gameObjects[0],"",true);
        }
        else
        {
            EditorUtility.DisplayDialog("警告", "当前选择对象不可创建脚本", "确定");

        }

    }
    public static void CreateScriptAction(GameObject go,string path,bool defaultPath=false)
    {      
        if (go!=null)
        {
            string[] typeArr = go.name.Split('_');
            if (typeArr.Length<=1)
            {
                EditorUtility.DisplayDialog("警告", "当前选择对象不是界面或命名规范错误", "确定");
                return;
            }
            if (typeArr[0].Contains("Wnd"))
            {
                string type = typeArr[0].Replace("Wnd", "").Trim();
                if (wndTypeMap.ContainsKey(type))
                {
                    info = new CreateScriptUnit();
                    CurGo = go;
                    ReadChild(CurGo.transform);
                    info.classname = CurGo.name;
                    info.windowType = wndTypeMap[type];
                    info.WtiteClass(path,defaultPath);
                    info = null;
                    CurGo = null;
                }
                else
                {
                    Debug.LogError($"不可以创建的界面:{go.name}");
                }
            }
          
        }
        else
        {
            EditorUtility.DisplayDialog("警告", "你只能选择一个GameObject", "确定");
        }
    }
    public static void ReadChild(Transform tf)
    {
        foreach (Transform child in tf)
        {
            string[] typeArr = child.name.Split('_');
            if (typeArr.Length > 1)
            {
                string typeKey = typeArr[typeArr.Length - 1];
                if (objTypeMap.ContainsKey(typeKey))
                {
                    info.evenlist.Add(new UIInfo(child.name, typeKey, buildGameObjectPath(child).Replace(CurGo.name + "/", "")));
                }

            }
            if (child.childCount > 0)
            {
                ReadChild(child);
            }
        }
    }
    private static string buildGameObjectPath(Transform obj)
    {
        var buffer = new StringBuilder();

        while (obj != null)
        {
            if (buffer.Length > 0)
                buffer.Insert(0, "/");
            buffer.Insert(0, obj.name);
            obj = obj.parent;
            if (obj!=null&& obj.name== CurGo.name)
            {
                obj = null;
            }
        }
        return buffer.ToString();
    }
}
//导出脚本的模版
public class CreateScriptUnit
{
    public string classname;
    public string windowType;
    public string template = @"
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

    @attribute
    public class @ClassName 
    {  

@fields

        protected override void AfterInit()
        {
            base.AfterInit();
@body1

        }
        public void RegisterButtonEvent()
        {
@body2
        }
        protected override void AfterShow()
        {
            base.AfterShow();
        }
        protected override void BeforeClose()
        {
            base.BeforeClose();
        }
@body3
    }
";
    public List<UIInfo> evenlist = new List<UIInfo>();

    public void WtiteClass(string path, bool defaultPath = false)
    {
        bool flag = true;
        bool throwOnInvalidBytes = false;
        UTF8Encoding encoding = new UTF8Encoding(flag, throwOnInvalidBytes);
        bool append = false;
        StreamWriter writer;
        if (defaultPath)
	    {
            writer = new StreamWriter(Application.dataPath + "/" + classname + ".cs", append, encoding);
        }
        else
        {
            writer = new StreamWriter(path + "/" + classname + ".cs", append, encoding);
        }
        writer.Write(GetClasss());
        writer.Close();
        AssetDatabase.Refresh();
          
}
    public string GetClasss()
    {
        var fields = new StringBuilder();
        var body1 = new StringBuilder();
        var body2 = new StringBuilder();
        var body3 = new StringBuilder();     
        
        string Attribute = string.Format("[Window(\"{0}\", (int){1})]", classname, windowType);

        for (int i = 0; i < evenlist.Count; i++)
        {
            fields.AppendLine("\t" + evenlist[i].field);
            body1.AppendLine("\t\t" + evenlist[i].body1);

            if (!string.IsNullOrEmpty(evenlist[i].body2))
            {
                body2.AppendLine("\t\t" + evenlist[i].body2);
                body3.AppendLine("\t\t" + evenlist[i].body3);
            }
        }
        template = template.Replace("@attribute", Attribute).Trim();
        template = template.Replace("@ClassName", classname+ ": WindowBase").Trim();
        template = template.Replace("@body1", body1.ToString()).Trim();
        template = template.Replace("@fields", fields.ToString()).Trim();
        template = template.Replace("@body2", body2.ToString()).Trim();
        template = template.Replace("@body3", body3.ToString()).Trim();
        return template;
    }
}
public class UIInfo
{
    public string field;
    public string body1;
    public string body2;
    public string body3;
    public UIInfo(string name, string typeKey, string path)
    {
        field = string.Format("{0} m_{1};", CreateWindowsScript.objTypeMap[typeKey], name);
        if (typeKey == "go")
        {
            body1 = string.Format("m_{0} = FindByPath(\"{1}\");", name, path, CreateWindowsScript.objTypeMap[typeKey]);
        }
        else if (typeKey == "Btn")
        {
            body1 = string.Format("m_{0} = FindByPath(\"{1}\");", name, path, CreateWindowsScript.objTypeMap[typeKey]);
            string btn = $"On{name.Split('_')[0]}Click";
            body2 = string.Format("RegisterEventClick(m_{0}, {1});", name, btn);
            body3 = string.Format("public void {0}<PointerEventData>(GameObject go, PointerEventData eventData){1}", btn, "{}");
        }
        else if (typeKey == "Grid")
        {
            body1 = string.Format("m_{0} = new UIGridTool(FindByPath(\"{1}\"),\"\");", name, path);
        }
        else
        {
            body1 = string.Format("m_{0} = FindByPath<{1}>(\"{2}\");", name, CreateWindowsScript.objTypeMap[typeKey], path);
        }
    }
}