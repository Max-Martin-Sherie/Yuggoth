using System.Collections;
using UnityEngine;
/// <summary>
/// Cette classe typiquement afféctée au joueur sert à attirer et manipuler des gameobject du tag "pickupable"
/// Cette classe nécessite un collider et un rigidbody
/// </summary>
public class PickupRb : MonoBehaviour
{
    [SerializeField][Tooltip("The minimum range at which the object has to be so that player can pick it up")] float m_minRange = 1;
    [SerializeField][Tooltip("The distance at which the object will be held after pickup")] float m_pickupDistance = 1;
    [SerializeField][Tooltip("The distance at which the object hold will be locked")][Range(0,1)] float m_holdLockDistance = 0.1f;
    [SerializeField][Tooltip("The distance at which the object hold will be locked")][Range(0,1)] float m_setParentDistance = 0.1f;
    
    private float m_holdLockSave;
    
    [SerializeField][Tooltip("The time the pickup mode will take to change between toggle and hold")][Range(0,5)] float m_switchHoldModeDelay = 1f;
    [SerializeField][Tooltip("The force with which the item will be pulled in")] private float m_moveForce = 150f;
    [SerializeField][Tooltip("The speed at which the item will rotate to face the player")] private float m_rotateSpeed = 10f;
    [SerializeField][Range(0,1000)][Tooltip("the maximum velocity at which the cube will go and the velocity at which it will drop when blocked")] private float m_maxVelocity = 4;
    [SerializeField][Tooltip("The empty gameObject that will hold the cube")] private Transform m_newParent;
    [SerializeField][Range(-1.5f,1.5f)][Tooltip("the offset of the height at which the cube will be held")] private float m_yOffset = -0.2f;
    bool m_mouseHold = false; //Switch the pick object command between hold and toggle

    [HideInInspector]public GameObject m_heldObj; //The gameObject that will be held

    private Camera m_camera; //The main camera
    private GameObject m_seenObject; //The seen object
    
    private GameObject m_oldparent; //The seen object

    private void OnDrawGizmosSelected()
    {
        Vector3 position = transform.position + transform.forward * m_pickupDistance + transform.up * m_yOffset;
        
        //Coloring the gizmos
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(position,m_setParentDistance);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(position,m_holdLockDistance);
    }

    private void Start()
    {
        m_holdLockSave = m_holdLockDistance; //Setting the hold lock save
        
        m_camera = Camera.main; //Getting the main camera

        m_newParent.transform.position = transform.position + transform.forward * m_pickupDistance + Vector3.up *m_yOffset; //Setting the position of the new parent
        
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"),LayerMask.NameToLayer("HeldCube"));
    }

    void Update()
    {
        bool availableTarget = false; //Bool determining if the player is aiming at a movable object

        //Checking if the player is aiming at an available object
        if(!m_heldObj)availableTarget = InteractRaycast.m_hitSomething && InteractRaycast.m_hitTarget.collider.gameObject.layer == LayerMask.NameToLayer("Pickupable");

        //Actions à réaliser quand le joueur ne tiens pas d'objet et qu'il observe l'espace autour de lui
        if(!m_heldObj){
            //Actions à réaliser quand le joueur ne tiens pas d'objet et qu'il observe un objet "Pickupable"
            if (availableTarget)
            {
                GameObject obj = InteractRaycast.m_hitTarget.transform.gameObject; //Fetching the targeted gameobject
                //Checking if the player's targeted object is the same as the pickuable object
                if (m_seenObject && m_seenObject != obj)
                {
                    //If it isn't switching the targeted objects value
                    m_seenObject = obj;
                }
            }
            //Actions à réaliser quand le joueur ne tiens pas d'objet et qu'il n'observe plus un objet "Pickupable"
            else if (m_seenObject)
                m_seenObject = null;
        }
        //Reacting to the fire 1 input
        if (Input.GetButtonDown("Fire1"))
        {
            m_mouseHold = false; //Reseting the mouse hold
            
            StartCoroutine(ChangePickupControl()); //Starting the change pickup control coroutine
            
            //If the player isn't holding an object..
            if (!m_heldObj)
            {
                // ...and he's looking at an available target..
                if (availableTarget)
                {
                    GameObject obj = InteractRaycast.m_hitTarget.transform.gameObject; //...Fetch the gameObject
                    if (obj != gameObject) PickUpObject(InteractRaycast.m_hitTarget.collider.gameObject); //...and pick it up
                }
            } else 
                DropObject(); //Dropping the object on second click if the player is in toggle
        }
        //If the player is holding an object...
        if(m_heldObj)
        {
            MoveObject();//...Move the object
            
            //... If the player isn't holding the button anymore and is in hold mode : drop the object
            if (m_mouseHold && !Input.GetButton("Fire1"))
                DropObject();
        }
    }

    /// <summary>
    /// A Coroutine to change the pickup method from hold to toggle based off of what the player is doing
    /// </summary>
    /// <returns></returns>
    IEnumerator ChangePickupControl()
    {
        yield return new WaitForSeconds(m_switchHoldModeDelay);
        if(Input.GetButton("Fire1"))m_mouseHold = true;
    }
    
    /// <summary>
    /// Fonction permettant le déplacement d'objet dans l'espace par rapport au parent dans la hiérarchie
    /// </summary>
    private void MoveObject()
    {
        m_heldObj.transform.rotation = Quaternion.Lerp(m_heldObj.transform.rotation,transform.rotation,m_rotateSpeed * Time.deltaTime);
        Rigidbody rb = m_heldObj.GetComponent<Rigidbody>();
        Vector3 targetPosition = m_newParent.position;
        
        if(Vector3.Distance(m_heldObj.transform.position, targetPosition) > m_holdLockDistance)
        {
            m_holdLockDistance = m_holdLockSave;
            Vector3 moveDir = targetPosition - m_heldObj.transform.position;
            Vector3 newForce = moveDir * m_moveForce;
            
            int layerMask =~ LayerMask.NameToLayer("Player");

            
            if (newForce.magnitude > m_maxVelocity && Physics.Raycast(m_heldObj.transform.position, moveDir,0.6f,layerMask))
            {
                DropObject();
                return;
            }
            rb.AddForce(newForce * Time.deltaTime, ForceMode.Impulse);
            
            Color rayColor = Color.green;
            if(newForce.magnitude > m_maxVelocity)rayColor = Color.red;
            
            Debug.DrawRay(m_heldObj.transform.position,moveDir,rayColor);
        }
        
        
        if(Vector3.Distance(m_heldObj.transform.position, targetPosition) < m_setParentDistance)
            m_heldObj.transform.SetParent(m_newParent.transform);
    }

    /// <summary>
    /// Récupération de données et définition du nouveau parent de l'objet déplaçable
    /// </summary>
    /// <param name="p_pickObj"> object à rammasser </param>
    private void PickUpObject(GameObject p_pickObj)
    {
        Rigidbody objRb = p_pickObj.GetComponent<Rigidbody>();
        
        Vector3 ogPos = p_pickObj.transform.position;

        if (Vector3.Distance(m_camera.transform.position, m_newParent.transform.position) < m_minRange)
            m_newParent.transform.position = new Vector3(ogPos.x+m_minRange,m_newParent.transform.position.y,ogPos.z+m_minRange);
        
        objRb.useGravity = false;
        objRb.drag = 10f;
        objRb.freezeRotation = true;
        
        m_heldObj = p_pickObj;
        if(p_pickObj.transform.parent)m_oldparent = p_pickObj.transform.parent.gameObject;
        m_heldObj.layer = LayerMask.NameToLayer("HeldCube");
        
        InteractRaycast.m_interacting = true;
    }

    /// <summary>
    /// Fonction pour lacher un objet
    /// </summary>
    public void DropObject()
    {
        m_heldObj.GetComponent<MeshRenderer>().material.SetFloat("_Metallic", 0f);
        Rigidbody objRb = m_heldObj.GetComponent<Rigidbody>();
        objRb.useGravity = true;
        objRb.drag = 1;
        objRb.freezeRotation = false;
        
        if(m_oldparent)
            m_heldObj.transform.SetParent(m_oldparent.transform);
        else
            m_heldObj.transform.parent = null;
        
        m_heldObj.layer = LayerMask.NameToLayer("Pickupable");
        
        m_heldObj = null;
        objRb.velocity = Vector3.zero;
        
        InteractRaycast.m_interacting = false;
    }
}