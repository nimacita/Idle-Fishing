using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectSkinItem : MonoBehaviour
{

    [SerializeField] private int currentSkinId = 3;

    [Header("Components")]
    [SerializeField] private GameObject shopBtn;
    [SerializeField] private TMPro.TMP_Text selectTxt;
    [SerializeField] private GameObject equiped;
    [SerializeField] private GameObject locked;
    [SerializeField] private ShopController shopController;

    void Start()
    {
        shopBtn.GetComponent<Button>().interactable = true;
        shopBtn.GetComponent<Button>().onClick.AddListener(ShopItemBtnClick);

        UpdateItemView();
    }

    private void FixedUpdate()
    {
        UpdateItemView();
    }

    //определяем вид кнопки
    private void UpdateItemView()
    {
        //доступно ли
        if (!GameSettings.instance.IsCollectSkinOpened)
        {
            //если не доступен
            equiped.SetActive(false);
            locked.SetActive(true);
        }
        else
        {
            //если доступно
            equiped.SetActive(IsEquiped());
            locked.SetActive(false);
        }

    }

    //проверяем если скин экипирован
    private bool IsEquiped()
    {
        bool isEquiped = false;

        if (GameSettings.instance.SkinId == currentSkinId)
        {
            isEquiped = true;
        }
        else
        {
            isEquiped = false;
        }

        return isEquiped;
    }

    //экипируем или снимаем
    private void EquipedSelectSkin()
    {
        if (GameSettings.instance.SkinId == currentSkinId)
        {
            GameSettings.instance.SkinId = 0;
        }
        else
        {
            //экипируем
            SoundController.instance.PlayEquipSound();
            GameSettings.instance.SkinId = currentSkinId;
        }
    }

    //нажатие на кнопку покупки
    public void ShopItemBtnClick()
    {
        //если разблокирован
        if (GameSettings.instance.IsCollectSkinOpened)
        {
            EquipedSelectSkin();
        }
        else
        {
            //выводим сообщение
            shopController.SkinNotAvailablePopUp();
        }
    }
}
