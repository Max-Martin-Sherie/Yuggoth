using UnityEngine;

/// <summary>
/// Cette classe typiquement afféctée au joueur sert à attirer et manipuler des gameobject du tag "pickuable"
/// Cette classe nécessite un collider et un rigidbody
/// </summary>

public class PickupRb : MonoBehaviour
{
    //Définition de la distance minimal pour récupérer un objet
    [SerializeField][Tooltip("The minimum range at which the object has to be so that player can pick it up")] float m_minRange = 1;
    [SerializeField][Tooltip("The distance at which the object will be held after pickup")] float m_pickupDistance = 1;
    
    [SerializeField][Tooltip("The force with which the item will be pulled in")] private float m_moveForce = 150f;
    [SerializeField][Tooltip("The speed at which the item will rotate to face the player")] private float m_rotateSpeed = 10f;
    [SerializeField][Range(0,100)][Tooltip("the maximum velocity at which the cube will go and the velocity at which it will drop when blocked")] private float m_maxVelocity = 4;
    //Définition du parent dans lequel le gameObject va être transféré 
    [SerializeField][Tooltip("The ampty gameObject that will hold the cube")] private Transform m_newParent;
    [SerializeField][Range(-1.5f,1.5f)][Tooltip("the offset of the height at which the cube will be held")] private float m_yOffset = -0.2f;
    
    [SerializeField][Tooltip("Switch the pick object command between hold and toggle")] bool m_mouseHold = false;
    
    //Définition de l'ancien parent dans lequel le gameObject sera renvoyé
    private Transform m_oldParent;

    //The gameObject that will be held
    [HideInInspector]public GameObject m_heldObj = null;

    //The main camera
    private Camera m_camera;

    //The sceen object
    private GameObject m_seenObject;

    private void OnDrawGizmosSelected()
    {
        //Coloring the gizmos
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position+transform.forward * m_pickupDistance + transform.up * m_yOffset,0.2f);
    }

    private void Start()
    {
        m_camera = Camera.main; //Getting the main camera
        m_oldParent = transform.parent; //Getting the old parent

        m_newParent.transform.position = transform.position + transform.forward * m_pickupDistance + Vector3.up *m_yOffset; //Setting the position of the new parent
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
                GameObject obj = InteractRaycast.m_hitTarget.collider.gameObject;
                if (m_seenObject != obj)
                {
                    m_seenObject = obj;
                }
            }
            //Actions à réaliser quand le joueur ne tiens pas d'objet et qu'il n'observe plus un objet "Pickupable"
            else
            {
                if (m_seenObject)
                {
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
            if (Input.GetButton("Fire1"))
            {
                
            }
        }
    }

    /// <summary>
    /// Fonction permettant le déplacement d'objet dans l'espace par rapport au parent dans la hiérarchie
    /// </summary>
    private void MoveObject()
    {
        m_heldObj.transform.rotation = Quaternion.Lerp(m_heldObj.transform.rotation,transform.rotation,m_rotateSpeed * Time.deltaTime);
        Rigidbody rb = m_heldObj.GetComponent<Rigidbody>();

        Vector3 targetPosition = m_newParent.position;
        
        if(Vector3.Distance(m_heldObj.transform.position, targetPosition) > 0.1f)
        {
            Vector3 moveDir = targetPosition - m_heldObj.transform.position;

            Vector3 newForce = moveDir * (m_moveForce * Time.deltaTime);
            
            if (newForce.magnitude > m_maxVelocity && Physics.Raycast(m_heldObj.transform.position, moveDir, 1f))
            {
                DropObject();
                return;
            } if (newForce.magnitude > m_maxVelocity)
                newForce = Vector3.Normalize(newForce)*m_maxVelocity;

            
            
            rb.AddForce(newForce, ForceMode.Impulse);
        }
    }

    /// <summary>
    /// Récupération de données et définition du nouveau parent de l'objet déplaçable
    /// </summary>
    /// <param name="p_pickObj"></param>
    private void PickUpObject(GameObject p_pickObj)
    {
        Rigidbody objRb = p_pickObj.GetComponent<Rigidbody>();
        
        Vector3 ogPos = p_pickObj.transform.position;
        
        
        if (Vector3.Distance(m_camera.transform.position, m_newParent.transform.position) < m_minRange)
            m_newParent.transform.position = new Vector3(ogPos.x+m_minRange,m_newParent.transform.position.y,ogPos.z+m_minRange);
        
        objRb.useGravity = false;
        objRb.drag = 10;
        objRb.freezeRotation = true;
        
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
        Rigidbody objRb = m_heldObj.GetComponent<Rigidbody>();
        objRb.useGravity = true;
        objRb.drag = 1;
        objRb.freezeRotation = false;

        objRb.transform.SetParent(m_oldParent);
        m_heldObj = null;
        objRb.velocity = Vector3.zero;
        
        
        InteractRaycast.m_interacting = false;
    }
}