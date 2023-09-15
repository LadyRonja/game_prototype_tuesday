using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Mallet : Item
{
    [Header("Base")]
    [SerializeField] private float useCdMax = 1f;
    private float useCdCur = 0f;
    private bool usable = true;

    [Header("Swinging")]
    private bool isSwinging = false;
    [SerializeField] private float restOffSet = 0f;
    [SerializeField] private float swingSpeed = 0.1f;
    private float swingingDurationLeft = 0f;

    [Header("Effect on Player")]
    [SerializeField] private float momentum = 20f;
    [SerializeField] private float energyGainOnHit = 3f;

    [Header("Effect on Npcs")]
    [SerializeField] private Collider2D hurtbox;
    [SerializeField] private float hurtboxDuration = 0.05f;
    private float hurtboxDurationLeft = 0f;
    [SerializeField] private float knockbackAmount = 100f;
    [SerializeField] private float screenShakeOnHit = 10f;



    private void Start()
    {
        if (hurtbox == null)
        {
            Debug.LogError($"{gameObject.name} has no Collider2D");
            return;
        }
        hurtbox.isTrigger = true;
        hurtbox.enabled = false;
        swingingDurationLeft = 0;
        hurtboxDurationLeft = 0;
    }

    public override void Use()
    {
        if (!usable) return;

        // General
        usable= false;
        useCdCur = useCdMax;
        isSwinging = true;
        swingingDurationLeft = swingSpeed;

        // Swing animation + player force
        Vector3 force = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        Vector2 force2D = new Vector2(force.x, force.y);
        GameManager.Instance.Player.AddKnockback(momentum, force2D);

        // Enable hurtbox
        if (hurtbox == null) return;
        hurtbox.enabled = true;
        hurtboxDurationLeft = hurtboxDuration;

    }


    private void Update()
    {
        CooldownManager();
        RotationManager();
        HurtboxManager();
    }

    private void HurtboxManager()
    {
        if (hurtbox.enabled)
        {
            if (hurtboxDurationLeft <= 0)
            {
                hurtbox.enabled = false;
                hurtboxDurationLeft = hurtboxDuration;
            }
            else
            {
                hurtboxDurationLeft -= Time.deltaTime;
            }
        }
    }

    private void RotationManager()
    {
        // If swinging, rotate 360 degrees
        if (isSwinging)
        {
            transform.RotateAround(GameManager.Instance.Player.transform.position, Vector3.forward, 360f * Time.deltaTime/swingSpeed);

            // Countdown how long a swing takes
            if (swingingDurationLeft <= 0)
            {
                isSwinging = false;
                swingingDurationLeft = swingSpeed;
            }
            else
            {
                swingingDurationLeft -= Time.deltaTime;
            }
        }
        else // If not swinging, rest by the side
        {
            float angle = GameManager.Instance.AngleToCam(transform.position) - restOffSet;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

    }

    private void CooldownManager()
    {
        if (useCdCur <= 0) {
            usable = true;
        }
        else
        {
            useCdCur -= Time.deltaTime;
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
                IKnockbackable kb = col.gameObject.GetComponent<IKnockbackable>();
        if (kb == null) return;
        Transform kbt= col.transform;
        Vector3 dir = kbt.position - transform.position;
        dir = dir.normalized;

        kb.AddKnockback(dir * knockbackAmount);
        CamController.Instance.ShakeScreen(screenShakeOnHit);
        GameManager.Instance.Player.AddEnergy(energyGainOnHit);

    }


}
