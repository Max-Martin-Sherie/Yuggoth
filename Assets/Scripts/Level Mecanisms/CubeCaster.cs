using System;
using UnityEngine;

public class CubeCaster : MonoBehaviour
{
    //Initial positon of the cylinder 
    private Vector3 m_initialPosition; 
    
    //Objects which have to get the raycast
    [SerializeField] private GameObject m_firstTargetRaycast;
    [SerializeField] private GameObject m_secondTargetRaycast;
    
    //object it activates 
    [SerializeField] private GameObject m_objectActivated;
    //Distance of raycast 
    [SerializeField] private float m_raycastDistance = 5.0f;

    private void Start()
    {
        m_initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Raycast from the cube, children of the cylinder 
        Debug.DrawRay(transform.position, Vector3.up * m_raycastDistance, Color.green);
        if (Physics.Raycast(transform.position, Vector3.up, out RaycastHit objectHit, m_raycastDistance))
        {
            Debug.DrawRay(transform.position, Vector3.up * m_raycastDistance, Color.red);
            if (objectHit.transform.gameObject == m_firstTargetRaycast)
            {
                Debug.DrawRay(transform.position, Vector3.up * m_raycastDistance, Color.magenta);
                if (Physics.Raycast(m_firstTargetRaycast.transform.position, Vector3.up, out RaycastHit secondObjectHit,
                    m_raycastDistance))
                {
                    
                    if (secondObjectHit.transform.gameObject == m_secondTargetRaycast)
                    {
                        Debug.DrawRay(transform.position, Vector3.up * m_raycastDistance, Color.blue);
                        m_objectActivated.SetActive(true);
                    }
                }
                
            }
        }
    }

    private void GetToInitialPosition()
    {
        
    }
}
