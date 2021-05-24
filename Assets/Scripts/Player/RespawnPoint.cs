using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    [SerializeField] private Transform m_respawnPoint;
    [SerializeField] private GameObject m_player;

    
    
    private Respawn m_playerRespawn;
    private PlayerMove m_playerMove;
    
    private bool m_set = false;
    
    public Respawn.OnRespawnRun OnRespawnCall;
    
    private void Start()
    {
        m_playerRespawn = m_player.GetComponent<Respawn>();
        OnRespawnCall += DropHeldObject;
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider p_other)
    {
        if(!m_set && p_other.transform.gameObject.layer == LayerMask.NameToLayer("Player")){
            m_playerRespawn.m_respawnTransform = m_respawnPoint;
            m_playerRespawn.OnRespawn += OnRespawnCall;
            m_set = true;

        }
    }

    public void DropHeldObject()
    {
        PickupRb pickupRb = m_player.GetComponent<PickupRb>();

        if(pickupRb.m_heldObj != null) m_player.GetComponent<PickupRb>().DropObject();
    }
}