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
        localScale.x = Mathf.Clamp(player.GetComponent<IHealth>().GetHealth() / 40, 0, 2);
        transform.localScale = localScale;
    }
}
