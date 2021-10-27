using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Menu : MonoBehaviour
{
    private ControlsInput controls;
    public static bool isPaused = false;
    public GameObject pauseMenu;
    public EnergyBar energyBar;
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
        Conductor.Instance.musicSource.Pause();
        AudioListener.pause = true;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        energyBar.gameObject.SetActive(true);
        isPaused = false;
        Conductor.Instance.musicSource.Play();
        AudioListener.pause = false;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Restart()
    {		
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(PlayerPrefs.GetInt("lastLevel"));
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
