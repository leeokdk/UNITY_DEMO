using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginScene : BaseScean
{
    protected override void Init()
    {
        base.Init();

        // 상속이라 바로 접근 가능
        SceneType = Define.Scene.Login;
/*
        List<GameObject> list = new List<GameObject>();
        for (int i = 0; i < 5; i++)
            list.Add(Managers.Resource.Instantiate("UnityChan"));
        
        foreach (GameObject obj in list)
        {
            Managers.Resource.Destroy(obj);
        }
*/
    }

    private void Update()
    {
        // 간단하게 로그인화면에서 게임화면으로 갈 수 있도록 구현
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            Managers.Scene.LoadScene(Define.Scene.Game);
            //SceneManager.LoadScene("Game");
        }
    }

    public override void Clear()
    {
        Debug.Log("Clear 작동중");
    }
}
