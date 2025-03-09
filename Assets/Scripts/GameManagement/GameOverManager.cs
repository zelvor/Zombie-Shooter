using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public TextMeshProUGUI resultText;
    [SerializeField] private Button backToMenuButton;

    void Start()
    {
        bool isVictory = GameOverData.IsVictory();
        resultText.text = isVictory ? "Victory!" : "Lose!";
        backToMenuButton.onClick.AddListener(OnBackToMenuClicked);
    }

    private void OnBackToMenuClicked()
    {
        GameOverData.Reset(); 
        SceneManager.LoadScene("MenuScene");
    }
}