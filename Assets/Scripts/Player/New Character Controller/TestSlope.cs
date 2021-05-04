using System;
using UnityEngine;

public class TestSlope : MonoBehaviour
{
    
    CharacterController m_cr = null;
    public Vector3 m_hitNormal = Vector3.zero;
    public Vector3 m_velocity = Vector3.zero;

    [SerializeField] private float m_moveSpeed = 1;
    [SerializeField] private float m_jumpHeight;
    [SerializeField] private float m_slideAcceleration;
    [SerializeField] float m_gravity = 9.81f;
    [SerializeField] private bool m_useUnityPhysicsGravity;
    [SerializeField][Range(0.5f,1)] private float m_drag;
    
    
    [SerializeField] private bool m_showForces;

    public bool m_isOnSlope;

    private void OnDrawGizmos()
    {
        CharacterController cr = GetComponent<CharacterController>();
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position + Vector3.down * (cr.height / 2 - cr.radius/2) , cr.radius+0.05f);
    }

    // Start is called before the first frame update
    void Start()
    {
        m_cr = GetComponent<CharacterController>();

        if (m_useUnityPhysicsGravity) m_gravity = Physics.gravity.y;
    }

    // Update is called once per frame
    void Update()
    {
        float radius = m_cr.radius;
        bool grounded = Physics.CheckSphere(transform.position + Vector3.down* (m_cr.height / 2 - radius/2), radius+0.05f,~LayerMask.GetMask("Player"));
        if(m_showForces) DebugForce(); //Show the forces applied to the player in the form of debug Rays

        Debug.Log(grounded);
        
        //Cheking if the player is on a slope
        m_isOnSlope = (Vector3.Angle(Vector3.up, m_hitNormal) > m_cr.slopeLimit);
        
        
        //If the player is grounded resetting the gravity and giving the player the possibility to jump
        if (!m_isOnSlope && grounded)
        {
            if (m_velocity.y < 0) m_velocity.y = 0;
            if(Input.GetButton("Jump"))m_velocity.y = Mathf.Sqrt(m_jumpHeight * -2f * m_gravity);
        }
        
        //if the player is on a slope apply the sliding force
        if (m_isOnSlope && grounded)
        {
            float yOpposite = 1f - m_hitNormal.y;
            
            m_velocity.x += (yOpposite * m_hitNormal.x) * m_slideAcceleration;
            m_velocity.z += (yOpposite * m_hitNormal.z) * m_slideAcceleration;
        }
        
        //Update Gravity
        m_velocity.y += m_gravity * Time.deltaTime;
        
        //https://www.youtube.com/watch?v=ybljJGA1ksk
        
        //Adding the players input
        Vector3 inputMove = transform.forward * Input.GetAxis("Vertical");
        inputMove += transform.right * Input.GetAxis("Horizontal");

        //inputMove = Vector3.Normalize(inputMove);
        inputMove*= (m_moveSpeed * 0.1f);

        inputMove = Vector3.ProjectOnPlane(inputMove, m_hitNormal);
        
        
        m_velocity += inputMove;

        //Apply velocity
        m_cr.Move( m_velocity * Time.deltaTime);

        //Apply Drag
        m_velocity.x *= m_drag;
        m_velocity.z *= m_drag;
        
        if(grounded)m_velocity.y *= m_drag;
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
