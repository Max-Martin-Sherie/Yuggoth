using UnityEngine;

public class RespawnCubeOnDelegate : MonoBehaviour
{
    [SerializeField] private RespawnPoint m_respawnPoint;

    private Vector3 m_respawnPosition;
    private Quaternion m_respawnRotation;

    private Rigidbody m_rb;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (m_respawnPoint == null) Gizmos.DrawRay(transform.position,Vector3.up * 10000f);;
    }

    private void Start()
    {
        if(m_respawnPoint == null) Debug.LogWarning($"Please affect a respawn point to {gameObject.name}");
        
        m_rb = GetComponent<Rigidbody>();
        m_respawnPosition = transform.position;
        m_respawnRotation = transform.rotation;

        m_respawnPoint.OnRespawnCall += RespawnCube;
    }

    public void RespawnCube()
    {
        m_rb.velocity = Vector3.zero;
        m_rb.angularVelocity = Vector3.zero;
        transform.position = m_respawnPosition;
        transform.rotation = m_respawnRotation;
    }
}
