using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float m_moveSpeed = 5f;
    [SerializeField] private float m_jumpForce = 7f;
    [SerializeField] private float m_playerHeight = 1f;


    [Header("Multiplier")]
    [SerializeField] private float m_groundControlMultiplier = 10f;
    [SerializeField] private float m_airControlMultiplier = 0.4f;
    [SerializeField] private float m_jumpForceMultiplier = 10f;

    [Header("Drag")]
    public float m_groundDrag = 6f;
    public float m_airDrag = 2f;

    [Header("Ground Check")]
    [SerializeField]
    private Transform m_groundCheck;
    [SerializeField]
    private float m_groundDistance = 0.4f;
    [SerializeField]
    private LayerMask m_groundMask;

    private bool m_isGrounded;

    private Vector3 m_moveDir;
    private Vector3 m_slopeMoveDir;

    private Rigidbody m_playerRb;

    private RaycastHit m_slopeHit;

    private OxygeneTimer m_oxygeneTimer;


    private bool OnSlope() 
    {
        if (Physics.Raycast(transform.position, Vector3.down, out m_slopeHit, m_playerHeight / 2 + 0.5f))
        {
            if(m_slopeHit.normal != Vector3.up)
                return true;
            else
                return false;
        }
        return false;
    }

    void Start()
    {
        m_playerRb = GetComponent<Rigidbody>();
        m_playerRb.freezeRotation = true;
        m_oxygeneTimer = GetComponent<OxygeneTimer>();
    }

    void Update()
    {
        if (!m_oxygeneTimer.m_stopTimer)
        {
            m_isGrounded = Physics.CheckSphere(m_groundCheck.position, m_groundDistance, m_groundMask);

            MyInput();
            ControlDrag();

            if (m_isGrounded && Input.GetAxisRaw("Jump") > 0)
                Jump();

            m_slopeMoveDir = Vector3.ProjectOnPlane(m_moveDir, m_slopeHit.normal);
        }
        else
        {
            m_moveDir = new Vector3( 0f, 0f, 0f);
        }
    }

    void MyInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        m_moveDir = transform.forward * vertical + transform.right * horizontal;
    }

    void Jump()
    {
        m_playerRb.velocity = new Vector3(m_playerRb.velocity.x, 0, m_playerRb.velocity.z);
        m_playerRb.AddForce(transform.up * m_jumpForce * m_jumpForceMultiplier, ForceMode.Impulse);
    }

    void ControlDrag()
    {
        if (m_isGrounded)
            m_playerRb.drag = m_groundDrag;
        else
            m_playerRb.drag = m_airDrag;
}

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        if (m_isGrounded && !OnSlope())
        {
            m_playerRb.AddForce(m_moveDir * m_moveSpeed * m_groundControlMultiplier, ForceMode.Acceleration);
        }
        else if (m_isGrounded && OnSlope())
        {
            m_playerRb.AddForce(m_slopeMoveDir * m_moveSpeed * m_groundControlMultiplier, ForceMode.Acceleration);
        }
        else if(!m_isGrounded)
        {
            m_playerRb.AddForce(m_moveDir * m_moveSpeed * m_airControlMultiplier, ForceMode.Acceleration);
        }
    }
}
