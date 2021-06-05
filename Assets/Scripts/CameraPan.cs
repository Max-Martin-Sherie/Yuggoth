using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CameraPan : MonoBehaviour
{
    [SerializeField] private RawImage m_image;
    
    
    [SerializeField] private Transform[] m_targets;
    [SerializeField] private TextMeshProUGUI[] m_credits;
    
    [SerializeField] private float m_switchDistance;
    [SerializeField] private float m_moveSpeed;
    [SerializeField] private float m_creditStartDelay;
    [SerializeField] private float m_creditFadeSpeed;
    [SerializeField] private GameObject m_Ui;
    [SerializeField] private GameObject m_scrollCredits;
    [SerializeField]private RawImage m_logo;

    private Vector3 m_currentTarget;
    [SerializeField]private Vector3 m_secondFocalPoint;
    [SerializeField]private float m_targetSwitchSpeed;
    [SerializeField]private int m_switchIndex;

    
    private bool m_switched = false;

    public Vector3 m_focalPoint;
    
    [SerializeField]private bool m_start = false;

    [SerializeField]private float m_timeBtweenCredits = 3f;
    
    [SerializeField]private float m_timeBeforeScrollCredits = 5f;

    // Start is called before the first frame update
    void Start()
    {
        m_image.color = new Color(0,0,0,1);

        m_Ui.SetActive(false);
        
        StartCoroutine(StartFadeDelay());
        m_currentTarget = m_targets[0].position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        foreach (Transform trs in m_targets)
        {
            Gizmos.DrawWireSphere(trs.position, m_switchDistance);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(GetComponent<Camera>().enabled && m_start)
            m_image.color = Color.Lerp(m_image.color,new Color(0,0,0,0),Time.deltaTime*.1f);
    }

    IEnumerator StartFadeDelay()
    {
        yield return new WaitForSeconds(4f);

        GetComponent<AudioSource>().enabled = true;

        yield return new WaitForSeconds(3f);
        m_start = true;
        
        StartCoroutine(CameraTravelling());
        StartCoroutine(FadeCredits());
    }
    IEnumerator CameraTravelling()
    {
        int index = 0;

        Vector3 previousTarget =Vector3.zero;
        
        float previousSpeed = 0;
        float currentSpeed = m_moveSpeed;
        
        while (true)
        {
            transform.LookAt(m_focalPoint); 
            
            transform.position = Vector3.MoveTowards(transform.position,m_currentTarget, currentSpeed * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position,previousTarget, previousSpeed * Time.deltaTime);

            previousSpeed = Mathf.Lerp(previousSpeed, 0, Time.deltaTime * .5f);
            currentSpeed = Mathf.Lerp(currentSpeed, m_moveSpeed, Time.deltaTime * .5f);
            
            if (Vector3.Distance(m_currentTarget, transform.position) < m_switchDistance)
            {

                previousSpeed = m_moveSpeed;
                currentSpeed = 0;
                index++;
                if(index == m_switchIndex)StartCoroutine(SwitchTarget());
                
                if (index == m_targets.Length) index = 0;
                
                previousTarget = m_currentTarget;
                m_currentTarget = m_targets[index].position;
            }
            
            yield return (Time.deltaTime);
        }
    }

    IEnumerator SwitchTarget()
    {
        while (Vector3.Distance(m_focalPoint, m_secondFocalPoint) >0.1f)
        {
            m_focalPoint = Vector3.MoveTowards(m_focalPoint, m_secondFocalPoint, Time.deltaTime * m_targetSwitchSpeed);

            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
    
    IEnumerator FadeCredits()
    {
        yield return new WaitForSeconds(m_creditStartDelay);

        int index = 0;

        bool fadingIn = true;
        
        while (index < m_credits.Length)
        {
            if(fadingIn)
            {
                m_credits[index].alpha = Mathf.Lerp(m_credits[index].alpha, 1, Time.deltaTime * m_creditFadeSpeed);

                if (1 - m_credits[index].alpha < .05f) fadingIn = false;
            }
            else
            {
                m_credits[index].alpha = Mathf.Lerp(m_credits[index].alpha, 0, Time.deltaTime * m_creditFadeSpeed);
                if (m_credits[index].alpha < .05f)
                {
                    m_credits[index].alpha = 0;
                    fadingIn = true;
                    index++;
                    yield return new WaitForSeconds(m_timeBtweenCredits);
                }
            }
            
            yield return new WaitForSeconds(Time.deltaTime);
        }

        StartCoroutine(FadeToBlack());
    }

    IEnumerator FadeToBlack()
    {
        m_start = false;

        bool loop = true;
            
        while (loop)
        {
            m_logo.color = Color.Lerp(m_logo.color,Color.white,Time.deltaTime*.8f);

            if (1 - 
                m_logo.color.a < 0.01f)
                loop = false;
            
            yield return new WaitForSeconds(Time.deltaTime);
        }

        m_logo.color = Color.white;
        
        loop = true;
        while (loop)
        {
            m_image.color = Color.Lerp(m_image.color,Color.black,Time.deltaTime*.5f);
            m_logo.color = Color.Lerp(m_logo.color,Color.white * new Color(1f,1f,1f,0f),Time.deltaTime*.8f);
            if (1 - m_image.color.a < 0.01f)
                loop = false;
            
            yield return new WaitForSeconds(Time.deltaTime);
        }
        
        
        
        m_image.color = Color.black;
        
        yield return new WaitForSeconds(m_timeBeforeScrollCredits);
        
        Debug.Log("scroll");
        
        m_scrollCredits.GetComponent<ConstantForceUp>().enabled = true;
    }
}
