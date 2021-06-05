using UnityEngine;
using UnityEngine.SceneManagement;

public class ConstantForceUp : MonoBehaviour
{
    [SerializeField] private float m_speed;

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
        if (m_rt.transform.position.y >= 3000f)
            SceneManager.LoadScene(0);
    }
}
