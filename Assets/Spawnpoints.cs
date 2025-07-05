using UnityEngine;

public class Spawnpoints : MonoBehaviour{
    private static Spawnpoints instance;
    [SerializeField] private Transform[] spawnPoints;
    private void Awake(){
        instance = this;   
    }
    public static Vector2 GetSpawnpoints(byte id){
        return instance.spawnPoints[id].position;
    }
}
