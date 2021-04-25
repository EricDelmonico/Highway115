using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Menu : MonoBehaviour
{
    private ControlsInput controls;
    public bool isPaused = false;
    public GameObject pauseMenu;
    public EnergyBar energyBar;
    //public GameObject gameOverMenu;
    //public GameObject instructionsMenu;
    //public GameObject enemiesMenu;
    //public GameObject controlsMenu;
    // public TextMeshProUGUI scoreText;

    private void Awake()
    {
        controls = new ControlsInput();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Start()
    {
        controls.Player.PauseGame.performed += _ => DeterminePaused();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    // Start is called before the first frame update
    void Update()
    {
		
    }

    void DeterminePaused()
    {
        if (isPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        energyBar.gameObject.SetActive(false);
        isPaused = true;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        //instructionsMenu.SetActive(false);
        //enemiesMenu.SetActive(false);
        //controlsMenu.SetActive(false);
        Time.timeScale = 1f;
        energyBar.gameObject.SetActive(true);
        isPaused = false;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Restart()
    {		
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

    public void QuitGame()
    {
        Application.Quit();
    }
}
