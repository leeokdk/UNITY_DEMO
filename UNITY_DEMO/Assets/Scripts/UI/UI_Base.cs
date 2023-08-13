using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UI_Base : MonoBehaviour
{

    // �� Ÿ�Ժ� ������Ʈ�� ��ųʸ��� �����Ѵ�
    protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();

    // �������� ����Ͽ� �� �� �ֵ��� base���� init �������ش�
    public abstract void Init();

    // ����� �ֵ���� ��� init�ǵ��� ��������
    private void Start()
    {
        Init();
    }

    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        // 1. enum ����� ��ųʸ��� �־ ���ȭ.
        // 2, ������� ��ųʸ��� �� Ÿ�Ը��� �ڵ�����.

        // c��� �̸��� ��Ʈ�� �迭 ��ȯ�Ѵ�
        string[] names = Enum.GetNames(type);

        // ������ŭ ũ���� �迭����
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        _objects.Add(typeof(T), objects);

        for (int i = 0; i < names.Length; i++)
        {
            //Ÿ���� �ƴ϶� gameobject�� ���� �� ����
            if (typeof(T) == typeof(GameObject))
                objects[i] = Util.FindChild(gameObject, names[i], true);
            else
                objects[i] = Util.FindChild<T>(gameObject, names[i], true);

            if (objects[i] == null)
                Debug.Log($"Failed to bind({names[i]})");
        }
    }

    // ���ε� �� ��������(button, text �� ������Ʈ�� ��ȯ)
    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;
        if (_objects.TryGetValue(typeof(T), out objects) == false)
            return null;

        return objects[idx] as T;
    }

    // ���� ����ϴ� �κ� �Լ��� �����
    protected GameObject GetObject(int idx) { return Get<GameObject>(idx); }

    protected TextMeshProUGUI GetText(int idx) { return Get<TextMeshProUGUI>(idx); }

    protected Button GetButton(int idx) { return Get<Button>(idx); }

    protected Image GetImage(int idx) { return Get<Image>(idx); }

    // �̺�Ʈ �߰��ϴ� ���
    public static void BindEvent(GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        // �ڵ�����
        UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(go);

        switch (type) 
        {
            case Define.UIEvent.Click:
                evt.OnPointerClickHandler -= action;
                evt.OnPointerClickHandler += action;
                break;
            case Define.UIEvent.Drag:
                evt.OnDragHanler -= action;
                evt.OnDragHanler += action;
                break;
        }


        //evt.OnDragHanler += ((PointerEventData data) => { evt.gameObject.transform.position = data.position; });
    }

}
