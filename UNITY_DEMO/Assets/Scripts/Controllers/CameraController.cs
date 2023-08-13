using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Define.CameraMode _mode = Define.CameraMode.QuarterView;

    [SerializeField]
    Vector3 _delta = new Vector3(0.0f, 6.0f, -5.0f);

    [SerializeField]
    GameObject _player = null;

    public void SetPlayer(GameObject player) { _player = player; }

    void Start()
    {

    }

    //--- 플레이어 움직임이 update에서 이루어지기 때문에 카메라 움직임은 그 뒤에 와야함.
    //-- 유니티 엔진의 프로세스에 의하여 game logic 구간에서 update 이후에 이루어지는것이 late update
    void LateUpdate()
    {
        if (_mode == Define.CameraMode.QuarterView)
        {
            if (_player.IsValid() == false)
            {
                return;
            }
            //-- player를 찍고 있는 카메라가 벽을 만날때 카메라 앞당기기(쿼터뷰 사용할때 주로 사용)
            RaycastHit hit;
            LayerMask mask = LayerMask.GetMask("Block");
            bool hitWall = Physics.Raycast(_player.transform.position, _delta, out hit, _delta.magnitude, mask);
            if (hitWall)
            {
                // -- 나와 벽 사이의 거리보다 쪼끔 앞으로 메인카메라를 옮김
                float dist = (hit.point - _player.transform.position).magnitude * 0.8f;
                transform.position = _player.transform.position + _delta.normalized * dist;
            }
            else
            {
                transform.position = _player.transform.position + _delta;
                //-- 무조건 플레이어의 좌표를 주시하도록 (기존값의 카메라 각도(45 0 0)가 맞춰 변경됨)
                transform.LookAt(_player.transform);

            }
        }
    }

    //-- 함수로 쿼터뷰 설정하기
    public void SetQuarterView(Vector3 delta)
    {
        _mode = Define.CameraMode.QuarterView;
        _delta = delta;
    }
}