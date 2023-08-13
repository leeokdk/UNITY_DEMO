using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-- Resource를 불러오고 삭제하는 과정을 모두 넣지 않고 공통매니저 기능으로 빼기
// Load, Instantiate, Destroy
public class ResourceManager
{
    //-- 실제 로드 함수가 구현되는 방식으로 구현해줌.
    //-- generic 타입(사용자 지정 타입)을 obejct로 조건을 건다. -- 해당 로드는 object 한정이므로.
    public T Load<T>(string path) where T : Object
    {
        // 해당 조건절에 부합하면 프리팹일 확률이 높다.
        if (typeof(T) == typeof(GameObject))
        {
            string name = path;
            int index = name.LastIndexOf('/');
            if (index >= 0)
                name = name.Substring(index + 1);
            GameObject go = Managers.Pool.GetOriginal(name);
            // 풀링
            if (go != null)
                return go as T;
        }
        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        // -- 오브젝트 풀 : original 이미 들고 있으면 바로 사용

        //-- 작성한 load로 프리팹 가져오기
        // -- C#의 문자열+매개변수 작성법?
        GameObject original = Load<GameObject>($"Prefabs/{path}");

        //- null일때는 오류이므로 로그생성 후 리턴 널
        if (original == null)
        {
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }

        // -- 오브젝트 풀 : 혹시 풀링된 애가 있을까?
        if (original.GetComponent<Poolable>() != null)
            return Managers.Pool.Pop(original, parent).gameObject;

        //-- 그냥 Instantiate만 호출하면 재귀되어버리니 Object라고 명시하여 호출함
        GameObject go = Object.Instantiate(original, parent); //원본 복사
        go.name = original.name; // 아래 코드 쓰지말고 그냥 프리팹 이름으로 넣어주면 됨
        // 프리팹으로 호출할 때 이름에 (clone)이 붙는 부분 제거
        //int index = go.name.IndexOf("(Clone)");
        //if (index > 0)
        //    go.name = go.name.Substring(0, index); // substring 후 반드시 재할당시켜야함

        return go;
    }

    public void Destroy(GameObject go)
    {
        if (go == null)
            return;

        // -- 오브젝트 풀 : 만약에 풀링이 필요한 대상이면 destroy 가 아닌 풀링 매니저한테 위탁하기
        Poolable poolable = go.GetComponent<Poolable>();
        if (poolable != null)
        {
            Managers.Pool.Push(poolable);
            return;
        }

        Object.Destroy(go);
    }
}