using UnityEngine;

public class Activate_cube_on_button_press : MonoBehaviour
{
    [SerializeField][Tooltip("Drag and drop a gameobject that has button component here")] private Button m_button; //Fetching the button

    private MeshRenderer m_meshRenderer; //the mesh renderer

    private void Start()
    {
        //Fetching the mesh renderer
        m_meshRenderer = GetComponent<MeshRenderer>();

        m_meshRenderer.enabled = m_button.m_triggered;
        
        m_button.OnButtonPress += EnableMeshRenderer; //Adding the enable mesh renderer to the on button press delegate
        m_button.OnButtonRelease += DisableMeshRenderer; //Adding the disable mesh renderer to the on button release of the delegate delegate
    }

    private void EnableMeshRenderer()
    {
        m_meshRenderer.enabled = true;
    }
    private void DisableMeshRenderer()
    {
        m_meshRenderer.enabled = false;
    }
}
