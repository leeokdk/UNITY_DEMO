using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginScene : BaseScean
{
    protected override void Init()
    {
        base.Init();

        // ����̶� �ٷ� ���� ����
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
        // �����ϰ� �α���ȭ�鿡�� ����ȭ������ �� �� �ֵ��� ����
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            Managers.Scene.LoadScene(Define.Scene.Game);
            //SceneManager.LoadScene("Game");
        }
    }

    public override void Clear()
    {
        Debug.Log("Clear �۵���");
    }
}
