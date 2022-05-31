using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 为了防止报错,加入自己UI基类的部分代码，实际需要根据自己UI结构进行调整
/// </summary>
public abstract partial class WindowBase
{ 
    [Flags]
    public enum EWindowType
    {
        Invalid = 0,
        Main = 1 << 0,   //主UI
        Normal = 1 << 1, //普通UI
        Popup = 1 << 2,//二级弹窗
        Tips = 1 << 3, //三级弹窗
        Loading = 1 << 4, //加载
        SystemNotice = 1 << 5, //系统

        ALL = Main | Normal | Tips | Loading | SystemNotice,
    }
    protected virtual void AfterInit()
    {
    }

    protected virtual void AfterShow()
    {
       
    }

    protected virtual void BeforeClose()
    {
       
    }

    protected virtual void BeforeDestory()
    {
       
    }
    public void Close()
    {

    }


    //自定义的对象查找,只贴出结构部分防止报错
    protected T FindByPath<T>(string name)
    {
        return default(T);
    }
    public GameObject FindByPath(string name)
    {
        return null;
    }
    protected static GameObject FindByPath(GameObject go, string name)
    {
        return null;
    }

    //自定义的按钮点击绑定事件,只贴出结构部分防止报错
    public delegate void UIEventHandle<T>(GameObject go, T eventData) where T : BaseEventData;
    protected static void RegisterEventClick(GameObject go, UIEventHandle<PointerEventData> handle)
    {
        if (null == go || null == handle)
            return;
     
    }
}
