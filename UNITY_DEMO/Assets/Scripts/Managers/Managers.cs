using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_instance;

    //-- instance�� �ܺ����� ����
    static Managers Instatnce { get { Init(); return s_instance; } }

    #region Contents
    GameManager _game = new GameManager();
    public static GameManager Game { get { return Instatnce._game; } }
    #endregion

    #region Core
    // ���� �Ŵ��� ����
    InputManager _input = new InputManager();
    ResourceManager _resource = new ResourceManager();
    UIManager _ui = new UIManager();
    SceneManagerEx _scene = new SceneManagerEx();
    SoundManager _sound = new SoundManager();
    PoolManager _pool = new PoolManager();
    DataManager _data = new DataManager();

    //-- �Ŵ����� ����ϱ� ���� ȣ�⹮ ����. (InputManager�� ��ȯ)
    //- Input ���� �θ���  instance���� _input �̾ƿ���
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
                // GameObject�� String���� �Ű������� �ָ� name���� �޴� �ɱ�?
                // go = new GameObject("@Managers"); �̷��� ����ص� �����ϴ� �� ����. 
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go); // ���־��� �߿��ϹǷ� �Ժη� �������� ���ϵ��� ��.
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
        // data �� �׻� ��� �־�� �ϹǷ� clear�� ���� �ʿ� ����.
    }

}