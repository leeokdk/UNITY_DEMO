using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_instance;

    //-- instance도 외부접근 통제
    static Managers Instatnce { get { Init(); return s_instance; } }

    #region Contents
    GameManager _game = new GameManager();
    public static GameManager Game { get { return Instatnce._game; } }
    #endregion

    #region Core
    // 만든 매니저 연결
    InputManager _input = new InputManager();
    ResourceManager _resource = new ResourceManager();
    UIManager _ui = new UIManager();
    SceneManagerEx _scene = new SceneManagerEx();
    SoundManager _sound = new SoundManager();
    PoolManager _pool = new PoolManager();
    DataManager _data = new DataManager();

    //-- 매니저를 사용하기 위한 호출문 생성. (InputManager를 반환)
    //- Input 으로 부르면  instance에서 _input 뽑아오기
    public static InputManager Input { get { return Instatnce._input; } }
    public static ResourceManager Resource { get { return Instatnce._resource; } }
    public static UIManager UI { get { return Instatnce._ui; } }
    public static SceneManagerEx Scene { get { return Instatnce._scene; } }
    public static SoundManager Sound { get { return Instatnce._sound; } }
    public static PoolManager Pool { get { return Instatnce._pool; } }
    public static DataManager Data { get { return Instatnce._data; } }
    #endregion

    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        _input.OnUpdate();
    }

    static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                // GameObject에 String으로 매개변수를 주면 name으로 받는 걸까?
                // go = new GameObject("@Managers"); 이렇게 사용해도 동작하는 것 같다. 
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go); // 아주아주 중요하므로 함부로 삭제되지 못하도록 함.
            s_instance = go.GetComponent<Managers>();

            s_instance._sound.Init();
            s_instance._pool.Init();
            s_instance._data.Init();
        }
    }

    public static void Clear()
    {
        Input.Clear();
        Sound.Clear();
        UI.Clear();
        Scene.Clear();
        Pool.Clear();
        // data 는 항상 들고 있어야 하므로 clear에 넣을 필요 없음.
    }

}