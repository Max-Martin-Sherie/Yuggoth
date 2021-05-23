using UnityEngine;

/// <summary>
/// This is an example script for the use of the l_laserHit boolean which is publicly contained in the LaserReceptor class
/// It is NECESARY to have a LaserReceptor Script on this GameObject
/// </summary>

public class ChangeColorOnActivate : MonoBehaviour
{
    
    //Creating the variable that will contain the LaserReceptor script of the gameObject
    private LaserReceptor m_receptorScript;

    //Creating the variable that will contain the Material of the gameObject
    private Material m_meshMaterial;
    
    //Creating the variable that will contain the initial color of the game object 
    private Color m_initialColor; 

    /// <summary>
    ///  Start is called before the first frame update
    /// </summary>
    void Start()
    {
        //Getting the mesh renderer of the game object
        m_meshMaterial = GetComponent<MeshRenderer>().material;
        
        //Getting the initial color of the game object 
        m_initialColor = m_meshMaterial.color; 
        
        
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
        // Using the public boolean m_enabled of the LaserReceptor class
        //if it is true change the color to black 
        if (m_receptorScript.m_enabled) m_meshMaterial.color = Color.black; 
        //if it is false change the color to the original color of the object 
        else m_meshMaterial.color = m_initialColor;
    }
}
