using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooldown : MonoBehaviour{
    Vector3 localScale;
    public GameObject player;
    public float divde_hook = 2;
    void Start(){
        localScale = transform.localScale;
    }
    void Update(){
        localScale.x = Mathf.Clamp(player.GetComponent<PlayerController>().dashCooldown / divde_hook, 0, float.MaxValue);
        transform.localScale = localScale;
    }
}
