using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : PlayerCompetence
{
    public float m_pickUpRange = 20f;
    [SerializeField] private float m_moveForce = 150f;

    [SerializeField] private Transform m_newParent;
    [SerializeField] private Transform m_oldParent;

    private GameObject m_heldObj;
    

    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            if (m_heldObj == null)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, m_pickUpRange))
                {
                    PickUpObject(hit.transform.gameObject);
                }
                Debug.Log("essaie");
            }
            else if(m_heldObj != null)
            {
                DropObject();
            }
        }

        if(m_heldObj != null)
        {
            MoveObject();
        }
    }

    private void MoveObject()
    {
        if(Vector3.Distance(m_heldObj.transform.position, m_newParent.position) > 0.1f)
        {
            Vector3 moveDir = m_newParent.position - m_heldObj.transform.position;
            m_heldObj.GetComponent<Rigidbody>().AddForce(moveDir * m_moveForce);
        }
        //m_heldObj.transform.rotation = m_newParent.transform.rotation;
    }

    private void PickUpObject(GameObject p_pickObj)
    {
        if (p_pickObj.GetComponent<Rigidbody>())
        {
            //m_newParent.transform.position = p_pickObj.transform.position;
            Rigidbody objRb = p_pickObj.GetComponent<Rigidbody>();
            objRb.useGravity = false;
            objRb.drag = 10;

            objRb.transform.parent = m_newParent;
            m_heldObj = p_pickObj;
            Debug.Log("j'ai");
        }
    }

    private void DropObject()
    {
        Rigidbody heldRb = m_heldObj.GetComponent<Rigidbody>();
        heldRb.useGravity = true;
        heldRb.drag = 1;

        heldRb.transform.parent = m_oldParent;
        m_heldObj = null;
        Debug.Log("j'ai pas");
    }
}
