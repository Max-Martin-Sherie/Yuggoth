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
    
    // Start is called before the first frame update
    void Start()
    {
        m_distanceBetweenNodes = Vector3.Distance(m_startPosition.position, m_endPosition.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_minstartDistance < Vector3.Distance(transform.position, m_startPosition.position))
        {
            Fade();
        }
    }

    private void Fade()
    {
        Color bsClr = m_blackScreen.color;
        
        //https://www.geogebra.org/calculator/uyjc6cyb
        float newAlpha = -Vector3.Distance(transform.position, m_endPosition.position) / (m_distanceBetweenNodes - m_minEndDistance) + 1f;

        newAlpha = Mathf.Clamp(newAlpha,0f,1f);

        if (newAlpha > m_highestAlpha) m_highestAlpha = newAlpha;
        
        newAlpha = Mathf.Lerp(bsClr.a,m_highestAlpha, m_fadeSpeed * Time.deltaTime);
        
        if(newAlpha > bsClr.a)m_blackScreen.color = new Color(bsClr.r,bsClr.g,bsClr.b,newAlpha);

        if (m_blackScreen.color.a > 0.958f) NextSCene();
    }

    private void NextSCene()
    {
        Color bsClr = m_blackScreen.color;
        m_blackScreen.color = new Color(bsClr.r,bsClr.g,bsClr.b,Mathf.Lerp(bsClr.a,1f, Time.deltaTime));
        
        if(m_blackScreen.color.a >= 0.999f)SceneManager.LoadScene("next scene", LoadSceneMode.Single);
    }
}
