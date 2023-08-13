using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//-- 지금처럼 모든 플레이어 컨트롤러에 update 조건문이 달리게 되면 굉장히 시스템 부하를 줄 수 있음
//-- 공통된 사항이므로 총괄하는 매니저를 만들어 각각 불러와 사용할 수 있게 만들면 좋다
// --- 1. InputManager 의 업데이트문이 대표로 체크함.
// --- 2. 입력이 있다고 판단될 경우 해당 매니저를 구독하는 플레이어에게 전파함. (리스너 패턴)
// 체크하는 부분을 유일하게!
public class InputManager
{
    //-- action 정의
    public Action KeyAction = null;
    //-- 마우스 이벤트를 분류하여 날릴 수 있는 변수
    public Action<Define.MouseEvent> MouseAction = null;
    //-- 누르고 있는 상태의 여부를 알기 위한 상태값 변수
    bool _pressed = false;
    // 누르고 있는 시간 추가
    float _pressedTime = 0;

    public void OnUpdate()
    {
        // UI를 클릭할 때는 캐릭터가 이동하지 않도록 예외처리
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        // ---- 마우스를 떼는것도 액션의 한 종류. 아래 코드로는 마우스가 씹힐 가능성이 있음.
        // if (Input.anyKey == false) // 마우스든 키보드든 뭐든 입력하지 않았음
        //   return;
        // if (KeyAction != null) // 키액션이 있다!
        //   KeyAction.Invoke();

        if (Input.anyKey && KeyAction != null)
            KeyAction.Invoke();

        if (MouseAction != null)
        {
            if (Input.GetMouseButton(0)) //-- 누르고만 있어도 무조건 뜸 0:왼쪽클릭
            {
                if (!_pressed) // 누른직후
                {
                    MouseAction.Invoke(Define.MouseEvent.PointerDown);
                    _pressedTime = Time.time;
                }
                // Press 이벤트 발생
                MouseAction.Invoke(Define.MouseEvent.Press);
                // 처음 마우스를 눌렀을 때
                _pressed = true;
            }
            else
            {
                // 마우스를 떼면 이 쪽으로 들어옴
                if (_pressed) //-- 한 번이라도 press 이벤트가 발생되었다면
                {
                    // 시간측정 추가
                    if (Time.time < _pressedTime + 0.2f)
                        MouseAction.Invoke(Define.MouseEvent.Click);
                    MouseAction.Invoke(Define.MouseEvent.PointerUp);
                }
                _pressed = false;
                _pressedTime = 0;
            }
        }
    }

    public void Clear()
    {
        KeyAction = null;
        MouseAction = null;
    }
}
