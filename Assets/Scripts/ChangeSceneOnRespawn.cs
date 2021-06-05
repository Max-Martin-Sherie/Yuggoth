using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneOnRespawn : MonoBehaviour
{
    [SerializeField] private Respawn m_player;

    private bool m_set = false;

    private void OnTriggerEnter(Collider p_other)
    {
        if(!m_set && p_other.transform.gameObject.layer == LayerMask.NameToLayer("Player")){
            m_player.OnRespawn += SwitchScene;
            
            m_set = true;
        }
    }

    private void SwitchScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
}
