using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSlope : MonoBehaviour
{
    CharacterController m_cr = null;
    Vector3 m_hitNormal = Vector3.zero;
    Vector3 m_moveDir = Vector3.zero;
    Vector3 m_velocity = Vector3.zero;

    bool m_isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        m_cr = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float slopeLimit = m_cr.slopeLimit;
        m_moveDir = Vector3.zero;

        if (!m_isGrounded)
        {

            m_moveDir.x += (1f - m_hitNormal.y) * m_hitNormal.x * (1f);
            m_moveDir.z += (1f - m_hitNormal.y) * m_hitNormal.z * (1f);

            //m_moveDir.x += (1f - m_hitNormal.y) * m_hitNormal.x * slide speed
        }
    }

    void FixedUpdate()
    {
        Debug.DrawRay(transform.position, m_moveDir * m_moveDir.magnitude);

        m_velocity.y += Physics.gravity.y * Time.deltaTime;
        m_cr.Move(m_velocity + m_moveDir);

        m_isGrounded = (Vector3.Angle(Vector3.up, m_hitNormal) <= m_cr.slopeLimit);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        m_hitNormal = hit.normal;
    }

}
