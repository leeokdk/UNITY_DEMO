using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//-- ����ó�� ��� �÷��̾� ��Ʈ�ѷ��� update ���ǹ��� �޸��� �Ǹ� ������ �ý��� ���ϸ� �� �� ����
//-- ����� �����̹Ƿ� �Ѱ��ϴ� �Ŵ����� ����� ���� �ҷ��� ����� �� �ְ� ����� ����
// --- 1. InputManager �� ������Ʈ���� ��ǥ�� üũ��.
// --- 2. �Է��� �ִٰ� �Ǵܵ� ��� �ش� �Ŵ����� �����ϴ� �÷��̾�� ������. (������ ����)
// üũ�ϴ� �κ��� �����ϰ�!
public class InputManager
{
    //-- action ����
    public Action KeyAction = null;
    //-- ���콺 �̺�Ʈ�� �з��Ͽ� ���� �� �ִ� ����
    public Action<Define.MouseEvent> MouseAction = null;
    //-- ������ �ִ� ������ ���θ� �˱� ���� ���°� ����
    bool _pressed = false;
    // ������ �ִ� �ð� �߰�
    float _pressedTime = 0;

    public void OnUpdate()
    {
        // UI�� Ŭ���� ���� ĳ���Ͱ� �̵����� �ʵ��� ����ó��
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        // ---- ���콺�� ���°͵� �׼��� �� ����. �Ʒ� �ڵ�δ� ���콺�� ���� ���ɼ��� ����.
        // if (Input.anyKey == false) // ���콺�� Ű����� ���� �Է����� �ʾ���
        //   return;
        // if (KeyAction != null) // Ű�׼��� �ִ�!
        //   KeyAction.Invoke();

        if (Input.anyKey && KeyAction != null)
            KeyAction.Invoke();

        if (MouseAction != null)
        {
            if (Input.GetMouseButton(0)) //-- ������ �־ ������ �� 0:����Ŭ��
            {
                if (!_pressed) // ��������
                {
                    MouseAction.Invoke(Define.MouseEvent.PointerDown);
                    _pressedTime = Time.time;
                }
                // Press �̺�Ʈ �߻�
                MouseAction.Invoke(Define.MouseEvent.Press);
                // ó�� ���콺�� ������ ��
                _pressed = true;
            }
            else
            {
                // ���콺�� ���� �� ������ ����
                if (_pressed) //-- �� ���̶� press �̺�Ʈ�� �߻��Ǿ��ٸ�
                {
                    // �ð����� �߰�
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
