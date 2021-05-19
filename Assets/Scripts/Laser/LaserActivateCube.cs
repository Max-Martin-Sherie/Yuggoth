using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserActivateCube : MonoBehaviour
{
       
    //Creating the variable that will contain the LaserReceptor script of the gameObject
    private LaserReceptor m_receptorScript;

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
        bool lrIsPresent = TryGetComponent<LaserReceptor>(out m_receptorScript);
        
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
        if (m_receptorScript.m_laserHit)
            foreach (GameObject obj in m_gameObjectsToActivate)
                obj.SetActive(true);
        else
            foreach (GameObject obj in m_gameObjectsToActivate)
                obj.SetActive(false);
    }
}
