using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class RainbowColors : MonoBehaviour{
    private Volume volume;
    private Vignette vg;
    private Bloom bloom;
    private void Awake(){
        volume = GetComponent<Volume>();
        volume.profile.TryGet(out vg);
        volume.profile.TryGet(out bloom);
    }
    void Update()
    {
        float t = Time.time * .1f;
        Color rainbow = Color.HSVToRGB(t % .6f, .6f, .6f);
        vg.color.value = rainbow;
        //bloom.tint.value = rainbow;
    }
}
