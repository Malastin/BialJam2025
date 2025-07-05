using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour{
    [SerializeField] private string[] levelNames;
    [SerializeField] private GameObject[] playerReferences = new GameObject[2];
    private static GameManager instance;
    private int[] score = new int[2];
    private void Awake(){
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public static GameObject GetPlayerReference(byte id){
        return instance.playerReferences[id];
    }
    public static void PlayerDied(byte id){
        instance.score[id]++;
        NextMap();
    }
    private static void NextMap(){
        SceneManager.LoadScene(instance.levelNames[Random.Range(0, instance.levelNames.Length)]);
        ResurrectPlayers();
    }
    private static void ResurrectPlayers(){
        foreach (var player in instance.playerReferences){
            player.GetComponent<IHealth>().Resurrect();
        }
    }
}
