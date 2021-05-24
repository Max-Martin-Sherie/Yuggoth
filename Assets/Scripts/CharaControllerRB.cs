using UnityEngine;


/// <summary>
/// This class will control a character GameObject based off of the inputs
/// This class Includes Jump
/// Note that any slide interactions isn't coded here and has to be handled with physics materials 
/// </summary>
public class CharaControllerRB : MonoBehaviour
{
    //This class uses RigiBody based movement
    private Rigidbody m_rb;
    
    //Global Variables that can be changed by the user in the unity inspector
    [Header("Movement")]
    [SerializeField][Tooltip("The movement speed of the player")][Range(0,30)]private float m_moveSpeed;
    [SerializeField][Tooltip("the range at which the player will register that he is touching the ground(note : is independent of physics) (note : it is visualized by the red or green line under the player)")]
    [Range(1,2)]private float m_groundCheckRange;
    [SerializeField][Tooltip("the jump height in meters")][Range(0,5)]private float m_jumpHeight;
    [SerializeField][Tooltip("the level of slobe at which the player registers himself as not grounded anymore")][Range(0,90)] private short m_slideAngle;

    [Header("Drag")]
    [Tooltip("The level of control over his movement the character has in both states (in air & grounded)")]
    [SerializeField][Range(0,5)]private float m_groundDrag;
    [Tooltip("The level of control over his movement the character has in both states (in air & grounded)")]
    [SerializeField][Range(0,1)]private float m_airDrag;
    private bool m_hasJumped = false;


    /**/
    
    //(to be removed before launch) the position at which the player starts tz
    
    private Vector3 m_startPos;
    /**/
    
    /// <summary>
    /// OnDrawGizmoSelected is called when gizmos are enabled in the viewport and it is selected in the inspector
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        //Creation of a Vector3 that will be the source of all the rays 
        Vector3 source = transform.position;
        
        //The base color of the ground checker debug ray
        Color rayColor = Color.red;
        
        //The conditional change of the color of the ground checker debug ray
        if(Physics.Raycast(source, -transform.up, m_groundCheckRange)) rayColor = Color.green;
        
        //Draw the ground checker ray
        Debug.DrawRay(source,-transform.up * m_groundCheckRange, rayColor);
        
        //Draw the rigidbody velocity ray
        if(m_rb)Debug.DrawRay(source , m_rb.velocity, Color.magenta);
    }

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        //Fetching the rigidbody
        m_rb = GetComponent<Rigidbody>();

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
        bool grounded = Physics.Raycast(transform.position,-transform.up, out RaycastHit groundHit, m_groundCheckRange);

        //Confirming that the surface on which the grounded player is isn't at too much of an angle
        if (grounded && Vector3.Angle(groundHit.normal, transform.up) > m_slideAngle) grounded = false;
        
        //Setting the direction of the player
        Vector3 moveDirection = SetDirection();
        
        //Checking if the player can jump. To do so the player must :
        // Be grounded
        // Isn't holding in the jump key since his last jump
        // Is pressing the jump key
        if (grounded && !m_hasJumped && Input.GetAxis("Jump") != 0)
        {
            //Making the GameObject jump
            Jump(m_jumpHeight);
            //Checking again if the GameObject is grounded
            grounded = Physics.Raycast(transform.position,-transform.up, out groundHit, m_groundCheckRange);
        }

        //Resetting the m_hasjumped boolean if the player:
        // Is grounded
        // The variable is true
        // If the player isn't holding down the jump key
        if (grounded && m_hasJumped && Input.GetAxis("Jump") == 0) m_hasJumped = false;
        
        //Updating the drag of the player
        SetDrag(grounded);

        //If the player isn't on level ground
        if (groundHit.normal != Vector3.up)
        {
            //Change his movement direction to be projected n the slope
            moveDirection = Vector3.ProjectOnPlane(moveDirection, groundHit.normal);
            
            //Normalizing the vector for better use later
            Vector3.Normalize(moveDirection);
        }

        //Adds the force to the rigidbody to move the GameObject
        MoveCharacter(moveDirection, grounded);
    }
    
    /// <summary>
    /// Uses the players input and the rotation of the gameobject to determine which direction to go in
    /// </summary>
    /// <returns> the direction of the GameObject</returns>
    private Vector3 SetDirection()
    {
        //Fetching the player's analogical inputs and applying them to variables
        float horizontal = Input.GetAxisRaw("Horizontal");
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
    private void MoveCharacter(Vector3 p_moveDirection, bool p_grounded)
    {
        //Applies the force to the RigidBody of the GameObject
        if (p_grounded) m_rb.AddForce(p_moveDirection * m_moveSpeed , ForceMode.Acceleration);
    }
}
