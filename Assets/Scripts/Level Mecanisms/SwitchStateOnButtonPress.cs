using UnityEngine;

public class SwitchStateOnButtonPress : MonoBehaviour
{
    [SerializeField][Tooltip("Drag and drop a gameobject that has button component here")] private ActivatorParent[] m_activators; //Fetching the button
    [SerializeField] private bool m_setActive = true;
    
    private void Start()
    {
        gameObject.SetActive(!m_setActive);

        foreach (ActivatorParent btn in m_activators)
        {
            btn.OnActivate += EnableGameobject; //Adding the enable mesh renderer to the on button press delegate
            btn.OnRelease += DisableGameobject; 
        }
    }

    private void EnableGameobject()
    {
        gameObject.SetActive(m_setActive);
    }
    private void DisableGameobject()
    {
        bool noneActive = true;

        foreach (ActivatorParent activator in m_activators)
        {
            if ( activator.m_enabled)
            {
                noneActive = false;
                break;
            }
        }
        
        if(noneActive)gameObject.SetActive(!m_setActive);
    }
}
