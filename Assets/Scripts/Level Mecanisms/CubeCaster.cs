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
    
    //Initial position de l'objet a activer 
    private Vector3 m_targetInitialPos;
    
    //Offset pour desactiver la source 
    private Vector3 m_offsetPos = new Vector3(0, 1, 0);


    //Object que le poteau active
    [Tooltip("Glisser ici l'objet a activer ou a desactiver par le poteau")]
    [SerializeField] private GameObject m_targetObject;
    
    //Distance du raycast 
    [SerializeField] private float m_raycastDistance = 5.0f;

    private void Start()
    {
        m_targetInitialPos = m_targetObject.transform.position; 
    }

    // Update is called once per frame
    void Update()
    {
        //Appel de la fonction qui active ou desactive l'objet cible 
        ActivateOrDesactivateTargetObject();
        //Premier raycast a partir du cube lanceur (cube du bas) 
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
                    //Si la deuxieme cible est atteinte alors le raycast est complet
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

    /// <summary>
    /// Fonction qui gere l'apparition ou la disparition (selon la valeur du boolean unactive) de l'objet target 
    /// </summary>
    void ActivateOrDesactivateTargetObject()
    {
        //Si le raycast est complet, l'objet s'active 
        if (m_raycastCompleted)
        {
            m_targetObject.transform.position = m_targetInitialPos;
            if (m_unactiveObject)
            {
                m_targetObject.transform.position = m_targetInitialPos;
            }
        }
        //Si le raycast est complet et que le bool unactive est true alors l'objet se desactive 
        else if (!m_raycastCompleted)
        {
            m_targetObject.transform.position = m_targetInitialPos + m_offsetPos;
            if (m_unactiveObject)
            {
                m_targetObject.transform.position = m_targetInitialPos + m_offsetPos;
            }
        }
    }
}
