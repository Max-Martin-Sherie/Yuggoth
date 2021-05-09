using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Script pour ramasser, déplacer et lacher les objets déplaçable
/// </summary>

public class PickupRb : MonoBehaviour
{
    //Définition de la distance minimal pour récupérer un objet
    [SerializeField] float m_minRange = 1;
    
    [SerializeField] private float m_moveForce = 150f;
    [SerializeField][Range(0,100)] private float m_maxVelocity = 4;
    //Définition du parent dans lequel le gameObject va être transféré 
    [SerializeField] private Transform m_newParent;
    [SerializeField][Range(-1.5f,1.5f)][Tooltip("the offset of the height at which the cube will be held")] private float m_yOffset = -0.2f;
    [SerializeField][Range(0, 2)][Tooltip("the size of the steps at which the player will pickup the cube")] private float m_step = 0.5f;
    
    [SerializeField] bool m_mouseHold = false;
    
    //Définition de l'ancien parent dans lequel le gameObject sera renvoyé
    private Transform m_oldParent;

    public GameObject m_heldObj = null;

    private Camera m_camera;

    private GameObject m_seenObject;
    
    
    
    private void Start()
    {
        m_camera = Camera.main;
        m_oldParent = transform.parent;

    }

    void Update()
    {
        //Vérifie si le Raycast touche quelque chose et, si oui, si ce quelque chose a pour Layer "Pickupable"
        bool target = InteractRaycast.m_hitSomething && InteractRaycast.m_hitTarget.collider.gameObject.layer == LayerMask.NameToLayer("Pickupable");

        //Actions à réaliser quand le joueur ne tiens pas d'objet et qu'il observe l'espace autour de lui
        if(!m_heldObj){
            //Actions à réaliser quand le joueur ne tiens pas d'objet et qu'il observe un objet "Pickupable"
            if (target)
            {
                if (m_seenObject != InteractRaycast.m_hitTarget.collider.gameObject)
                {
                    //Changing the metallic instead of the color material because i can't get the initial color of a pickable obj (Nono) 
                    if(m_seenObject) m_seenObject.GetComponent<MeshRenderer>().material.SetFloat("_Metallic", 0f);
                    m_seenObject = InteractRaycast.m_hitTarget.collider.gameObject;
                    m_seenObject.GetComponent<MeshRenderer>().material.SetFloat("_Metallic", 1f);
                }
            }
            //Actions à réaliser quand le joueur ne tiens pas d'objet et qu'il n'observe plus un objet "Pickupable"
            else
            {
                if (m_seenObject)
                {
                    m_seenObject.GetComponent<MeshRenderer>().material.SetFloat("_Metallic", 0f);
                    m_seenObject = null;
                }
            }
        }

        //Conditions en fonction de si le joueur doit garder Fire1 enfoncé ou non
        if (m_mouseHold && Input.GetButton("Fire1"))
        {
            if (!m_heldObj)
            {
                if (target)
                {
                    GameObject hitObject = InteractRaycast.m_hitTarget.collider.gameObject;
                    if (hitObject != gameObject) PickUpObject(hitObject);
                }
            }
        }else if(m_mouseHold && !Input.GetButton("Fire1"))
        {
            if(m_heldObj)
            {
                DropObject();
            }
        }
        
        if (!m_mouseHold && Input.GetButtonDown("Fire1"))
        {
            if (!m_heldObj)
            {
                
                if (target)
                {
                    GameObject hitObject = InteractRaycast.m_hitTarget.collider.gameObject;
                    if (hitObject != gameObject) PickUpObject(InteractRaycast.m_hitTarget.collider.gameObject);
                }
            }
            else if(m_heldObj)
            {
                DropObject();
            }
        }

        if(m_heldObj)
        {
            MoveObject();
        }
    }

    /// <summary>
    /// Fonction permettant le déplacement d'objet dans l'espace par rapport au parent dans la hiérarchie
    /// </summary>
    private void MoveObject()
    {
        //m_heldObj.GetComponent<MeshRenderer>().material.color = Color.green;
        if(Vector3.Distance(m_heldObj.transform.position, m_newParent.position) > 0.1f)
        {
            Vector3 moveDir = m_newParent.position - m_heldObj.transform.position;

            Vector3 newForce = moveDir * m_moveForce;

            if (newForce.magnitude > m_maxVelocity)
            {
                DropObject();
                return;
            }
            
            m_heldObj.GetComponent<Rigidbody>().AddForce(newForce);
        }

        Vector3 moveDirection = Vector3.Normalize(m_newParent.position - m_camera.transform.position);
        
        moveDirection.y = 0;
        Vector3 newPosition = m_newParent.position + moveDirection * (Input.mouseScrollDelta.y * m_step);
        
        float distance = Vector3.Distance(m_camera.transform.position, newPosition);
        
        if(distance > m_minRange && distance < InteractRaycast.m_range)m_newParent.position = newPosition;
    }

    /// <summary>
    /// Récupération de données et définition du nouveau parent de l'objet déplaçable
    /// </summary>
    /// <param name="p_pickObj"></param>
    private void PickUpObject(GameObject p_pickObj)
    {
        Rigidbody objRb = p_pickObj.GetComponent<Rigidbody>();
        
        //Debug.Log("PickUp");
        Vector3 ogPos = p_pickObj.transform.position;
        m_newParent.transform.position = new Vector3(ogPos.x, m_camera.transform.position.y + m_yOffset,ogPos.z);

        if (Vector3.Distance(m_camera.transform.position, m_newParent.transform.position) < m_minRange)
            m_newParent.transform.position = new Vector3(ogPos.x+m_minRange,m_newParent.transform.position.y,ogPos.z+m_minRange);
        
        objRb.useGravity = false;
        objRb.drag = 10;
        
        objRb.transform.SetParent(m_newParent);
        m_heldObj = p_pickObj;

        InteractRaycast.m_interacting = true;
    }

    /// <summary>
    /// Fonction pour lacher un objet
    /// </summary>
    private void DropObject()
    {
       
        m_heldObj.GetComponent<MeshRenderer>().material.SetFloat("_Metallic", 0f);
        Rigidbody heldRb = m_heldObj.GetComponent<Rigidbody>();
        heldRb.useGravity = true;
        heldRb.drag = 1;

        heldRb.transform.SetParent(m_oldParent);
        m_heldObj = null;
        heldRb.velocity = Vector3.zero;
        m_newParent.transform.position = m_camera.transform.position;
        
        
        InteractRaycast.m_interacting = false;
    }
}