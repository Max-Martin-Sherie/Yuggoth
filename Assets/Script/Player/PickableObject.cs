using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : MonoBehaviour
{
    public float m_pickUpRange = 20f;
    //Objet que ramasse le joueur
    public GameObject m_heldObj;

    #region private
    //force d'attraction
    [SerializeField] private float m_moveForce = 150f;
    //définition des parent dans la hiérarchie
    [SerializeField] private Transform m_newParent;
    [SerializeField] private Transform m_oldParent;
    #endregion


    void Update()
    {
        //condition grab & drop
        if (Input.GetButtonDown("Fire2"))
        {
            //Si le joueur ne tiens un objet
            if (m_heldObj == null)
            {
                //Récupération de l'obj par raycast
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, m_pickUpRange))
                {
                    PickUpObject(hit.transform.gameObject);
                }
            }
            //Si le joueur tiens un objet
            else if(m_heldObj != null)
            {
                DropObject();
            }
        }

        //Condition move objet
        if(m_heldObj != null)
        {
            MoveObject();
        }
    }

    private void MoveObject()
    {
        //Mouvement d'objet par le transform
        if(Vector3.Distance(m_heldObj.transform.position, m_newParent.position) > 0.1f)
        {
            Vector3 moveDir = m_newParent.position - m_heldObj.transform.position;
            m_heldObj.GetComponent<Rigidbody>().AddForce(moveDir * m_moveForce);
        }
    }

    private void PickUpObject(GameObject p_pickObj)
    {
        //récupération du rigidbody pour mouvoir l'objet pour modification
        Rigidbody objRb = p_pickObj.GetComponent<Rigidbody>();
        if (objRb)
        {
            m_newParent.transform.position = p_pickObj.transform.position + new Vector3(0, 1.2f,0);
            objRb.useGravity = false;
            objRb.drag = 10;

            objRb.transform.parent = m_newParent;
            m_heldObj = p_pickObj;
        }
    }

    private void DropObject()
    {
        //récupération du rigidbody pour réinitialiser les valeurs modifiées.
        Rigidbody heldRb = m_heldObj.GetComponent<Rigidbody>();
        heldRb.useGravity = true;
        heldRb.drag = 1;

        heldRb.transform.parent = m_oldParent;
        m_heldObj = null;
        m_newParent.transform.position = new Vector3(0, 0, 0);
    }


}
