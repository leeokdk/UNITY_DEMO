using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

public class CursorController : MonoBehaviour
{
    // 아이콘값 정의
    Texture2D _attackIcon;
    Texture2D _handIcon;

    public enum CursorType
    {
        None,
        Attack,
        Hand,
    }

    CursorType _curssorType = CursorType.None;

    void Start()
    {
        _attackIcon = Managers.Resource.Load<Texture2D>("Textures/Cursor/Attack");
        _handIcon = Managers.Resource.Load<Texture2D>("Textures/Cursor/Hand");
    }

    int _mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);

    void Update()
    {
        // 마우스 누르고 있는 중에는 안바뀜
        if (Input.GetMouseButton(0))
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100.0f, _mask))
        {
            if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
            {
                if (_curssorType != CursorType.Attack)
                {
                    Cursor.SetCursor(_attackIcon, new Vector2(_attackIcon.width / 5, 0), CursorMode.Auto);
                    _curssorType = CursorType.Attack;
                }
            }
            else
            {
                if (_curssorType != CursorType.Hand)
                {
                    Cursor.SetCursor(_handIcon, new Vector2(_handIcon.width / 3, 0), CursorMode.Auto);
                    _curssorType = CursorType.Hand;
                }
            }
        }
    }
}
