using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnClick : MonoBehaviour
{
    void Start()
    {
        transform.parent.GetComponent<Button>().onClick.AddListener(ParrentBtnClick);
    }

    private void ParrentBtnClick()
    {
        SoundController.instance.PlayBtnClickSound();
    }
}
