using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : BaseController
{
    // �ѹ��� �����ϰ� ����
    int _mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);

    // stat���� ���ǵ尡 ���ǵǾ����� ���̻� ������� ����
    //float _speed = 10.0f;
    PlayerStat _stat;
    bool _stopSkill = false;

    public override void Init()
    {
        WorldObjectType = Define.WorldObject.Player;
        _stat = gameObject.GetComponent<PlayerStat>();
        Managers.Input.MouseAction -= OnMouseEvent;
        Managers.Input.MouseAction += OnMouseEvent;

        // ���� �ӽ÷� �÷��̾� ��Ʈ�ѷ����� ȣ�����ִ� ������ �κ��� ���� scean���� ȣ�����ֵ��� ��.
        //Managers.UI.ShowSceneUI<UI_Inven>();
        //Managers.UI.ShowPopupUI<UI_Button>();
        // ui�θ���
        //Managers.Resource.Instantiate("UI/UI_Button");
        // hp ui ���̱�
        if (gameObject.GetComponentInChildren<UI_HPBar>() == null)
            Managers.UI.WorldSpaceUI<UI_HPBar>(transform);
    }

    protected override void UpdateMoving()
    {
        // Ÿ�� ���Ͱ� �����Ÿ� �ȿ� ������ ����
        if (_lockTarget != null)
        {
            _destPos = _lockTarget.transform.position;
            float distance = (_destPos - transform.position).magnitude;
            if (distance <= 1)
            {
                State = Define.State.Skill;
                return;
            }
        }

        Vector3 dir = _destPos - transform.position; //��������
        dir.y = 0;
        //-- ���ͳ����� ������ ���������� 0�� �ƴ� �� ����. ������ 0�� ���� ���ٴ� �̷������� ���ϴ°� ��õ
        // navmesh ������ �����߻�, �����ߴٴ� ������ �� �÷��ش�
        if (dir.magnitude < 0.1f)
        {
            State = Define.State.Idle;
        }
        else
        {
            // navigation ���� �κ� �߰�
            
            // ���͸� �̴� �κж����� ����
            //NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();
            //float moveDist = Mathf.Clamp(_stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
            //nma.Move(dir.normalized * moveDist);

            // �������ϴ� ��� ����ĳ��Ʈ�� �Ǻ��ϱ�
            Debug.DrawRay(transform.position, dir.normalized, Color.green);
            if (Physics.Raycast(transform.position, dir, 1.0f, LayerMask.GetMask("Block")))
            {
                // ���콺�� ��� ������ �ִ� ���¿����� ������ �ʱ�
                if (Input.GetMouseButton(0) == false)
                    State = Define.State.Idle;
                return;
            }

            float moveDist = Mathf.Clamp(_stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
            transform.position += dir.normalized * moveDist;

            //�������� �ʾ����Ƿ� ��� �̵�
            //-- �÷��̾��� ��ġ + (�������� * (�ӵ�*�ð� = �Ÿ�))
            // transform.position += dir.normalized * _speed * Time.deltaTime;
            // �̵��ϴ� ���� �̵��� �Ÿ����� �۾ƾ� ��.

            // clamp �Լ� : val, min, max �� �Է��ϸ� val�� min�� max ���� ������ ����.
            //float moveDist = Mathf.Clamp(_speed * Time.deltaTime, 0, dir.magnitude);
            //transform.position += dir.normalized * moveDist;

            //�̵��ϰ��� �ϴ� ���� �ٶ�. ��� �ٷ� look�ؼ� �Ҷ� ���ܺ���.
            // transform.LookAt(_destPos);

            // �ڿ������� ȸ����Ű��
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
        }

        //-- ���� ���� ���¿� ���� ������ �Ѱ��ش�.
        //Animator anim = GetComponent<Animator>();
        // speed �Ķ���� ����
        //anim.SetFloat("speed", _stat.MoveSpeed);
    }

    protected override void UpdateSkill()
    {
        //Animator anim = GetComponent<Animator>();
        //anim.SetBool("attack",true);

        if (_lockTarget != null)
        {
            Vector3 dir = _lockTarget.transform.position - transform.position;
            Quaternion quat = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);
        }
    }

    void OnHitEvent()
    {
        if (_lockTarget != null)
        {
            Stat targetStat = _lockTarget.GetComponent<Stat>();
            targetStat.OnAttacked(_stat);
            //PlayerStat myStat = gameObject.GetComponent<PlayerStat>();
            //int damage = Mathf.Max(0, myStat.Attack - targetStat.Defense);
            //targetStat.HP -= damage;
        }
        //Animator anim = GetComponent<Animator>();
        //anim.SetBool("attack", false);
        //State = Define.State.Idle;
        if (_stopSkill)
        {
            State = Define.State.Idle;
        }
        else
        {
            State = Define.State.Skill;
        }
    }

    void OnMouseEvent(Define.MouseEvent evt)
    {
        switch (State)
        {
            case Define.State.Idle:
                OnMouseEvent_IdleRun(evt);
                break;
            case Define.State.Moving:
                OnMouseEvent_IdleRun(evt);
                break;
            case Define.State.Skill:
                {
                    if (evt == Define.MouseEvent.PointerUp)
                        _stopSkill = true;
                }
                break;
        }
    }

    void OnMouseEvent_IdleRun(Define.MouseEvent evt)
    {
        RaycastHit hit;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Vector3 cameraPos = Camera.main.transform.position;
        // Debug.DrawRay(cameraPos, ray.direction * 100.0f, Color.red, 1.0f);

        //LayerMask lmask = LayerMask.GetMask("Ground");
        bool raycastHit = Physics.Raycast(ray, out hit, 100.0f, _mask);

        switch (evt)
        {
            case Define.MouseEvent.PointerDown:
                {
                    if (raycastHit)
                    {
                        _destPos = hit.point;
                        State = Define.State.Moving;
                        // Debug.Log($"RayCast Camera! {hit.collider.gameObject.name}");
                        // Debug.Log($"RayCast Camera! getTag {hit.collider.gameObject.tag}");
                        _stopSkill = false;

                        if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
                            _lockTarget = hit.collider.gameObject;
                        else
                            _lockTarget = null;
                    }
                }
                break;
            case Define.MouseEvent.Press:
                {
                    if (_lockTarget == null && raycastHit)
                        _destPos = hit.point;
                }
                break;
            case Define.MouseEvent.PointerUp:
                _stopSkill = true;
                break;
        }
    }

    //void UpdateDie()
    //{
    // �ƹ��͵� ���ϴ� ����
    //}

    //void UpdateIdle()
    //{
    //-- ���� ���� ���¿� ���� ������ �Ѱ��ش�.
    //Animator anim = GetComponent<Animator>();
    //anim.SetFloat("speed", 0);
    //}
}