using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class UIManager
{
    int _order = 10; // 더 먼저 띄우고싶은 개체가 있을 수 있으므로 0~9를 비워둠

    // 팝업에 대한 정보가 담겨있는 콤포넌트를 참조
    Stack<UI_Popup> _popupStack = new Stack<UI_Popup>();
    UI_Scene _sceneUI = null;

    // 공통부분 분리
    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@UI_Root");
            if (root == null)
                root = new GameObject { name = "@UI_Root" };
            return root;
        }
    }

    // 캔버스에서 order 값 구하기
    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        if (sort)
        {
            canvas.sortingOrder = _order;
            _order++;
        }
        else
        {
            canvas.sortingOrder = 0;
        }
    }

    public T WorldSpaceUI<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/WorldSpace/{name}");
        if (parent != null)
            go.transform.SetParent(parent);

        Canvas canvas = go.GetOrAddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;

        return Util.GetOrAddComponent<T>(go);
    }

    public T MakeSubItem<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/SubItem/{name}");
        if (parent != null)
            go.transform.SetParent(parent);

        return Util.GetOrAddComponent<T>(go);
    }

    public T ShowSceneUI<T>(string name = null) where T : UI_Scene
    {
        // 기본적으로 T와 동일한 컴포넌트를 가져옴 (동일할예정)
        // 변수의 컴포넌트명은 옵션(필요시)
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/Scene/{name}");
        T sceneUI = Util.GetOrAddComponent<T>(go);
        _sceneUI = sceneUI; // 추후 사용을 위해 저장

        //-- UI가 대량으로 생길 때 폴더로 정리함.
        go.transform.SetParent(Root.transform);

        return sceneUI;
    }

    public T ShowPopupUI<T>(string name = null) where T : UI_Popup
    {
        // 기본적으로 T와 동일한 컴포넌트를 가져옴 (동일할예정)
        // 변수의 컴포넌트명은 옵션(필요시)
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/Popup/{name}");
        T popup = Util.GetOrAddComponent<T>(go);
        _popupStack.Push(popup);

        //-- UI가 대량으로 생길 때 폴더로 정리함.
        go.transform.SetParent(Root.transform);

        return popup;
    }


    public void ClosePopupUI(UI_Popup popup)
    {
        // 지정해서 닫기
        if (_popupStack.Count == 0)
            return;

        if (_popupStack.Peek() != popup)
        {
            Debug.Log("Close Popup Failed!");
            return;
        }

        ClosePopupUI();
    }

    public void ClosePopupUI()
    {
        if (_popupStack.Count == 0)
            return;

        UI_Popup popup = _popupStack.Pop();
        Managers.Resource.Destroy(popup.gameObject);
        popup = null; // 없앤 팝업에 접근 못하도록 널처리
        _order--;
    }

    public void CloseAllPopupUI()
    {
        // 모든 팝업 종료
        while (_popupStack.Count > 0)
            ClosePopupUI();
    }

    public void Clear()
    {
        CloseAllPopupUI();
        _sceneUI = null;
    }
}
