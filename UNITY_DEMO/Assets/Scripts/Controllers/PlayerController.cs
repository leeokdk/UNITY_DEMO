using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : BaseController
{
    // 한번만 연산하게 빼줌
    int _mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);

    // stat에서 스피드가 정의되었으니 더이상 사용하지 않음
    //float _speed = 10.0f;
    PlayerStat _stat;
    bool _stopSkill = false;

    public override void Init()
    {
        WorldObjectType = Define.WorldObject.Player;
        _stat = gameObject.GetComponent<PlayerStat>();
        Managers.Input.MouseAction -= OnMouseEvent;
        Managers.Input.MouseAction += OnMouseEvent;

        // 이제 임시로 플레이어 컨트롤러에서 호출해주는 전역적 부분을 고정 scean에서 호출해주도록 함.
        //Managers.UI.ShowSceneUI<UI_Inven>();
        //Managers.UI.ShowPopupUI<UI_Button>();
        // ui부르기
        //Managers.Resource.Instantiate("UI/UI_Button");
        // hp ui 붙이기
        if (gameObject.GetComponentInChildren<UI_HPBar>() == null)
            Managers.UI.WorldSpaceUI<UI_HPBar>(transform);
    }

    protected override void UpdateMoving()
    {
        // 타겟 몬스터가 사정거리 안에 있으면 공격
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

        Vector3 dir = _destPos - transform.position; //방향추출
        dir.y = 0;
        //-- 벡터끼리의 연산은 오차때문에 0이 아닐 수 있음. 때문에 0과 같다 보다는 이런식으로 비교하는걸 추천
        // navmesh 때문에 오차발생, 도착했다는 범위를 더 늘려준다
        if (dir.magnitude < 0.1f)
        {
            State = Define.State.Idle;
        }
        else
        {
            // navigation 적용 부분 추가
            
            // 몬스터를 미는 부분때문에 수정
            //NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();
            //float moveDist = Mathf.Clamp(_stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
            //nma.Move(dir.normalized * moveDist);

            // 가지못하는 장소 레이캐스트로 판별하기
            Debug.DrawRay(transform.position, dir.normalized, Color.green);
            if (Physics.Raycast(transform.position, dir, 1.0f, LayerMask.GetMask("Block")))
            {
                // 마우스를 계속 누르고 있는 상태에서는 멈추지 않기
                if (Input.GetMouseButton(0) == false)
                    State = Define.State.Idle;
                return;
            }

            float moveDist = Mathf.Clamp(_stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
            transform.position += dir.normalized * moveDist;

            //도착하지 않았으므로 계속 이동
            //-- 플레이어의 위치 + (순수방향 * (속도*시간 = 거리))
            // transform.position += dir.normalized * _speed * Time.deltaTime;
            // 이동하는 값이 이동할 거리보다 작아야 함.

            // clamp 함수 : val, min, max 를 입력하면 val를 min과 max 사이 값으로 보장.
            //float moveDist = Mathf.Clamp(_speed * Time.deltaTime, 0, dir.magnitude);
            //transform.position += dir.normalized * moveDist;

            //이동하고자 하는 곳을 바라봄. 대신 바로 look해서 뚝뚝 끊겨보임.
            // transform.LookAt(_destPos);

            // 자연스럽게 회전시키기
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
        }

        //-- 현재 게임 상태에 대한 정보를 넘겨준다.
        //Animator anim = GetComponent<Animator>();
        // speed 파라미터 설정
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
    // 아무것도 못하는 상태
    //}

    //void UpdateIdle()
    //{
    //-- 현재 게임 상태에 대한 정보를 넘겨준다.
    //Animator anim = GetComponent<Animator>();
    //anim.SetFloat("speed", 0);
    //}
}