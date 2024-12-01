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

    //���������� ��� ������
    private void UpdateItemView()
    {
        //�������� ��
        if (!GameSettings.instance.IsCollectSkinOpened)
        {
            //���� �� ��������
            equiped.SetActive(false);
            locked.SetActive(true);
        }
        else
        {
            //���� ��������
            equiped.SetActive(IsEquiped());
            locked.SetActive(false);
        }

    }

    //��������� ���� ���� ����������
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

    //��������� ��� �������
    private void EquipedSelectSkin()
    {
        if (GameSettings.instance.SkinId == currentSkinId)
        {
            GameSettings.instance.SkinId = 0;
        }
        else
        {
            //���������
            SoundController.instance.PlayEquipSound();
            GameSettings.instance.SkinId = currentSkinId;
        }
    }

    //������� �� ������ �������
    public void ShopItemBtnClick()
    {
        //���� �������������
        if (GameSettings.instance.IsCollectSkinOpened)
        {
            EquipedSelectSkin();
        }
        else
        {
            //������� ���������
            shopController.SkinNotAvailablePopUp();
        }
    }
}
