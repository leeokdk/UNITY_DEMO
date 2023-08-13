using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UI_Base : MonoBehaviour
{

    // 각 타입별 오브젝트를 딕셔너리로 관리한다
    protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();

    // 공통으로 상속하여 쓸 수 있도록 base에서 init 선언해준다
    public abstract void Init();

    // 상속한 애들까지 모두 init되도록 선언해줌
    private void Start()
    {
        Init();
    }

    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        // 1. enum 목록을 딕셔너리에 넣어서 목록화.
        // 2, 만들어진 딕셔너리로 각 타입마다 자동연결.

        // c언어 이름을 스트링 배열 반환한다
        string[] names = Enum.GetNames(type);

        // 갯수만큼 크기의 배열생성
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        _objects.Add(typeof(T), objects);

        for (int i = 0; i < names.Length; i++)
        {
            //타입이 아니라 gameobject가 들어올 수 있음
            if (typeof(T) == typeof(GameObject))
                objects[i] = Util.FindChild(gameObject, names[i], true);
            else
                objects[i] = Util.FindChild<T>(gameObject, names[i], true);

            if (objects[i] == null)
                Debug.Log($"Failed to bind({names[i]})");
        }
    }

    // 바인드 후 꺼내오기(button, text 등 오브젝트로 반환)
    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;
        if (_objects.TryGetValue(typeof(T), out objects) == false)
            return null;

        return objects[idx] as T;
    }

    // 자주 사용하는 부분 함수로 만들기
    protected GameObject GetObject(int idx) { return Get<GameObject>(idx); }

    protected TextMeshProUGUI GetText(int idx) { return Get<TextMeshProUGUI>(idx); }

    protected Button GetButton(int idx) { return Get<Button>(idx); }

    protected Image GetImage(int idx) { return Get<Image>(idx); }

    // 이벤트 추가하는 기능
    public static void BindEvent(GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        // 자동연결
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
