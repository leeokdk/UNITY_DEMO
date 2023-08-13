using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-- Resource�� �ҷ����� �����ϴ� ������ ��� ���� �ʰ� ����Ŵ��� ������� ����
// Load, Instantiate, Destroy
public class ResourceManager
{
    //-- ���� �ε� �Լ��� �����Ǵ� ������� ��������.
    //-- generic Ÿ��(����� ���� Ÿ��)�� obejct�� ������ �Ǵ�. -- �ش� �ε�� object �����̹Ƿ�.
    public T Load<T>(string path) where T : Object
    {
        // �ش� �������� �����ϸ� �������� Ȯ���� ����.
        if (typeof(T) == typeof(GameObject))
        {
            string name = path;
            int index = name.LastIndexOf('/');
            if (index >= 0)
                name = name.Substring(index + 1);
            GameObject go = Managers.Pool.GetOriginal(name);
            // Ǯ��
            if (go != null)
                return go as T;
        }
        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        // -- ������Ʈ Ǯ : original �̹� ��� ������ �ٷ� ���

        //-- �ۼ��� load�� ������ ��������
        // -- C#�� ���ڿ�+�Ű����� �ۼ���?
        GameObject original = Load<GameObject>($"Prefabs/{path}");

        //- null�϶��� �����̹Ƿ� �α׻��� �� ���� ��
        if (original == null)
        {
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }

        // -- ������Ʈ Ǯ : Ȥ�� Ǯ���� �ְ� ������?
        if (original.GetComponent<Poolable>() != null)
            return Managers.Pool.Pop(original, parent).gameObject;

        //-- �׳� Instantiate�� ȣ���ϸ� ��͵Ǿ������ Object��� ����Ͽ� ȣ����
        GameObject go = Object.Instantiate(original, parent); //���� ����
        go.name = original.name; // �Ʒ� �ڵ� �������� �׳� ������ �̸����� �־��ָ� ��
        // ���������� ȣ���� �� �̸��� (clone)�� �ٴ� �κ� ����
        //int index = go.name.IndexOf("(Clone)");
        //if (index > 0)
        //    go.name = go.name.Substring(0, index); // substring �� �ݵ�� ���Ҵ���Ѿ���

        return go;
    }

    public void Destroy(GameObject go)
    {
        if (go == null)
            return;

        // -- ������Ʈ Ǯ : ���࿡ Ǯ���� �ʿ��� ����̸� destroy �� �ƴ� Ǯ�� �Ŵ������� ��Ź�ϱ�
        Poolable poolable = go.GetComponent<Poolable>();
        if (poolable != null)
        {
            Managers.Pool.Push(poolable);
            return;
        }

        Object.Destroy(go);
    }
}