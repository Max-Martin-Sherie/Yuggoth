using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool m_GameIsPaused = false;
    public GameObject m_pauseMenuUI;

    private void Update()
    {
        //Use escape key for pause menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (m_GameIsPaused)
            {
                Resume();
            }
            else if (!m_GameIsPaused)
            {
                Pause();
            }
        }
    }

    //Continue the game
    public void Resume()
    {
        m_pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        m_GameIsPaused = false; 
    }

    //Open the pause Menu
    public void Pause()
    {
        m_pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        m_GameIsPaused = true;
    }

    //Open the Main Menu
    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main_Menu");
    }
    
}
