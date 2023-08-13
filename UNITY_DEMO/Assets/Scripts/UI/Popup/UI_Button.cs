using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Button : UI_Popup
{

    // ������ �������� �ʰ� �ڵ�ȭ��Ű��
    //[SerializeField]
    //TextMeshProUGUI _text;

    // ������enum ���� ��� �����ϱ�
    enum Buttons
    {
        PointButton,
    }

    enum TextMeshProUGUIs
    {
        PointText,
        ScoreText,
    }

    enum GameObjects
    {
        TestObject,
    }

    enum Images
    {
        ItemIcon,
    }

    public override void Init()
    {
        base.Init();

        // Ÿ�� �ѱ��
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(TextMeshProUGUIs));
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));

        //Get<TextMeshProUGUI>((int)TextMeshProUGUIs.ScoreText).SetText("Bind Test");

        GetButton((int)Buttons.PointButton).gameObject.BindEvent(OnButtonClicked);

        GameObject go = GetImage((int)Images.ItemIcon).gameObject;
        BindEvent(go, (PointerEventData data) => { go.gameObject.transform.position = data.position; }, Define.UIEvent.Drag);
        //UI_EventHandler evt = go.GetComponent<UI_EventHandler>();
        //evt.OnDragHanler += ((PointerEventData data) => { evt.gameObject.transform.position = data.position; });
    }

    int _score = 0;

    public void OnButtonClicked(PointerEventData data)
    {
        _score++;
        GetText((int)TextMeshProUGUIs.ScoreText).SetText($"score : {_score}");
        //_text.SetText($"score : {_score}");
    }

}
