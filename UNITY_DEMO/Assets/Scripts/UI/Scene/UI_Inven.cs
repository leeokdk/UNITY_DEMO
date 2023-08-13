using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inven : UI_Scene
{
    enum GameObjects
    {
        GridPanel,
    }

    public override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(GameObjects));

        GameObject gridPanel = Get<GameObject>((int)GameObjects.GridPanel);
        //하위 전부 순회 후 삭제
        foreach (Transform child in gridPanel.transform)
            Managers.Resource.Destroy(child.gameObject);

        // 원래 이 부분에서는 실제 인벤토리 정보를 참고하여 넣어주어야 함. 임시.
        for (int i = 0; i < 8; i++)
        {
            // manager 사용하기
            GameObject item = Managers.UI.MakeSubItem<UI_Inven_Item>(gridPanel.transform).gameObject;
            //GameObject item = Managers.Resource.Instantiate("UI/Scene/UI_Inven_Item"); //어디서 꺼내서
            //item.transform.SetParent(gridPanel.transform); //어디로 붙일건지

            // 스크립트 연결은 코드로해도 되고 프리팹으로 해놔도 되고
            // 하지만 뭘 설정하려고 할 때는 코드!
            //UI_Inven_Item invenItem = Util.GetOrAddComponent<UI_Inven_Item>(item); <- Util에서 함수 불러오는 부분을 가져와서 바로 호출하는 extension 함수 만들기 
            UI_Inven_Item invenItem = item.GetOrAddComponent<UI_Inven_Item>();
            invenItem.SetInfo($"POTION{i}");
        }

    }
}
