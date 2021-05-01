using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserChangeColor : MonoBehaviour
{
    
    private LaserReceptor m_receptorScript;

    private Material m_meshMaterial;

    // Start is called before the first frame update
    void Start()
    {
        m_meshMaterial = GetComponent<MeshRenderer>().material;
        m_receptorScript = GetComponent<LaserReceptor>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_receptorScript.m_laserHit) m_meshMaterial.color = Color.green;
        else m_meshMaterial.color = Color.gray;
        
        
    }
}
