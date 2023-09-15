using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private Player player;
    public Player Player { get => player;}

    [SerializeField] private GameObject GameOverUI;

    [SerializeField] bool checkGameOver = true;
    [SerializeField] float timeForLevel = 30f;
    float timerCur = 0f;
    [SerializeField] Image timerFill;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            GameObject.Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        GameOverUI.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public float AngleToCam(Vector3 pos) 
    {
        Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(pos);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        return angle;
    }

    private void Update()
    {
        TimerManager();

        if (checkGameOver)
            CheckGameOver();

        if (GameOverUI.activeInHierarchy)
        {
            if (Input.GetKey(KeyCode.R))
            {
                UnityEngine.SceneManagement.Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);
            }
        }
    }

    private void CheckGameOver()
    {
        //if (Player.EnergyCur <= 0)
        //    GameOver();

        if (timerCur >= timeForLevel)
        {
            GameOver();
        }
    }

    private void TimerManager()
    {
        timerCur += Time.deltaTime;
        if (timerFill != null)
            timerFill.fillAmount = timerCur / timeForLevel;
    }

    private void GameOver()
    {
        Time.timeScale = 0f;
        GameOverUI.SetActive(true);

    }
}
