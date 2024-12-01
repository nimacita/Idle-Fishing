using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CollectItem : MonoBehaviour
{

    [Header("Main Settings")]
    [SerializeField] private FishType fishType;

    [Header("Components")]
    public GameObject unlockItem;
    public GameObject lockItem;

    void Start()
    {
        UpdateItemView();
    }

    public void UpdateItemView()
    {
        bool isFishOpen = GameSettings.instance.IsFishOpen(fishType);

        unlockItem.SetActive(isFishOpen);
        lockItem.SetActive(!isFishOpen);
    }
}
