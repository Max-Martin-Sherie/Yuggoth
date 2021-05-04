using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This class will control a character GameObject based off of the inputs
/// This class Includes Jump
/// Note that any slide interactions isn't coded here and has to be handled with physics materials 
/// </summary>
public class CharaControllerRB : MonoBehaviour
{
    //This class uses RigiBody based movement
    [HideInInspector]public Rigidbody m_rb;
    
    //Global Variables that can be changed by the user in the unity inspector
    [Header("Movement")]
    [SerializeField][Tooltip("The movement speed of the player")][Range(0,30)]public float m_moveSpeed;
    [SerializeField][Tooltip("the jump height in meters")][Range(0,5)]public float m_jumpHeight;
    [SerializeField][Tooltip("the level of slobe at which the player registers himself as not grounded anymore")][Range(0,90)] private short m_slideAngle;

    [Header("Drag")]
    [Tooltip("The level of control over his movement the character has in both states (in air & grounded)")]
    [SerializeField][Range(0,5)]private float m_groundDrag;
    [Tooltip("The level of control over his movement the character has in both states (in air & grounded)")]
    [SerializeField][Range(0,0.5f)]private float m_airDrag;

    [HideInInspector]public bool m_canJump = true;
    
        public bool m_leftRightFrozen = false;
        
    #region private & hide
    
    private bool m_hasJumped = false;
    public bool m_grounded;
    private Vector3 m_groundNormal;
    private int m_collsionCount;
    private float m_colliderWidth;
    
    /**/
    
    //(to be removed before launch) the position at which the player starts tz
    
    private Vector3 m_startPos;
    /**/
    
    #endregion

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        //Fetching the rigidbody
        m_rb = GetComponent<Rigidbody>();

        m_colliderWidth = GetComponent<CapsuleCollider>().radius;
        
        /**/
        //(to be removed before launch) fetching the players spawn tz
        m_startPos = transform.position;
        /**/
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void FixedUpdate()
    {
        /**/
        //(to be removed before launch) teleports the player to spawn when you press "i" tz
        
        if (Input.GetKey("i")) transform.position = m_startPos;
        /**/
        
        //Using a raycast to check if the player is grounded

        //Setting the direction of the player
        Vector3 moveDirection = SetDirection();
        
        //Checking if the player can jump. To do so the player must :
        // Be grounded
        // Isn't holding in the jump key since his last jump
        // Is pressing the jump key
        if (m_grounded && m_canJump && !m_hasJumped && Input.GetAxis("Jump") != 0)
        {
            //Making the GameObject jump
            Jump(m_jumpHeight);
        }

        //Resetting the m_hasjumped boolean if the player:
        // Is grounded
        // The variable is true
        // If the player isn't holding down the jump key
        if (m_grounded && m_hasJumped && Input.GetAxis("Jump") == 0) m_hasJumped = false;
        
        //Updating the drag of the player
        SetDrag(m_grounded);

        //If the player isn't on level ground
        if (m_grounded && m_groundNormal != Vector3.up)
        {
            LowerGarvityOnSlopes();
            //Change his movement direction to be projected n the slope
            moveDirection = Vector3.ProjectOnPlane(moveDirection, m_groundNormal);
            
            //Normalizing the vector for better use later
            Vector3.Normalize(moveDirection);
        }

        //Adds the force to the rigidbody to move the GameObject
        MoveCharacter(moveDirection);
        
    }

    /// <summary>
    /// This function ower the player gravitational pull based on the slope on which he is
    /// </summary>
    private void LowerGarvityOnSlopes()
    {
        m_rb.AddForce(-Physics.gravity* (Vector3.Angle(m_groundNormal,Vector3.up)/180));
    }
    private void OnCollisionEnter(Collision p_other)
    {
        //Checking if the player is grounded based on the right collider
        foreach (ContactPoint contactPoint in p_other.contacts)
        {
            if (Vector2.Distance(new Vector2(contactPoint.point.x, contactPoint.point.z) , new Vector2(transform.position.x,transform.position.z)) <= m_colliderWidth +0.1f)
            {
                m_collsionCount=1;
                
                Vector3 collisionNormal = contactPoint.normal;
                if (Vector3.Angle(collisionNormal, Vector3.up) < m_slideAngle)
                {
                    m_groundNormal = collisionNormal;
                    m_grounded = true;
                    return;
                }
            }
        }
    }

    private void OnCollisionStay(Collision p_other)
    {
        //Making sure that code know that the player is still grounded
        if(!m_grounded || m_collsionCount==0)
        {
            foreach (ContactPoint contactPoint in p_other.contacts)
            {
                if (Vector2.Distance(new Vector2(contactPoint.point.x, contactPoint.point.z),
                    new Vector2(transform.position.x, transform.position.z)) <= m_colliderWidth + 0.1f)
                {
                    m_collsionCount = 1;
                    m_grounded = true;
                }
            }
        }
    }

    private void OnCollisionExit(Collision p_other)
    {
        //Ungrounding the player on a jump
        m_collsionCount--;
        if(m_collsionCount < 0) m_collsionCount = 0;

        if (m_collsionCount <= 0) m_grounded = false;
    }

    /// <summary>
    /// Uses the players input and the rotation of the gameobject to determine which direction to go in
    /// </summary>
    /// <returns> the direction of the GameObject</returns>
    private Vector3 SetDirection()
    {
        //Fetching the player's analogical inputs and applying them to variables
        float horizontal;
        if(m_leftRightFrozen)horizontal = 0;
        else horizontal= Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        
        //Getting the transform for later use
        Transform t = transform;

        //Returning the direction of the player based off of his inputs
        return (t.forward * vertical) + (t.right * horizontal);
    }
    
    /// <summary>
    /// Makes the GameObject jump based of off the wanted jump height
    /// </summary>
    /// <param name="p_jumpHeight">the wanted jump height</param>
    private void Jump(float p_jumpHeight)
    {
        //Fetching the velocity of the player
        Vector3 v = m_rb.velocity;
        
        //Determining the necessary velocity to jump up to the wanted height based off calculus to determine the required vertical force. Source : "dude trust me ☻"
        v = new Vector3(v.x, Mathf.Sqrt((p_jumpHeight * -2) * (Physics.gravity.y)), v.z);
        
        //Setting the new velocity for the jump
        m_rb.velocity = v;
        
        //Alerting the code that the player has already jumped
        m_hasJumped = true;
    }

    /// <summary>
    /// Sets the drag of the RigidBody according to whether or not it is grounded
    /// </summary>
    /// <param name="p_grounded">a boolean that asks "is the player grounded"</param>
    private void SetDrag(bool p_grounded)
    {
        //Getting the value of the RigidBody's drag
        float drag = m_rb.drag;
        
        //Setting a value to the RigidBody's drag based of whether or not the p_grounded variable is true
        if (p_grounded && drag != m_groundDrag) m_rb.drag = m_groundDrag;
        else if(!p_grounded && drag != m_airDrag) m_rb.drag = m_airDrag;
    }

    /// <summary>
    /// Move the GameObject if he isn't grounded
    /// </summary>
    /// <param name="p_moveDirection">The direction in which we want the GameObject to move</param>
    /// <param name="p_grounded">Is the player grounded</param>
    private void MoveCharacter(Vector3 p_moveDirection)
    {
        Vector3 vector = p_moveDirection* m_moveSpeed;
        //Applies the force to the RigidBody of the GameObject
        if(m_grounded)m_rb.AddForce(vector , ForceMode.Acceleration);
        else m_rb.AddForce(vector * m_airDrag , ForceMode.Acceleration);
    }
}
