using UnityEngine;

/// <summary>
/// This simple function will enable and disable the serialized laserSource scripts
/// </summary>

public class SetLazerReceptorActiveOnActivate : MonoBehaviour
{
    //The receptor script
    private ActivatorParent m_activatorScript;

    //The array of scripts that will be enbaled or disabled
    [SerializeField]private LaserSource[] m_laserSourcesToActivate;
    
    //A verification optimiser
    bool m_hitAlready = true;
    
    private void Start()
    {
        //Checking if the player has a LaserReceptor script
        bool lrIsPresent = TryGetComponent<ActivatorParent>(out m_activatorScript);
        
        //warning the level designer if he doesn't have a LaserReceptor script
        if (!lrIsPresent)
        {
            Debug.LogWarning($"Hey There is no LineRenderer on {gameObject.name} please add one!");
            gameObject.SetActive(false);
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //On the Receptor being hit by a laser
        if (!m_hitAlready && m_activatorScript.m_enabled)
        {
            foreach (LaserSource laserSource in m_laserSourcesToActivate) laserSource.enabled = true;
            m_hitAlready = true;
        }
        
        //On the Receptor not being hit by a laser
        else if (m_hitAlready && !m_activatorScript.m_enabled)
        {
            foreach (LaserSource laserSource in m_laserSourcesToActivate) laserSource.enabled = false;
            m_hitAlready = false;
        }
    }
}
