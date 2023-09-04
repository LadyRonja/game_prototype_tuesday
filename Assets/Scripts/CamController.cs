using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    [Header("Follow")]
    [SerializeField] float camSpeed = 3f;
    private Transform cam;
    private Transform player;

    [Header("Shake")]
    [SerializeField] private float shakeStabalizer = 5f;
    [SerializeField] private float maxShake = 2f;
    private float shakeLeft = 0f;
    public float debugShake = 20f;
    private enum CamMode
    {
        Follow, Shake
    }
    CamMode mode = CamMode.Follow;

    public static CamController Instance;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        cam = Camera.main.transform;
        player = GameManager.Instance.Player.transform;

        #region Null Checks
        if (cam == null)
        {
            Debug.LogError("Main Camera not found!");
            Application.Quit();
        }
        if (player == null)
        {
            Debug.LogError("Player not found!");
            Application.Quit();
        }
        #endregion

        cam.position = new Vector3(player.position.x, player.position.y, cam.position.z);

    }


    private void FixedUpdate()
    {
        
        switch (mode)
        {
            case CamMode.Follow:
                FollowPlayer();
                break;
            case CamMode.Shake:
                Shaking();
                break;
            default:
                Debug.LogError("Reached Default in state machince, new state cases not added?");
                break;
        }
    }

    private void FollowPlayer()
    {
        Vector3 mt = Vector3.MoveTowards(cam.position, player.position, camSpeed);
        cam.position = new Vector3(mt.x, mt.y, cam.position.z);
    }

    private void Shaking()
    {
        FollowPlayer();

        cam.position += new Vector3(Random.Range(-shakeLeft, shakeLeft), Random.Range(-shakeLeft, shakeLeft), 0);
        shakeLeft -= shakeStabalizer * Time.deltaTime;

        if (shakeLeft <= 0f)
        {
            mode = CamMode.Follow;
        }
    }

    public void ShakeScreen(float amount) 
    {
        mode = CamMode.Shake;
        shakeLeft += amount * 0.03f;
        shakeLeft = Mathf.Clamp(shakeLeft, 0f, maxShake);
    }


}
