using System;
using UnityEngine;
using System.Collections.Generic;
public class CubeCaster : MonoBehaviour
{

    //Objets qui doivent recevoir le raycast 
    [Tooltip("Glisser ici le cube du milieu")]
    [SerializeField] private GameObject m_firstTargetRaycast;
    [Tooltip("Glisser ici le cube du haut")]
    [SerializeField] private GameObject m_secondTargetRaycast;

    //Creation du delegate qui sera appele lorsque le raycast sera complet
    public delegate void RaycastDelegate();
    public static event RaycastDelegate onRaycastComplete;

    //Bool qui gere si le poteau active ou desactive un objet 
    [Tooltip("Si ce boolean est false alors le poteau va activer le target objet, si ce boolean est true alors le poteau va desactiver le target objet ")]
    [SerializeField] private bool m_unactiveObject; 
    
    //Bool qui indique si les objets sont dans la bonne position ou non
    [HideInInspector]
    public bool m_raycastCompleted; 
    
    //Object que le poteau active
    [Tooltip("Glisser ici l'objet a activer ou a desactiver par le poteau")]
    [SerializeField] private GameObject m_targetObject;
    
    //Distance du raycast 
    [SerializeField] private float m_raycastDistance = 5.0f;

    // Update is called once per frame
    void Update()
    {
        //Fonction qui gere l'apparition et la disparition de l'objet si le raycast est complete ou non
        ActivateOrDesactivateTargetObject();
        //Premier raycast a partir du premier cube (cube du bas) 
        Debug.DrawRay(transform.position, Vector3.up * m_raycastDistance, Color.green);
        if (Physics.Raycast(transform.position, Vector3.up, out RaycastHit objectHit, m_raycastDistance))
        {
            Debug.DrawRay(transform.position, Vector3.up * m_raycastDistance, Color.red);
            //Si l'objet est le bon, alors le raycast est transmis à la seconde cible 
            if (objectHit.transform.gameObject == m_firstTargetRaycast)
            {
                Debug.DrawRay(transform.position, Vector3.up * m_raycastDistance, Color.magenta);
                //Raycast depuis la premiere cible pour atteindre la deuxieme cible
                if (Physics.Raycast(m_firstTargetRaycast.transform.position, Vector3.up, out RaycastHit secondObjectHit,
                    m_raycastDistance))
                {
                    //Si la deuxieme cible est atteinte alors le raycast est complete
                    if (secondObjectHit.transform.gameObject == m_secondTargetRaycast)
                    {
                        Debug.DrawRay(transform.position, Vector3.up * m_raycastDistance, Color.blue);
                        m_raycastCompleted = true;
                        if(onRaycastComplete!=null)onRaycastComplete();
                    }
                }
            }

        }
        
    }

    void ActivateOrDesactivateTargetObject()
    {
        if (m_raycastCompleted)
        {
            m_targetObject.SetActive(true);
            if (m_unactiveObject)
            {
                m_targetObject.SetActive(false);
            }
        }
        else if (!m_raycastCompleted)
        {
            m_targetObject.SetActive(false);
            if (m_unactiveObject)
            {
                m_targetObject.SetActive(true);
            }
        }
    }
}
