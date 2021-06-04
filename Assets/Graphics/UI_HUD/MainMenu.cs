using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static bool m_GameIsPaused = false;
    private bool m_SubMenuIsActive = false;
    public GameObject m_SubMenu; 
    public GameObject m_pauseMenuUI;
  
    
    
        private void Update()
    {
        //Disable escape key when the submenu is open
        if (m_SubMenu.active)
        {
            m_SubMenuIsActive = true;
        }
        else if (!m_SubMenu.active)
        {
            m_SubMenuIsActive = false;
        }
        
        //Use escape key for pause menu
        if (Input.GetKeyDown(KeyCode.Escape) && !m_SubMenuIsActive)
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

    //Lauch Game
   public void PlayGame()
   {
       SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
   }
   
   //Exit Game
   public void QuitGame()
   {
       Debug.Log("QUIT !");
       Application.Quit();
       
   }
   
   //Play Game
   public void Resume()
   {
       	m_pauseMenuUI.SetActive(false);
       	Time.timeScale = 1f;
       	m_GameIsPaused = false; 
		Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
		
		Debug.Log("hey");
   }

   //Open the pause Menu
   public void Pause()
   {
      	m_pauseMenuUI.SetActive(true);
      	Time.timeScale = 0f;
       	m_GameIsPaused = true;
		Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
   }

   //Open the Main Menu
   public void LoadMenu()
   {
       m_GameIsPaused = false;
       Time.timeScale = 1f;
       SceneManager.LoadScene("Main_Menu");
   }

   public void LoadSubMenu()
   {
       m_pauseMenuUI.SetActive(false);
       m_SubMenu.SetActive(true);
       
   }
}

