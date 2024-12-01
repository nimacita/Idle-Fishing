using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    [Header("Item Settings")]
    [Tooltip("���� �����")]
    [Range(0, 2)]
    [SerializeField] private int currentSkinId;

    [Header("Components")]
    [SerializeField] private int coinPrice;
    [SerializeField] private GameObject shopBtn;
    [SerializeField] private TMPro.TMP_Text shopTxt;
    [SerializeField] private TMPro.TMP_Text coinTxt;
    [SerializeField] private GameObject coinBg;
    [SerializeField] private GameObject equiped;
    [SerializeField] private ShopController shopController;

    void Start()
    {
        coinTxt.text = $"{coinPrice}";
        shopBtn.GetComponent<Button>().interactable = true;
        shopBtn.GetComponent<Button>().onClick.AddListener(ShopItemBtnClick);

        UpdateItemView();
    }

    private void FixedUpdate()
    {
        UpdateItemView();
    }

    //����������� �������� ������ �� �����
    private bool IsBought
    {
        get
        {
            if (PlayerPrefs.HasKey($"IsBought{currentSkinId}"))
            {
                if (PlayerPrefs.GetInt($"IsBought{currentSkinId}") == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (currentSkinId != 0)
                {
                    PlayerPrefs.SetInt($"IsBought{currentSkinId}", 0);
                    return false;
                }
                else
                {
                    PlayerPrefs.SetInt($"IsBought{currentSkinId}", 1);
                    return true;
                }         
            }
        }

        set
        {
            if (value)
            {
                PlayerPrefs.SetInt($"IsBought{currentSkinId}", 1);
            }
            else
            {
                PlayerPrefs.SetInt($"IsBought{currentSkinId}", 0);
            }
        }
    }

    //���������� ��� ������
    private void UpdateItemView()
    {
        //��������� ������� ��
        if (!IsBought)
        {
            //���� �� ������
            shopTxt.text = "Buy";
            equiped.SetActive(false);
            coinBg.SetActive(true);
        }
        else
        {
            //���� ������
            equiped.SetActive(IsEquiped());
            coinBg.SetActive(false);
            shopTxt.text = "Select";
        }

    }

    private bool IsCoinEnought()
    {
        if (GameSettings.instance.Coins < coinPrice)
        {
            return false;
        }
        else
        {
            return true;
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

    //����� �� ������, ������� ������ ������
    private void CanClaim()
    {
        //���� �� ������� - ��������
        if (IsCoinEnought())
        {
            //����� ������
            GameSettings.instance.Coins -= coinPrice;
            SoundController.instance.PlayAddCoins();
            IsBought = true;
            if (shopController != null) shopController.UpdateCoins();

        }
    }

    //��������� ��� �������
    private void EquipedSelectSkin()
    {
        SoundController.instance.PlayEquipSound();
        if (GameSettings.instance.SkinId == currentSkinId)
        {
            GameSettings.instance.SkinId = 0;
        }
        else
        {
            //���������
            //SoundController.instance.PlayEquipSound();
            GameSettings.instance.SkinId = currentSkinId;
        }
    }

    //������� �� ������ �������
    public void ShopItemBtnClick()
    {
        if (!IsBought)
        {
            //���� ����� �������
            if (IsCoinEnought())
            {
                CanClaim();
            }
            else
            {
                //������� ���������
                shopController.NotEnoughtMoneyPopUp();
            }
        }
        else
        {
            EquipedSelectSkin();
        }
    }
}
