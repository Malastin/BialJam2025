using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealhBar : MonoBehaviour{
    private Vector3 localScale;
    public GameObject player;
    public float hp_divide = 2;
    void Start(){
        localScale = transform.localScale;
    }
    void Update(){
        localScale.x = Mathf.Clamp(player.GetComponent<IHealth>().GetHealth() / hp_divide, 0, 100);
        transform.localScale = localScale;
    }
}
