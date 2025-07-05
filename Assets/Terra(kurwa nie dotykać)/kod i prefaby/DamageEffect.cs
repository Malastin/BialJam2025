using System.Collections;
using UnityEngine;
public class DamageEffect : MonoBehaviour{
    public AnimationCurve splashAnimation;
    private Material material;
    private void Awake(){
        material = GetComponent<SpriteRenderer>().sharedMaterial;
    }
    public IEnumerator DamageSplash(){
        float currentFlashAmount = 0;
        float elapseTime = 0;
        while (elapseTime < splashAnimation.keys[^1].time){
            elapseTime += Time.deltaTime;
            currentFlashAmount = Mathf.Lerp(splashAnimation.Evaluate(elapseTime), 0, elapseTime / splashAnimation.keys[^1].time);
            GetComponent<SpriteRenderer>().material.SetFloat("_SplashStrength", currentFlashAmount);
            yield return null;
        }
        GetComponent<SpriteRenderer>().material = material;
    }
}
