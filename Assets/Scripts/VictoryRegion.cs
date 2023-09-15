using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryRegion : MonoBehaviour
{
    [SerializeField] private GameObject victoryScreen;
    private bool gameIsWon = false;

    private void Awake()
    {
        if (victoryScreen != null)
        {
            victoryScreen.SetActive(false);
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R) && gameIsWon)
        {
            SceneManager.LoadScene("StartScreen");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>() != null)
        {
            if (victoryScreen != null)
                victoryScreen.SetActive(true);

            gameIsWon = true;
            Time.timeScale = 0;
        }
    }
}
