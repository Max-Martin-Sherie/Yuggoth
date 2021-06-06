using UnityEngine;
using UnityEngine.SceneManagement;

public class ConstantForceUp : MonoBehaviour
{
    [SerializeField] private float m_speed;
    [SerializeField] private float m_exitHeight = 3500f;

    private RectTransform m_rt;
    
    // Start is called before the first frame update
    void Start()
    {
        m_rt = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        m_rt.Translate(new Vector2(0,1) * m_speed * Time.deltaTime);
        if (m_rt.transform.position.y >= m_exitHeight)
            SceneManager.LoadScene(0);
    }
}
