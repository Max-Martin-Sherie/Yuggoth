using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickObject : MonoBehaviour
{
    [SerializeField] private GameObject m_parent;

    [SerializeField] private Material m_nonSelectedMat;
    [SerializeField] private Material m_selectedMat; 

    private bool isGrabbing;

    Transform m_selection;
    private Rigidbody m_selectionRb;

    private void Start()
    {
        isGrabbing = false;
    }
    void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit) && !isGrabbing)
        {
            if (Input.GetMouseButtonDown(0))
            {
                m_selection = hit.transform;
                if (m_selection.TryGetComponent(out Rigidbody rigidbody))
                {
                    m_selection.GetComponent<MeshRenderer>().material = m_selectedMat;
                    m_selectionRb = rigidbody;
                    m_selectionRb.useGravity = false;
                    m_selectionRb.constraints = RigidbodyConstraints.FreezePosition;
                    m_selection.parent = Camera.main.transform;
                    isGrabbing = true;
                }
                
                
                
            }
        }else if (isGrabbing)
        {
            if (Input.GetMouseButtonUp(0))
            {
                Quaternion baseRotation = new Quaternion(0,0,0,0);
                m_selection.transform.localRotation = baseRotation;
                m_selection.parent = m_parent.transform;
                m_selection.GetComponent<MeshRenderer>().material = m_nonSelectedMat;
                m_selectionRb.useGravity = true;
                m_selectionRb.constraints = RigidbodyConstraints.None;
                isGrabbing = false;
                m_selection = null;
            }
        }


    }

    
}
