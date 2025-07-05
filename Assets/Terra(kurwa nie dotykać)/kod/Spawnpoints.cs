using UnityEngine;

public class Spawnpoints : MonoBehaviour{
    private static Spawnpoints instance;
    private Vector2[] spawnPoints = new Vector2[2];
    private void Awake(){
        instance = this;
        int i = 0;
        spawnPoints = new Vector2[GetComponentsInChildren<Transform>().Length];
        foreach (var spawnpoint in GetComponentsInChildren<Transform>()) {
            instance.spawnPoints[i] = spawnpoint.position;
            i++;
        }   
    }
    public static Vector2 GetSpawnpoints(byte id){
        return instance.spawnPoints[id];
    }
}
