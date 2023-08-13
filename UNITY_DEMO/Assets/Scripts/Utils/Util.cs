using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util
{
    // 기능성 함수 모음
    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
            component = go.AddComponent<T>();
        return component;
    }

    // go를 반환하는 버전
    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform == null) 
            return null;

        return transform.gameObject;
    }

    // go = 첫인자.
    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null)
            return null;

        if (recursive == false)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
        }
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>())
                {
                    if (string.IsNullOrEmpty (name) || component.name == name)
                        return component;
                }
        }

        return null;
    }
}
