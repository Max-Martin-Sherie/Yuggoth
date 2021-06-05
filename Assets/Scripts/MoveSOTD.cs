using System.Collections;
using UnityEngine;

public class MoveSOTD : MonoBehaviour
{

    [SerializeField] private float m_speed;
    [SerializeField] private float m_fadeSpeed = 0.5f;
    [SerializeField][Range(0,1)] private float m_maxAlpha = 0.8f;
    [SerializeField] private AudioClip[] m_soundClips;

    private AudioSource m_as;
    private MeshRenderer m_mr;
    
    // Start is called before the first frame update
    void Start()
    {
        m_as = GetComponent<AudioSource>();
        m_mr = GetComponent<MeshRenderer>();
        m_as.clip = m_soundClips[Random.Range(0, m_soundClips.Length)];
        
        if (Random.Range(0, 20) == 1)
            m_as.Play();

        StartCoroutine(FadeInOut());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.forward * m_speed * -Time.deltaTime);
    }

    IEnumerator FadeInOut()
    {
        bool fading = true;
        while (fading)
        {
            Color target = new Color(m_mr.materials[0].color.r, m_mr.materials[0].color.g, m_mr.materials[0].color.b, m_maxAlpha);
            m_mr.materials[0].color = Color.Lerp(m_mr.materials[0].color, target, Time.deltaTime*m_fadeSpeed);

            if (m_maxAlpha - m_mr.materials[0].color.a <= 0.01f) fading = false;
            
                yield return new WaitForSeconds(Time.deltaTime);
        }
        
        while (!fading)
        {
            Color target = new Color(m_mr.materials[0].color.r, m_mr.materials[0].color.g, m_mr.materials[0].color.b, 0);
            m_mr.materials[0].color = Color.Lerp(m_mr.materials[0].color, target, Time.deltaTime*m_fadeSpeed);
            
            if(m_mr.materials[0].color.a <= 0.01f)
                Destroy(gameObject);
            
                yield return new WaitForSeconds(Time.deltaTime);
        }
        
    }
}
