using System.Collections;
using UnityEngine;

public class MoveSOTD : MonoBehaviour
{

    [SerializeField] private float m_speed;
    [SerializeField] private float m_fadeSpeed = 0.5f;
    [SerializeField][Range(0,1)] private float m_maxAlpha = 0.8f;
    [SerializeField] private AudioClip[] m_soundClips;

    private AudioSource m_as;
    [SerializeField] SkinnedMeshRenderer m_mr;
    
    // Start is called before the first frame update
    void Start()
    {
        m_as = GetComponent<AudioSource>();
        m_as.clip = m_soundClips[Random.Range(0, m_soundClips.Length)];
        
        if (Random.Range(0, 10) == 1)
            m_as.Play();

        StartCoroutine(FadeInOut());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.forward * (m_speed * -Time.deltaTime));
    }

    IEnumerator FadeInOut()
    {
        bool fading = true;
        while (fading)
        {
            for (int i = 0; i < m_mr.materials.Length; i++)
            {
                Color target = new Color(m_mr.materials[i].color.r, m_mr.materials[i].color.g, m_mr.materials[i].color.b, m_maxAlpha);
                m_mr.materials[i].color = Color.Lerp(m_mr.materials[i].color, target, Time.deltaTime*m_fadeSpeed * (1 + Mathf.Abs(i-1) * 4f));
            }

            if (m_maxAlpha - m_mr.materials[1].color.a <= 0.05f) fading = false;
            
            yield return new WaitForSeconds(Time.deltaTime);
        }
        
        while (!fading)
        {

            for (int i = 0; i < m_mr.materials.Length; i++)
            {
                Color target = new Color(m_mr.materials[i].color.r, m_mr.materials[i].color.g, m_mr.materials[i].color.b, 0);
                m_mr.materials[i].color = Color.Lerp(m_mr.materials[i].color, target, Time.deltaTime*m_fadeSpeed );
            }

            if (m_mr.materials[1].color.a <= 0.05f) Destroy(gameObject);

            yield return new WaitForSeconds(Time.deltaTime);
        }
        
    }
}
