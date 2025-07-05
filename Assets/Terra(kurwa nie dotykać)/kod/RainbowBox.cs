using UnityEngine;

public class RainbowBox : MonoBehaviour{
    void Update(){
        float t = Time.time * .1f;  
        Color rainbow = Color.HSVToRGB(t % .9f, .9f, .9f);
        GetComponent<SpriteRenderer>().color = rainbow;
    }
}
