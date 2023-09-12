using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private Button startGameButton;
    [SerializeField] private TMP_Text startText;
    [SerializeField] private Image arrow;

    private void Awake()
    {
        startGameButton.interactable = false;
        startText.text = "<s>Start Game</s>";
        arrow.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Player p = other.GetComponent<Player>();
        if (p != null)
        {
            startGameButton.interactable = true;
            startText.text = "Start Game";
            p.canUseItems = false;
            arrow.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Player p = other.GetComponent<Player>();
        if (p != null)
        {
            startGameButton.interactable = false;
            startText.text = "<s>Start Game</s>";
            p.canUseItems = true;
            arrow.enabled = false;
        }
    }
}
