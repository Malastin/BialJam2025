using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour{
    [SerializeField] private string[] levelNames;
    [SerializeField] private GameObject[] playerReferences = new GameObject[2];
    [SerializeField] private TMP_Text[] scoreVisualisation; 
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
        instance.scoreVisualisation[id].text = $"{instance.score[id]}";
        NextMap();
    }
    private static void NextMap(){
        SceneManager.LoadScene(instance.levelNames[Random.Range(0, instance.levelNames.Length)]);
        ResurrectPlayers();
        MovePlayersToSpawnpoints();
    }
    private static void ResurrectPlayers(){
        foreach (var player in instance.playerReferences){
            player.GetComponent<IHealth>().Resurrect();
        }
    }
    private static void MovePlayersToSpawnpoints(){
        for(byte i = 0; i < instance.playerReferences.Length; i++){
            instance.playerReferences[i].transform.position = Spawnpoints.GetSpawnpoints(i);
        }
    }
}
