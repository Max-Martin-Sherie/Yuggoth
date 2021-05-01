using UnityEngine;

public class LaserChangeColor : MonoBehaviour
{
    
    private LaserReceptor m_receptorScript;

    private Material m_meshMaterial;

    // Start is called before the first frame update
    void Start()
    {
        m_meshMaterial = GetComponent<MeshRenderer>().material;
        
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

    // Update is called once per frame
    void Update()
    {
        if (m_receptorScript.m_laserHit) m_meshMaterial.color = Color.green;
        else m_meshMaterial.color = Color.gray;
        
        
    }
}
