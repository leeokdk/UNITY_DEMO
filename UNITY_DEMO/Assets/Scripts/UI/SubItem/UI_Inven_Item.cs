using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Inven_Item : UI_Base
{
    enum GameObjects
    {
        ItemIcon,
        ItemNameText,
    }

    string _name;

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));

        Get<GameObject>((int)GameObjects.ItemNameText).GetComponent<TextMeshProUGUI>().SetText(_name);
        Get<GameObject>((int)GameObjects.ItemIcon).BindEvent((PointerEventData) => { Debug.Log($"click {_name}"); });

    }

    public void SetInfo(string name)
    {
        _name = name;
    }
}
