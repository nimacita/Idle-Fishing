using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkin : MonoBehaviour
{
    [Header("Skins")]
    public GameObject[] playerSkins;

    //настройка скнинов
    public void DefineSkin()
    {
        for (int i = 0; i < playerSkins.Length; i++)
        {
            if (i == GameSettings.instance.SkinId)
            {
                playerSkins[i].SetActive(true);
            }
            else
            {
                playerSkins[i].SetActive(false);
            }
        }
    }
}
