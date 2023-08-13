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
        //���� ���� ��ȸ �� ����
        foreach (Transform child in gridPanel.transform)
            Managers.Resource.Destroy(child.gameObject);

        // ���� �� �κп����� ���� �κ��丮 ������ �����Ͽ� �־��־�� ��. �ӽ�.
        for (int i = 0; i < 8; i++)
        {
            // manager ����ϱ�
            GameObject item = Managers.UI.MakeSubItem<UI_Inven_Item>(gridPanel.transform).gameObject;
            //GameObject item = Managers.Resource.Instantiate("UI/Scene/UI_Inven_Item"); //��� ������
            //item.transform.SetParent(gridPanel.transform); //���� ���ϰ���

            // ��ũ��Ʈ ������ �ڵ���ص� �ǰ� ���������� �س��� �ǰ�
            // ������ �� �����Ϸ��� �� ���� �ڵ�!
            //UI_Inven_Item invenItem = Util.GetOrAddComponent<UI_Inven_Item>(item); <- Util���� �Լ� �ҷ����� �κ��� �����ͼ� �ٷ� ȣ���ϴ� extension �Լ� ����� 
            UI_Inven_Item invenItem = item.GetOrAddComponent<UI_Inven_Item>();
            invenItem.SetInfo($"POTION{i}");
        }

    }
}
