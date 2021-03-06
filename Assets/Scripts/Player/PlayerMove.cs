using System;
using UnityEngine;
/// <summary>
/// This class allows the player to control a gameObject using the horizontal and vertical axis
/// </summary>
public class PlayerMove : MonoBehaviour
{
    CharacterController m_cr = null;
    [HideInInspector]public Vector3 m_velocity = Vector3.zero;
    [HideInInspector]public bool m_canJump = true;

    [SerializeField][Tooltip("The player's movement speed")]public float m_moveSpeed = 1;
    [SerializeField][Tooltip("The player's jump height in meters")] private float m_jumpHeight;
    [SerializeField][Tooltip("The speed at which the player will fall while sliding")] private float m_slideAcceleration;
    [SerializeField][Tooltip("The acceleration of the gravity applied top the player")] float m_gravity = 9.81f;
    [SerializeField][Tooltip("If enabled the previously set gravity value will be replaced by the in game gravity")] private bool m_useUnityPhysicsGravity;
    [SerializeField][Tooltip("The multiplier that will be affected to the acceleration every frame")][Range(0.5f,1)] private float m_drag;
    [SerializeField][Tooltip("The range at which the player will cast a sphere to check if he is grounded")][Range(.1f,.5f)] private float m_groundCheckRange = .3f;

    [SerializeField] private bool m_joMode = false;

    [HideInInspector]
    public bool m_grounded;
    
    private bool m_noClip = false;
    [SerializeField]private float m_noclipSpeed = 20f;
    
    private Vector3 m_inputMove = Vector3.zero;
    
    // Start is called before the first frame update
    void Start()
    {
        m_cr = GetComponent<CharacterController>();//Fetching the character controller

        if (m_useUnityPhysicsGravity) m_gravity = Physics.gravity.y; //if the m_useUnityPhysicsGravity bool is enabled then replacing the in game gravity by the default unity one
    }

    // Update is called once per frame
    void FixedUpdate() {

        float radius = m_cr.radius; //The radius of the character controller
        
        //The point from where the ground checking SphereCast will be sent
        //It will be sent from the bottom of the capsule collider of the character controller downwards
        Vector3 origin = transform.position + Vector3.down * (m_cr.height / 2f - radius ); 

        //Sending out the sphereCast to check the position of the player
        m_grounded = Physics.SphereCast(origin,radius, Vector3.down, out RaycastHit hit, m_groundCheckRange);
        
        Vector3 hitNormal = hit.normal; //The ground hit normal
        
        //using the character controller's slope limit to check if the player is on a slope or not
        bool onSlope = (Vector3.Angle(Vector3.up, hitNormal) > m_cr.slopeLimit);

        //Making the player slide down a slope if he is on one
        if (onSlope)
        {
            float yOpposite = 1f - hitNormal.y; //avoiding a recurring operation
            
            //Moving the player down
            m_velocity.x += (yOpposite * hitNormal.x) * m_slideAcceleration;
            m_velocity.z += (yOpposite * hitNormal.z) * m_slideAcceleration;
        }

        bool aircontrol = (m_joMode && m_grounded) || !m_joMode;

        if(aircontrol)
        {
            //Fetching the player's input 
            m_inputMove = transform.forward * Input.GetAxis("Vertical");
            m_inputMove += transform.right * Input.GetAxis("Horizontal");

            //Adding the speed plus a small multiplier to it
            m_inputMove *= (m_moveSpeed * 0.1f);
        }

        if (m_grounded && !Input.GetButton("Jump"))
        {
            m_canJump = true;
        }
        
        //Projecting the player's movement direction onto a plane to avoid stepping
        if(!onSlope && m_grounded){
            m_inputMove = Vector3.ProjectOnPlane(m_inputMove, hitNormal);
            
            //Fetching the jump key and jumping
            if(Input.GetButton("Jump") && m_canJump)
            {
                m_velocity.y = Mathf.Sqrt(m_jumpHeight * -2f * m_gravity); //Jump height is accurate
                m_canJump = false;
            }
        }else m_velocity.y += m_gravity * Time.deltaTime; //Adding gravity
        
        //Adding the player's input to the global velocity
        m_velocity += m_inputMove;

        if (m_noClip)
        {
            m_velocity = Vector3.zero;
            
            m_velocity = transform.forward * Input.GetAxis("Vertical");
            m_velocity += transform.right * Input.GetAxis("Horizontal");

            if(Input.GetButton("Jump"))
                m_velocity += Vector3.up;
            
            if(Input.GetKey(KeyCode.LeftControl))
                m_velocity -= Vector3.up;

            m_velocity *= m_noclipSpeed;
        }
        
        //Applying the global velocity
        m_cr.Move( m_velocity * Time.deltaTime);
        
        //Applying Drag to the horizontal axis
        m_velocity.x *= m_drag;
        m_velocity.z *= m_drag;
        
        //Applying drag to the vertical axis if the player is grounded and on a slope
        if(m_grounded && !onSlope) m_velocity.y *= m_drag;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N) && Input.GetKey(KeyCode.LeftShift))
        {
            m_noClip = !m_noClip;
            GetComponent<ShakeCameraOnHighVSpeed>().enabled = !m_noClip;
            GetComponent<CameraSway>().enabled = !m_noClip;
        }
    }
}