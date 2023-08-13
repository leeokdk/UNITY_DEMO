using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Popup : UI_Base
{
    // virtual �����Լ� �� ���� ����ÿ��� ��밡��
    public override void Init()
    {
        Managers.UI.SetCanvas(gameObject, true);
    }

    // UI_POPUP�� ��ӹ��� �ֵ� ��� �������� ����� �� �ְ� ����
    public virtual void ClosePopupUI()
    {
        Managers.UI.ClosePopupUI(this);
    }
}
