using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorkController : MonoBehaviour
{

    [Header("Main Settings")]
    private float flightForce;
    private float flightAngle;
    private Vector3 startPos;
    private Rigidbody rb;

    [Header("Components")]
    public GameObject corkVisual;
    public Animation corkAnim;
    public AnimationClip corkCasting;
    public AnimationClip corkIdle;
    public AnimationClip corkFishing;

    //bools
    private bool isKicked;
    private bool isOnWater;
    private bool isPause;

    //Actions
    public static Action onCasingEnded;
    public static Action<Depth> onDepthDefinded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        corkVisual.SetActive(false);
        startPos = transform.position;
    }


    //начальные настройки
   public void StartSettings()
   {
        rb.useGravity = false;
        corkVisual.SetActive(false);
        //устанавливаем позицию
        if (startPos != Vector3.zero)
        {
            transform.position = startPos;
        }
        else
        {
            startPos = transform.position;
        }

        isKicked = false;
        isPause = false;
        isOnWater = false;
   }

    //запустили поплавок дугой
    private void ThrewInArc()
    {
        if (isKicked && !isPause)
        {
            // –ассчитываем направление запуска на основе текущего поворота объекта и угла
            Vector3 launchDirection = Quaternion.Euler(flightAngle, 0f, 0f) * Vector3.forward;
            // ѕримен€ем силу импульса в направлении дуги
            rb.AddForce(launchDirection * flightForce, ForceMode.Impulse);
            isKicked = false;
        }
    }

    //закинули поплавок
    public void Threw(float force, float angle)
    {
        if (!isKicked)
        {
            flightForce = force;
            flightAngle = angle;
            isKicked = true;

            corkVisual.SetActive(true);
            ThrewInArc();
            rb.useGravity = true;
            PlayCastingAnim();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Water")
        {
            if (!isOnWater)
            {
                onCasingEnded?.Invoke();
                SoundController.instance.PlaySplashSound();
                PlayIdleAnim();
                isOnWater = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Shallow":
                {
                    //мелководье
                    onDepthDefinded?.Invoke(Depth.Shallow);
                    break;
                }
            case "AwerageDeep":
                {
                    //средневодье
                    onDepthDefinded?.Invoke(Depth.AverageDepth);
                    break;
                }
            case "Deep":
                {
                    //глубоководье
                    onDepthDefinded?.Invoke(Depth.Deep);
                    break;
                }
        }
    }

    private void PlayCastingAnim() { corkAnim.Play(corkCasting.name); }
    private void PlayIdleAnim() { corkAnim.Play(corkIdle.name); }
    private void PlayFishingAnim() { corkAnim.Play(corkFishing.name); }

}
