using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Popup : UI_Base
{
    // virtual 가상함수 는 최초 선언시에만 사용가능
    public override void Init()
    {
        Managers.UI.SetCanvas(gameObject, true);
    }

    // UI_POPUP을 상속받은 애들 모두 공통으로 사용할 수 있게 설정
    public virtual void ClosePopupUI()
    {
        Managers.UI.ClosePopupUI(this);
    }
}
