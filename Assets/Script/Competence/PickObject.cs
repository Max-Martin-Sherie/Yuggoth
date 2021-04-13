using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickObject : PlayerCompetence
{
    [SerializeField] private string m_selectableObject = "Selectable";

    [SerializeField] private GameObject m_newParent;
    [SerializeField] private GameObject m_parent;

    private bool isGrabbing;

    Transform m_selection;

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
            if (Input.GetButton("Fire2"))
            {
                m_selection = hit.transform;
                m_selection.GetComponent<Rigidbody>().useGravity = false;
                if (m_selection.CompareTag(m_selectableObject))
                {
                    m_selection.parent = Camera.main.transform;
                    isGrabbing = true;
                }
                
            }
        }else if (isGrabbing)
        {
            if (Input.GetButton("Fire2"))
            {
                m_selection.parent = m_parent.transform;
                m_selection.GetComponent<Rigidbody>().useGravity = true;
                isGrabbing = false;
                m_selection = null;
            }
        }


    }



}
