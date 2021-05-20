using UnityEngine;

public class SwitchStateOnButtonPress : MonoBehaviour
{
    [SerializeField][Tooltip("Drag and drop a gameobject that has button component here")] private Button[] m_button; //Fetching the button
    [SerializeField] private bool m_setActive = true;
    
    private void Start()
    {
        gameObject.SetActive(!m_setActive);

        foreach (Button btn in m_button)
        {
            btn.OnButtonPress += EnableGameobject; //Adding the enable mesh renderer to the on button press delegate
            btn.OnButtonRelease += DisableGameobject; 
        }
    }

    private void EnableGameobject()
    {
        
        gameObject.SetActive(m_setActive);
    }
    private void DisableGameobject()
    {
        bool noneActive = true;

        foreach (Button btn in m_button)
        {
            if (btn.m_triggered)
            {
                noneActive = false;
                break;
            }
        }
        
        if(noneActive)gameObject.SetActive(!m_setActive);
    }
}
