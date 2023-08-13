using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScean
{
    //Coroutine co;

    protected override void Init()
    {
        base.Init();

        // 상속이라 바로 접근 가능
        SceneType = Define.Scene.Game;

        //Managers.UI.ShowSceneUI<UI_Inven>();

        Dictionary<int, Data.Stat> dict = Managers.Data.StatDict;

        // 컨트롤러 붙이기
        gameObject.GetOrAddComponent<CursorController>();

        //co = StartCoroutine("ExplodeAfterSeconds", 4.0f);
        //StartCoroutine("CoStopExplode", 2.0f);

        // 스폰하기
        GameObject player = Managers.Game.Spawn(Define.WorldObject.Player, "UnityChan");
        Camera.main.gameObject.GetOrAddComponent<CameraController>().SetPlayer(player);
        //Managers.Game.Spawn(Define.WorldObject.Monster, "Knight");

        GameObject go = new GameObject { name = "SpawningPool" };
        SpawningPool pool = go.GetOrAddComponent<SpawningPool>();
        pool.SetKeepMonsterCount(2);
    }

    // 코루틴 시간 잴때 짱조음
/*
    IEnumerator ExplodeAfterSeconds(float seconds)
    {
        Debug.Log("Cast Explode");
        yield return new WaitForSeconds(seconds);
        Debug.Log("Exploding!");
        co = null;
    }

    IEnumerator CoStopExplode(float seconds)
    {
        Debug.Log("Stop Cast");
        yield return new WaitForSeconds(seconds);
        Debug.Log("Stop Exploded");
        if (co != null)
        {
            StopCoroutine(co);
            co = null;
        }
    }
*/

    public override void Clear()
    {

    }
}
