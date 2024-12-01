using System.IO;
using UnityEngine;

public class SaveAndLoadData : MonoBehaviour
{
    //��������� ������ �� ����������� ����
    static public T Load<T>(string path)
    {
        //���� ���� ����������, �� ����������
        if (File.Exists(path))
        {
            return JsonUtility.FromJson<T>(File.ReadAllText(path));
        }
        //���� ���� �� ����������, ���������� ��� ��������� ��������
        else
        {
            return default(T);
        }
    }

    //��������� ������ ������
    static public void Save(object obj, string path)
    {
        //��������� ���������� ������ � ���� � ��������� �����
        File.WriteAllText(path, JsonUtility.ToJson(obj));
    }
}
