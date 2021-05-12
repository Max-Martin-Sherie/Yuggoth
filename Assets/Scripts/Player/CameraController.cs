using UnityEngine;
/// <summary>
/// cette classe est pour la rotation de la caméra selon les axes X et Y
/// </summary>

public class CameraController : MonoBehaviour
{
    //Définition de la ensibilité de la caméra
    [Range(200, 1000)] public float m_mouseSensitivity = 200;
    
    //Varibles d'inversion de caméra
    [SerializeField] [Range(-1,1)] private int m_yInvertion = -1;
    [SerializeField] [Range(-1, 1)] private int m_xInvertion = 1;

    private GameObject m_cam;

    //Force de rotation appliqué par rapport à un axe
    private float m_xRotation;
    private float m_yRotation;

    private void Start()
    {
        //Récupération de l'objet camera
        m_cam = GetComponentInChildren<Camera>().gameObject;

        //Rend le curseur invisible et bloqué
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        m_xRotation = transform.rotation.eulerAngles.x;
        m_yRotation = transform.rotation.eulerAngles.y;
    }

    private void Update()
    {   
        //Récupération et affection des valeurs lors de l'utilisation du controller
        float mouseX = Input.GetAxisRaw("Mouse X");
        float mouseY = Input.GetAxisRaw("Mouse Y");

        //Set the values of rotations
        m_yRotation += mouseX * m_mouseSensitivity * m_xInvertion * Time.deltaTime;
        m_xRotation += mouseY * m_mouseSensitivity * m_yInvertion * Time.deltaTime;

        //Limitation de l'angle de rotation de l'axe X pour éviter une rotation à plus de 180degré
        m_xRotation = Mathf.Clamp(m_xRotation, -80f, 80);

        //Affectation of values
        m_cam.transform.localRotation = Quaternion.Euler(m_xRotation, 0, 0);
        transform.rotation = Quaternion.Euler(0, m_yRotation, 0);
    }
}
