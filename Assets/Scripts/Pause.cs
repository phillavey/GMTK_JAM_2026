using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    private InputAction pause;
    private bool isPaused = false;

    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject ingameUI;

    void Start()
    {
        pause = InputSystem.actions.FindAction("Pause");
        Time.timeScale = 0;
        ingameUI.SetActive(false);
        pauseMenuUI.SetActive(true);
        isPaused = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (pause.WasPressedThisFrame())
        {
            if (!isPaused) { PauseGame(); } else { ResumeGame(); }
        }
    }

    public void PauseGame()
    {
        Debug.Log("pause!");
        Time.timeScale = 0;
        ingameUI.SetActive(false);
        pauseMenuUI.SetActive(true);
        isPaused = true;
    }

    public void ResumeGame()
    {
        Debug.Log("resume!");
        Time.timeScale = 1;
        pauseMenuUI.SetActive(false);
        ingameUI.SetActive(true);
        isPaused = false;
    }
}

