using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Respawn : MonoBehaviour
{
    [HideInInspector]public Transform m_respawnTransform;
    [HideInInspector]public float m_respawnHeight = -5f;

    [SerializeField] private RawImage m_blackscreen;
    [SerializeField][Range(2f,15f)] private float m_respawnSpeed = 0.02f;

    [SerializeField] private AudioSource m_as;

    private CameraController m_cameraController;
    private PlayerMove m_pm;

    private bool m_respawning = false;
    
    public delegate void OnRespawnRun();
    public OnRespawnRun OnRespawn;
    
    private void Start()
    {
        m_respawnTransform = GetComponent<Spawn>().m_spawnPoints[0]._transform;
        m_cameraController = GetComponent<CameraController>();
        m_pm = GetComponent<PlayerMove>();

        StartCoroutine(CheckIfShouldRespawn());
    }

    IEnumerator CheckIfShouldRespawn()
    {
        while (true)
        {
            if(!m_respawning && transform.position.y < m_respawnHeight)
                StartCoroutine(RespawnPlayer());
            yield return new WaitForSeconds(0.2f);
        }
    }

    
    [ContextMenu("Respawn")]
    void StartRespawn()
    {
        StartCoroutine(RespawnPlayer());
    }
    IEnumerator RespawnPlayer()
    {
        m_respawning = true;
        while (m_blackscreen.color.a < .958f)
        {
            m_blackscreen.color = Color.Lerp(m_blackscreen.color, Color.black,m_respawnSpeed * Time.deltaTime );
            yield return new WaitForSeconds(Time.deltaTime);
        }

        m_blackscreen.color = Color.black;
        
        transform.position = m_respawnTransform.position;
        m_cameraController.m_xRotation = m_respawnTransform.rotation.eulerAngles.x;
        m_cameraController.m_yRotation = m_respawnTransform.rotation.eulerAngles.y;

        m_as.Play();
        
        if (OnRespawn != null) OnRespawn();
            
        m_pm.m_velocity = Vector3.zero;
        
        Color targetcolor = new Color(0, 0, 0, 0);
        
        while (m_blackscreen.color.a > .002f)
        {
            m_blackscreen.color = Color.Lerp(m_blackscreen.color, targetcolor,m_respawnSpeed  * Time.deltaTime);
            
            yield return new WaitForSeconds(Time.deltaTime);
        }
        
        m_blackscreen.color = targetcolor;

        m_respawning = false;
    }
}
