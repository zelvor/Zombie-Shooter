using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Threading.Tasks;

public class LoadingManager : MonoBehaviour
{
    public Slider progressBar;

    void Start()
    {
        LoadNextScene();
    }

    async void LoadNextScene()
    {
        var operation = SceneManager.LoadSceneAsync("GameScene");
        while (!operation.isDone)
        {
            progressBar.value = operation.progress;
            await Task.Yield();
        }
    }
}