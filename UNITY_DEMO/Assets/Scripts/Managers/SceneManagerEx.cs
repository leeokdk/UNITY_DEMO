using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
    public BaseScean CurrentScene { get { return GameObject.FindObjectOfType<BaseScean>(); } }

    public void LoadScene(Define.Scene type)
    {
        // 현재 씬 클리어 후 다음 씬으로 이동
        Managers.Clear();
        // define 에 미리 정의했던 값으로 들어가게끔 제한
        SceneManager.LoadScene(GetSceneName(type));
    }

    string GetSceneName(Define.Scene type)
    {
        string name = System.Enum.GetName(typeof(Define.Scene), type);
        return name;
    }

    public void Clear()
    {
        CurrentScene.Clear();
    }
}
