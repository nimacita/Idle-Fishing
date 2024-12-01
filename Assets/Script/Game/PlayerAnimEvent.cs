using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimEvent : MonoBehaviour
{
    public GameController gameController;

    public void ThrowEvent()
    {
        gameController.ThrowCork();
    }

    public void EndFishing()
    {
        gameController.IsFishingEnd();
    }
}
