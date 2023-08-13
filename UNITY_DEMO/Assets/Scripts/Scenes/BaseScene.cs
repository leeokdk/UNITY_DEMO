using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScean : MonoBehaviour
{
    //�⺻�� ����
    //Define.Scene _sceneType = Define.Scene.Unknown;

    // get : public / set : protected
    public Define.Scene SceneType { get; protected set; } = Define.Scene.Unknown;

    void Awake()
    {
        // Start : ������Ʈ�� ��Ȱ��ȭ ���¿����� �۵����� ����
        // Awake : ������Ʈ�� ��Ȱ��ȭ ���¿����� �۵���
        // Awake�� Start�� �ٸ��� Component�� ��������� �������� ȣ��ȴ�
        Init();
    }

    protected virtual void Init()
    {
        // ������ ������ �ΰ� ������ Scene �����ϱ�
        Object obj = GameObject.FindObjectOfType(typeof(EventSystem));
        if (obj == null)
            Managers.Resource.Instantiate("UI/EventSystem").name = "@EventSystem";
    }

    public abstract void Clear();
}
