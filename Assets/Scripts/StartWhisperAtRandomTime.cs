using UnityEngine;

public class StartWhisperAtRandomTime : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioSource _as = GetComponent<AudioSource>();
        _as.time = Random.Range(0,GetComponent<AudioSource>().clip.length);
    }
}
