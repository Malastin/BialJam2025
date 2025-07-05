using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessManager : MonoBehaviour{
    private static PostProcessManager instance;
    private Volume volume;
    private Vignette vg;
    public float smooth = .1f;
    private void Awake()
    {
        instance = this;
        volume = GetComponent<Volume>();
        volume.profile.TryGet(out vg);
    }
    void Update(){
        float t = Time.time * .1f;
        Color rainbow = Color.HSVToRGB(t % .6f, .6f, .6f);
        vg.color.value = rainbow;
    }
    public static void MakeDark()
    {

    }
    public static void MakeLight()
    {
        
    }
}
