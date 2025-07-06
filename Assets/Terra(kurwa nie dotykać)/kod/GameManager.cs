using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour{
    [SerializeField] private string[] levelNames;
    [SerializeField] private GameObject[] playerReferences = new GameObject[2];
    [SerializeField] private TMP_Text[] scoreVisualisation;
    [SerializeField] private TMP_Text timeText;
    public static GameManager instance;
    private int[] score = new int[2];
    private float timeToMonster = 0;
    private bool monsterSelected;
    [SerializeField] private GameObject napisTyJestesPotworem;
    private void Awake(){
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public static GameObject GetPlayerReference(byte id){
        return instance.playerReferences[id];
    }
    public static void AddPlayer(GameObject player, byte id){
        instance.playerReferences[id] = player;
    }
    public static void PlayerDied(byte id){
        instance.score[id]++;
        instance.scoreVisualisation[id].text = $"{instance.score[id]}";
        NextMap();
    }
    public static void NextMap(){
        FadeAway.GoDark();
        instance.Invoke(nameof(LoadMap), .17f);
    }
    private void LoadMap(){
        SceneManager.LoadScene(instance.levelNames[Random.Range(0, instance.levelNames.Length)]);
        timeToMonster = 22;
        monsterSelected = false;
        ResurrectPlayers();
    }
    private void Update()
    {
        if (timeToMonster > 0)
        {
            timeToMonster -= Time.deltaTime;
            timeText.text = timeToMonster.ToString("F2");
        }
        else
        {
            timeText.text = "";
            if (!monsterSelected)
            {
                SelectMonster();
                monsterSelected = true;
            }
        }
    }

    private void SelectMonster()
    {
        if (playerReferences[0] != null)
        {
            int rng = Random.Range(0, 2);
            playerReferences[rng].GetComponent<PlayerController>().BestiaMode();
            var napis = Instantiate(napisTyJestesPotworem, transform);
            napis.transform.position = playerReferences[rng].transform.position;
            napis.transform.position += new Vector3(0, 1, 0);
            Destroy(napis, 2f);
            SoundManager.PlaySound(SoundType.YouAreMonster);

            Debug.Log("Gracz " + rng + " zostal bestiom");
        }
    }

    private static void ResurrectPlayers(){
        foreach (var player in instance.playerReferences){
            if (player != null)
                player.GetComponent<IHealth>().Resurrect();
        }
    }
}
