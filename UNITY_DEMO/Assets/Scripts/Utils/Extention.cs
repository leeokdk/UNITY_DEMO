using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Extention
{
    // Extention 함수 : 기존 함수에 this 붙여서 go.함수로 호출하기
    public static T GetOrAddComponent<T>(this GameObject go) where T : UnityEngine.Component
    {
        return Util.GetOrAddComponent<T>(go);
    }

    public static void BindEvent(this GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_Base.BindEvent(go, action, type);
    }

    public static bool IsValid(this GameObject go)
    {
        return go != null && go.activeSelf;
    }
}
