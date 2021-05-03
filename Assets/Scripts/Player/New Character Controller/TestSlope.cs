using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class TestSlope : MonoBehaviour
{
    
    CharacterController m_cr = null;
    Vector3 m_hitNormal = Vector3.zero;
    public Vector3 m_velocity = Vector3.zero;

    [SerializeField] private float m_moveSpeed = 1;
    [SerializeField] private float m_jumpHeight;
    [SerializeField] private float m_slideAcceleration;
    [SerializeField] float m_gravity = 9.81f;
    [SerializeField] private bool m_useUnityPhysicsGravity;
    [SerializeField][Range(0.5f,1)] private float m_drag;
    [SerializeField] private bool m_instantStop;
    
    
    [SerializeField] private bool m_showForces;

    public bool m_isOnSlope;

    // Start is called before the first frame update
    void Start()
    {
        m_cr = GetComponent<CharacterController>();

        if (m_useUnityPhysicsGravity) m_gravity = Physics.gravity.y;
        if (m_instantStop) m_drag = 0;
    }
    
    // Update is called once per frame
    void Update()
    {
        //If the player is grounded reset the hit normal
        if (!m_cr.isGrounded)
        {
            m_hitNormal = Vector3.up;
        }

        

        
        if(m_showForces) DebugForce(); //Show the forces applied to the player in the form of debug Rays

        
        //Cheking if the player is on a slope
        m_isOnSlope = (Vector3.Angle(Vector3.up, m_hitNormal) > m_cr.slopeLimit);
        
        
        //If the player is grounded resetting the gravity and giving the player the possibility to jump
        if (!m_isOnSlope && m_cr.isGrounded)
        {
            m_velocity.y = 0;
            if(Input.GetButton("Jump"))m_velocity.y = Mathf.Sqrt(m_jumpHeight * -2f * m_gravity);
        }
        
        //Update Gravity
        m_velocity.y += m_gravity * Time.deltaTime;
        
        //Adding the players input
        Vector3 inputMove = transform.forward * (Input.GetAxis("Vertical"));
        inputMove += transform.right * (Input.GetAxis("Horizontal"));

        //inputMove = Vector3.Normalize(inputMove);
        
        

        inputMove*= (m_moveSpeed * 0.1f);
        
        //if the player is on a slope apply the sliding force
        if (m_isOnSlope)
        {
            float yOpposite = 1f - m_hitNormal.y;
            
            m_velocity.x += (yOpposite * m_hitNormal.x) * m_slideAcceleration;
            m_velocity.z += (yOpposite * m_hitNormal.z) * m_slideAcceleration;
        }
        
        Debug.Log(inputMove);
        
        m_velocity += inputMove;
        
        //Apply velocity
        m_cr.Move( m_velocity * Time.deltaTime);

        //Apply Drag
        m_velocity.x *= m_drag;
        m_velocity.z *= m_drag;
        
        Debug.Log(new Vector2(m_velocity.x,m_velocity.z).magnitude);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        m_hitNormal = hit.normal;
    }
    
    /// <summary>
    /// A function to draw rays according to the different forces applied to the object
    /// </summary>
    private void DebugForce()
    {
            //Debug Rays
            Vector3 position = transform.position;
            Debug.DrawRay(position, Vector3.Scale(m_velocity , new Vector3(1, 0, 1)) * m_velocity.magnitude / 30, Color.blue); //horizontal velocity
            Debug.DrawRay(position, Vector3.down * m_velocity.y / 30, Color.red); //vertical velocity
            Debug.DrawRay(position, m_velocity * m_velocity.magnitude / 30, Color.green); //general velocity
    }

}
