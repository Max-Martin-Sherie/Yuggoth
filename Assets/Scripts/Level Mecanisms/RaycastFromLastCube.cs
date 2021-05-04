using UnityEngine;

public class RaycastFromLastCube : MonoBehaviour
{
    //Object which have to get the raycast
    [SerializeField] private GameObject m_targetRayCast; 
    
    //Distance of raycast 
    [SerializeField] private float m_raycastDistance = 10.0f;
    
    //Oofset for raycast 
    //[SerializeField] private Vector3 m_raycastOffset = new Vector3(0.25f, 0, -0.25f);

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, Vector3.up * m_raycastDistance, Color.green);
        if (Physics.Raycast(transform.position, Vector3.up, out RaycastHit objectHit, m_raycastDistance))
        {
            Debug.DrawRay(transform.position, Vector3.up * m_raycastDistance, Color.red);
            if (objectHit.transform.gameObject == m_targetRayCast)
            {
                Debug.Log("i'm hiting the right target");
            }
        }
    }
}
