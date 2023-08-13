using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScean : MonoBehaviour
{
    //기본값 세팅
    //Define.Scene _sceneType = Define.Scene.Unknown;

    // get : public / set : protected
    public Define.Scene SceneType { get; protected set; } = Define.Scene.Unknown;

    void Awake()
    {
        // Start : 컴포넌트가 비활성화 상태에서는 작동하지 않음
        // Awake : 컴포넌트가 비활성화 상태에서도 작동함
        // Awake는 Start와 다르게 Component가 만들어지는 시점에서 호출된다
        Init();
    }

    protected virtual void Init()
    {
        // 있으면 가만히 두고 없으면 Scene 생성하기
        Object obj = GameObject.FindObjectOfType(typeof(EventSystem));
        if (obj == null)
            Managers.Resource.Instantiate("UI/EventSystem").name = "@EventSystem";
    }

    public abstract void Clear();
}
