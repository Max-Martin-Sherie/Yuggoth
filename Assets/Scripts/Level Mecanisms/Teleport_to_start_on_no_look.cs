using UnityEngine;

/// <summary>
/// This script will teleport and object to it's original place if it isn't visible
/// </summary>
public class Teleport_to_start_on_no_look : MonoBehaviour
{
    //The rotation and position of the player
    //It is better to do it this way instead of simply creating a single transform variable because a transform is a get only so it cannot be set
    private Vector3 m_startPosition;
    private Quaternion m_startRotation;

    //The renderer to check if the item is visible
    private Renderer m_renderer;
    
    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        //Fetching the original position and rotation
        m_startPosition = transform.position;
        m_startRotation = transform.rotation;


        //Getting the renderer
        m_renderer = GetComponent<Renderer>();
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        //Checking if the item is visible...
        if (transform.position != m_startPosition && !m_renderer.isVisible)
        {
            //If it isn't, teleport the block back to it's position and rotation
            transform.position = m_startPosition;
            transform.rotation = m_startRotation;
        }
    }
}
