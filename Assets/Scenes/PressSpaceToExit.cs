using UnityEngine;

public class PressSpaceToExit : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonUp("Jump"))
            {
                Application.Quit();
            }
    }
}
