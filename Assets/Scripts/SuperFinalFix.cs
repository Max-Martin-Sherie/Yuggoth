using UnityEngine;
using UnityEngine.SceneManagement;

public class SuperFinalFix : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape)) SceneManager.LoadScene(0);
    }
}
