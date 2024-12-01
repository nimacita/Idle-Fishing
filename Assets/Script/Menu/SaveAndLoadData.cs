using System.IO;
using UnityEngine;

public class SaveAndLoadData : MonoBehaviour
{
    //выгружаем данные по переданному пути
    static public T Load<T>(string path)
    {
        //если файл существует, то возвращаем
        if (File.Exists(path))
        {
            return JsonUtility.FromJson<T>(File.ReadAllText(path));
        }
        //если файл не существует, возвращаем его дефолтные значения
        else
        {
            return default(T);
        }
    }

    //сохраняем объект класса
    static public void Save(object obj, string path)
    {
        //сохраняем переданные данные в файл с указанным путем
        File.WriteAllText(path, JsonUtility.ToJson(obj));
    }
}
