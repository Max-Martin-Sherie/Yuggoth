using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeToBlackFromPoint : MonoBehaviour
{
    [SerializeField]private Transform m_startPosition;
    [SerializeField]private Transform m_endPosition;
    [SerializeField]private float m_minstartDistance;
    [SerializeField]private float m_fadeSpeed = 2f;
    [FormerlySerializedAs("m_minEndDistane")] [SerializeField]private float m_minEndDistance = 2f;
    [SerializeField]private RawImage m_blackScreen;

    private float m_highestAlpha;
    
    private float m_distanceBetweenNodes;

    [SerializeField] private bool m_end = false;
    [SerializeField] private GameObject m_finalStatue;
    [SerializeField] private GameObject m_finalCamera;
    [SerializeField] private GameObject m_ship;
    [SerializeField] private AudioSource m_rockSound;
    
    private bool m_finished = false;
    // Start is called before the first frame update
    void Start()
    {
        m_distanceBetweenNodes = Vector3.Distance(m_startPosition.position, m_endPosition.position);
    }
    
    // Update is called once per frame
    void Update()
    {
        if (!m_finished && m_minstartDistance < Vector3.Distance(transform.position, m_startPosition.position))
        {
            Fade();
        }
    }

    private void Fade()
    {
        Color bsClr = m_blackScreen.color;
        
        //https://www.geogebra.org/calculator/uyjc6cyb
        float newAlpha = -Vector3.Distance(transform.position, m_endPosition.position) / (m_distanceBetweenNodes + m_minEndDistance) + 1f;

        newAlpha = Mathf.Clamp(newAlpha,0f,1f);

        if (newAlpha > m_highestAlpha) m_highestAlpha = newAlpha;
        
        newAlpha = Mathf.Lerp(bsClr.a,m_highestAlpha, m_fadeSpeed * Time.deltaTime);
        
        if(newAlpha > bsClr.a)m_blackScreen.color = new Color(bsClr.r,bsClr.g,bsClr.b,newAlpha);

        if(m_end)
        {
            float f = gameObject.GetComponent<PlayerMove>().m_moveSpeed;
            gameObject.GetComponent<PlayerMove>().m_moveSpeed = Mathf.Lerp(f, Mathf.Clamp((1f - newAlpha) * f +3f,0,f), Time.deltaTime);
            
            if(f <= 4f && !m_rockSound.isPlaying)m_rockSound.Play();
        }
        
        if (m_blackScreen.color.a > 0.958f)
        {
            if (!m_end) NextSCene();
            else TheEnd();
        }
    }

    private void NextSCene()
    {
        Color bsClr = m_blackScreen.color;
        m_blackScreen.color = new Color(bsClr.r,bsClr.g,bsClr.b,Mathf.Lerp(bsClr.a,1f, Time.deltaTime));
        m_finished = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

    private void TheEnd()
    {
        m_finalStatue.SetActive(true);
        m_finalStatue.transform.position = transform.position + Vector3.down;
        m_finalStatue.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f,90f,0f));

        GetComponentInChildren<Camera>().enabled = false;
        m_finalCamera.GetComponent<Camera>().enabled = true;
        m_finalCamera.GetComponent<AudioListener>().enabled = true;
        
        //GetComponent<AudioSource>().Stop();

        CameraPan cp = m_finalCamera.GetComponent<CameraPan>();
        cp.enabled = true;

        cp.m_focalPoint = transform.position;

        m_finalCamera.transform.position = transform.position;

        Destroy(m_ship);
        
        Destroy(gameObject);
        
        m_finished = true;
    }
}
