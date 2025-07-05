using UnityEngine;

public class FadeAway : MonoBehaviour{
    private static FadeAway instance;
    private Animator anim;
    private void Awake(){
        instance = this;
        anim = GetComponent<Animator>();
    }
    public static void GoDark() {
        instance.anim.Play("Dark");
    }
}
