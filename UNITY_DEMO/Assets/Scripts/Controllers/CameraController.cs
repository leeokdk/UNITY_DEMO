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

    //--- �÷��̾� �������� update���� �̷������ ������ ī�޶� �������� �� �ڿ� �;���.
    //-- ����Ƽ ������ ���μ����� ���Ͽ� game logic �������� update ���Ŀ� �̷�����°��� late update
    void LateUpdate()
    {
        if (_mode == Define.CameraMode.QuarterView)
        {
            if (_player.IsValid() == false)
            {
                return;
            }
            //-- player�� ��� �ִ� ī�޶� ���� ������ ī�޶� �մ���(���ͺ� ����Ҷ� �ַ� ���)
            RaycastHit hit;
            LayerMask mask = LayerMask.GetMask("Block");
            bool hitWall = Physics.Raycast(_player.transform.position, _delta, out hit, _delta.magnitude, mask);
            if (hitWall)
            {
                // -- ���� �� ������ �Ÿ����� �ɲ� ������ ����ī�޶� �ű�
                float dist = (hit.point - _player.transform.position).magnitude * 0.8f;
                transform.position = _player.transform.position + _delta.normalized * dist;
            }
            else
            {
                transform.position = _player.transform.position + _delta;
                //-- ������ �÷��̾��� ��ǥ�� �ֽ��ϵ��� (�������� ī�޶� ����(45 0 0)�� ���� �����)
                transform.LookAt(_player.transform);

            }
        }
    }

    //-- �Լ��� ���ͺ� �����ϱ�
    public void SetQuarterView(Vector3 delta)
    {
        _mode = Define.CameraMode.QuarterView;
        _delta = delta;
    }
}