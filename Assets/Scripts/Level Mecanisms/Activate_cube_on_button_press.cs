using UnityEngine;

public class Activate_cube_on_button_press : MonoBehaviour
{
    [SerializeField] private Button m_button;

    private MeshRenderer m_meshRenderer;

    private void Start()
    {
        m_meshRenderer = GetComponent<MeshRenderer>();
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        m_meshRenderer.enabled = m_button.m_triggered;
    }
}
