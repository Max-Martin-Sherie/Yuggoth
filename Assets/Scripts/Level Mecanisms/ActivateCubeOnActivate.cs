using UnityEngine;

public class ActivateCubeOnActivate : MonoBehaviour
{
       
    //Creating the variable that will contain the LaserReceptor script of the gameObject
    private ActivatorParent m_activatorScript;

    //Creating the variable that will contain the Material of the gameObject
    [SerializeField]private GameObject[] m_gameObjectsToActivate;
    
    //Creating the variable that will contain the initial color of the game object 
    private Color m_initialColor; 

    /// <summary>
    ///  Start is called before the first frame update
    /// </summary>
    void Start()
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

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        if (m_activatorScript.m_enabled)
            foreach (GameObject obj in m_gameObjectsToActivate)
                obj.SetActive(true);
        else
            foreach (GameObject obj in m_gameObjectsToActivate)
                obj.SetActive(false);
    }
}
