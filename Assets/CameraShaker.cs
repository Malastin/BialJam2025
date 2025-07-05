using UnityEngine;

public class CameraShaker : MonoBehaviour{
    private static CameraShaker instance;
    private float amplitude = 0f; 
    private float currentAmplitude = 0f;
    private float decreaseRate = 0f;
    private float timeElapsed = 0f; 
    private float noisex = 0;
    private float noisey = 0;
    private float shake_time = 0;
    public float decrease_rate = 0.1f;
    public float noiseScale = 1f;
    public float noise_strength = 1.1f;
    public float smooth = 0.5f;
    private Vector3 velocity;
    private void Awake() {
        instance = this;
    }
    public static void StartCameraShake(float start_amplitude, float shake_time){
        if(start_amplitude > instance.currentAmplitude){
            instance.shake_time = shake_time;
            instance.timeElapsed = 0;
            instance.amplitude = start_amplitude;
        } 
    }  
    private void Update(){
        timeElapsed += Time.fixedDeltaTime;
        currentAmplitude = amplitude * Mathf.Exp(-decreaseRate * timeElapsed);
        transform.position = Vector3.SmoothDamp(transform.position,
            new Vector3(
                CalculateShakeFunctionX(),
                CalculateShakeFunctionY(),
                transform.position.z
            ),
            ref velocity,
            smooth
        );
    }
    private float CalculateShakeFunctionX(){
        if(shake_time < timeElapsed){amplitude = 0; return 0;}
        float perlinNoiseX = Mathf.PerlinNoise(Time.time * noiseScale, 0) * 2 - 1;
        perlinNoiseX += Mathf.PerlinNoise(Time.time * noiseScale / 2, 0) * 2 - 1;
        perlinNoiseX += Mathf.PerlinNoise(Time.time * noiseScale * 2, 0) * 2 - 1;
        noisex = perlinNoiseX /3 * noise_strength * currentAmplitude;
        return noisex;
    } 
    private float CalculateShakeFunctionY(){
        if(shake_time < timeElapsed){amplitude = 0; return 0;}
        float perlinNoiseY = Mathf.PerlinNoise(0,Time.time * noiseScale) * 2 - 1;
        perlinNoiseY += Mathf.PerlinNoise(0,Time.time * noiseScale / 2) * 2 - 1;
        perlinNoiseY += Mathf.PerlinNoise(0,Time.time * noiseScale * 2) * 2 - 1;
        noisey = perlinNoiseY /3 * noise_strength * currentAmplitude;
        return noisey;
    } 
}
