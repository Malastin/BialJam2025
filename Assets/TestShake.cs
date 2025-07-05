using UnityEngine;

public class TestShake : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CameraShaker.StartCameraShake(5,5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
