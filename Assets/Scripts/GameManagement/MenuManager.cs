using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button quitGameButton;

    void Start()
    {
        newGameButton.onClick.AddListener(StartGame);
        quitGameButton.onClick.AddListener(QuitGame);
    }

    public void StartGame()
    {
        GameOverData.Reset(); 
        SceneManager.LoadScene("LoadingScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
