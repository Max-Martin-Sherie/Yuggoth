using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class adds feedback to the cursor letting the user know that he will be able to interact with an object 
/// </summary>

public class CursorFeedback : MonoBehaviour
{
    [SerializeField][Tooltip("the layer mask of items that the cursor will react to")]private LayerMask m_interactableItems;
    [SerializeField][Tooltip("the color when there is no feedback")]private Color m_defaultColor;
    [SerializeField][Tooltip("the color when there is feedback")]private Color m_interactColor;
    [SerializeField][Tooltip("the scale at which the cursor will grow")]float m_scaleMultiplier;
    [SerializeField][Tooltip("the color change speed")]float m_colorChangeSpeed = 3f;
    [SerializeField][Tooltip("the grow and shrink speed")]float m_sizeChangeSpeed = 5f;

    private Image m_image; //Getting the image component to modify color
    private RectTransform m_rectTransform; //Getting the transform component to modify the scale
    private Vector3 m_ogScale; //Keeping the original scale in memory to be able to return to it

    private void Start()
    {
        m_image = GetComponent<Image>(); //Fetching the image component
        m_rectTransform = GetComponent<RectTransform>(); //Fetching the transform component
        m_ogScale = m_rectTransform.localScale; //Setting the original scale in memory
    }

    // Update is called once per frame
    void Update()
    {
        //Checking if the raycast object is interactable...
        if (InteractRaycast.m_hitSomething && m_interactableItems == (m_interactableItems | (1 << InteractRaycast.m_hitTarget.collider.gameObject.layer)))
        {
            //...if it is lerping to the interact state
            m_image.color = Color.Lerp(m_image.color, m_interactColor, m_colorChangeSpeed * Time.deltaTime);
            m_rectTransform.localScale = Vector3.Lerp(m_rectTransform.localScale,m_ogScale * m_scaleMultiplier, m_sizeChangeSpeed  * Time.deltaTime);
        }
        else
        {
            //..if it is lerping to the default state
            m_image.color = Color.Lerp(m_image.color, m_defaultColor, m_colorChangeSpeed * Time.deltaTime);
            m_rectTransform.localScale = Vector3.Lerp(m_rectTransform.localScale ,m_ogScale, m_sizeChangeSpeed * Time.deltaTime);
        }
    }
}
