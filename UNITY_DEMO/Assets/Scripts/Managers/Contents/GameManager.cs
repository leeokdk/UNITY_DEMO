using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    GameObject _player;
    HashSet<GameObject> _monsters = new HashSet<GameObject>(); // 이 프로젝트는 키가 없는 hash로 충분

    public Action<int> OnSpawnEvent;
    public GameObject GetPlayer() { return _player; }

    // 온라인 플레이어는 여러명이니까 이렇게 관리함. 예시.
    // 몬스터나 특정 아이템도 규모가 크면 dictionary로 보통 관리함.
    //Dictionary<int, GameObject> _players = new Dictionary<int, GameObject>();

    public GameObject Spawn(Define.WorldObject type, string path, Transform parent = null)
    {
        GameObject go = Managers.Resource.Instantiate(path, parent);

        switch (type)
        {
            case Define.WorldObject.Monster:
                _monsters.Add(go);
                if (OnSpawnEvent != null)
                    OnSpawnEvent.Invoke(1);
                break;
            case Define.WorldObject.Player:
                _player = go;
                break;
        }

        return go;

    }

    public Define.WorldObject GetWorldObjectType(GameObject go)
    {
        BaseController bc = go.GetComponent<BaseController>();
        if (bc == null)
            return Define.WorldObject.Unknown;

        return bc.WorldObjectType;
    }

    public void Despawn(GameObject go)
    {
        Define.WorldObject type = GetWorldObjectType(go);

        switch (type)
        {
            case Define.WorldObject.Monster:
                {
                    if (_monsters.Contains(go))
                    {
                        _monsters.Remove(go);
                        if (OnSpawnEvent != null)
                            OnSpawnEvent.Invoke(-1);
                    }
                }
                break;
            case Define.WorldObject.Player:
                if (_player == go)
                    _player = null;
                break;
        }
        Managers.Resource.Destroy(go);
    }
}
