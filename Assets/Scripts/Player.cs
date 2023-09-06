using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeedBase = 10f;
    private Rigidbody2D rb;
    public enum FaceDirections { Left, Right, Up, Down }
    private FaceDirections faceDirections = FaceDirections.Down;
    public FaceDirections PlayerDirection { get => faceDirections; }

    [Header("GFX")]
    [SerializeField] private SpriteRenderer sr;
    [Space]
    [SerializeField] private Sprite spriteDown;
    [SerializeField] private Sprite spriteUp;
    [SerializeField] private Sprite spriteLeft;
    [SerializeField] private Sprite spriteRight;


    [Header("Abilities & Equipment")]
    [SerializeField] private Item heldItem;

    [Header("Energy")]
    [SerializeField] private float energyMax = 100f;
    [SerializeField] private float energyStart = 50f;
    [SerializeField] private float energyDeclineRateStandard = 2f;
    [SerializeField] private Image energyFillBarLeft;
    [SerializeField] private Image energyFillBarRight;
    [SerializeField] private Image energyFlashLeft;
    [SerializeField] private Image energyFlashRight;
    [SerializeField] private List<Image> energyFlashBgs = new();
    private bool energyIsDeclining = true;
    private float energyDecreasePauseCooldownCur = 0f;
    private float energyDeclineRateCur;
    private float energyCur;
    public float EnergyCur { get => energyCur; }


    private void Start()
    {
        rb= GetComponent<Rigidbody2D>();
        if (rb == null) Debug.LogError("No rigidbody found!"); 
        if (sr == null)  Debug.LogError("No Sprite Renderer found!"); 
        if (spriteDown == null || spriteUp == null || spriteLeft == null || spriteRight == null)   Debug.LogError("Missing Sprites!");
        if (energyFillBarLeft == null) Debug.LogError("No Energy Fill Bar Left found!");
        if (energyFillBarRight == null) Debug.LogError("No Energy Fill Bar Right found!");

        energyCur = energyStart;
        energyDeclineRateCur = energyDeclineRateStandard;

    }

    private void Update()
    {
        ItemUseManager();
        EnergyManager();

        // Debug Purposes
        if(Input.GetKeyDown(KeyCode.L)) {
            energyCur = energyMax;
        }
    }

    private void FixedUpdate()
    {
        MovementManagaer();
        RotationManager();
    }

    private void EnergyManager()
    {
        // Visual Update
        float flashVisibleAtFinalPercent = 0.2f;
        float energyPercantageBar = (energyCur * (1+ flashVisibleAtFinalPercent)) / energyMax;
        float flashThreshhold = (energyCur / energyMax) - (1- flashVisibleAtFinalPercent);

        energyFillBarLeft.fillAmount = energyPercantageBar;
        energyFillBarRight.fillAmount = energyPercantageBar;

        if (flashThreshhold > 0f)
        {
            energyFlashLeft.gameObject.SetActive(true);
            energyFlashRight.gameObject.SetActive(true);
            foreach (Image bg in energyFlashBgs) bg.gameObject.SetActive(true);


            energyFlashLeft.fillAmount = flashThreshhold * (1/ flashVisibleAtFinalPercent);
            energyFlashRight.fillAmount = flashThreshhold * (1 / flashVisibleAtFinalPercent);
        }
        else {
            energyFlashLeft.gameObject.SetActive(false);
            energyFlashRight.gameObject.SetActive(false);
            foreach (Image bg in energyFlashBgs) bg.gameObject.SetActive(false);
        }


        // Decrease is paused
        if (!energyIsDeclining) 
        {
            if (energyDecreasePauseCooldownCur <= 0f)
                energyIsDeclining = true;
            else
                energyDecreasePauseCooldownCur -= Time.deltaTime;
            
            return; // Note return.
        }

        // Reduce energy over time
        energyCur -= energyDeclineRateCur * Time.deltaTime;
        energyCur = Mathf.Clamp(energyCur, 0, energyMax);
    }

    private void MovementManagaer()
    {
        Vector2 vertical = new Vector2 (0,0);
        Vector2 horizontal = new Vector2(0, 0);

        // Up
        if (Input.GetKey(KeyCode.W))
        {
            vertical += new Vector2(0, 1);
        }

        // Down
        if (Input.GetKey(KeyCode.S))
        {
            vertical -= new Vector2(0, 1);
        }

        // Left
        if (Input.GetKey(KeyCode.D))
        {
            horizontal += new Vector2(1, 0);
        }

        // Right
        if (Input.GetKey(KeyCode.A))
        {
            horizontal -= new Vector2(1, 0);
        }


        // Combine and normalize vectors to avoid "diagonal speed-up"
        Vector2 move = vertical + horizontal;
        rb.AddForce(move.normalized * moveSpeedBase, ForceMode2D.Force);
    }

    private void RotationManager()
    {
        float angle = GameManager.Instance.AngleToCam(transform.position);

        if (angle >= -45f && angle <= 45f)
        {
            sr.sprite = spriteRight;
            faceDirections = FaceDirections.Right;
        }
        if (angle >= 45f && angle <= 135f)
        {
            sr.sprite = spriteUp;
            faceDirections = FaceDirections.Up;
        }
        if (angle <= -45f && angle >= -135f)
        {
            sr.sprite = spriteDown;
            faceDirections = FaceDirections.Down;
        }
        if ((angle <= -135f && angle >= -180f) || (angle >= 135f && angle <= 180f))
        {
            sr.sprite = spriteLeft;
            faceDirections = FaceDirections.Left;
        }

    }

    private void ItemUseManager()
    {
        if (heldItem == null) return;

        if (Input.GetMouseButtonDown((int)MouseButton.Left))
        {
            heldItem.Use();
        }

    }

    public void PauseEnergyDecrease(float seconds)
    {
        energyIsDeclining = false;
        energyDecreasePauseCooldownCur = seconds;
    }

    public void AddEnergy(float amount)
    {
        energyCur += amount;
        energyCur = Mathf.Clamp(energyCur, 0, energyMax);
    }

    public void AddKnockback(float amount, Vector2 direction)
    {
        Vector2 impulse = direction.normalized * amount;
        rb.AddForce(impulse, ForceMode2D.Impulse);
    }

}
