using System.Collections;
using UnityEngine;

public class PlayerFighterStats : MonoBehaviour, IHealth
{
    //wszystkie zmienne narazie publiczne potem b�dziemy kombinowa� by zmniejszy� ilo�� syfu   "POTEM"
    //animacje chodzenie skalowac z movespeedem a niamacjie ataku z attack speedem

    private int health;
    public int healthMax;
    public float baseMovementSpeed;
    public float movementSpeed;
    public float baseAttackSpeed;
    public float attackSpeed;
    private PlayerController playerController;
    private bool isAlredyDeath = false;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void Awake()
    {
        //wczytanie stat gracza na pocz�tek by mia� te staty
        LoadAllBaseStats();
    }

    private void LoadAllBaseStats()
    {
        health = 40;
        healthMax = health;

        baseMovementSpeed = 2f;
        movementSpeed = baseMovementSpeed;
        baseAttackSpeed = 1f;
        attackSpeed = baseAttackSpeed;
    }

    public void SetPlayerMovespeedToBase()
    {
        //ta funkcja bedzie potrzebna jak bedziemy chcieli przywrucici graczowi normaln� pr�dko�� po debufie
        movementSpeed = baseMovementSpeed;
    }

    public void SetPlayerAttackSpeedToBase()
    {
        //ta funkcja bedzie potrzebna jak bedziemy chcieli przywrucici graczowi normaln� pr�dko�� po debufie
        attackSpeed = baseAttackSpeed;
    }

    private void PlayerDeath()
    {
        isAlredyDeath = true;
        //tutaj damy wywo�anie funkcji do jakiegos cora aby zatrzyam� walk� i da� ekran podsumowuj�cy gre
        //co� w stylu �e walka trwa�a x czasu, mag u�y� x czar�w i mo�liwo�c zacz�cia kolejnej walki chuj
        playerController.DeatchAnimationTrigger();
        StartCoroutine(LoadNextGame());
    }

    public void Damage(int damage) {
        if (isAlredyDeath)
        {
            return;
        }
        health -= damage;
        Debug.Log("I go dmg " + damage + " | Hp: " + health + "/" + healthMax);
        playerController.damageEffect.TriggerDamageSplash();
        if (health <= 0){
            PlayerDeath();
        }
    }
    public void Heal(int heal){
        health = Mathf.Clamp(health + heal, 0, healthMax);
    }
    public void Resurrect(){}
    public int GetHealth(){return health;}
    private IEnumerator LoadNextGame()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            GameManager.PlayerDied(GetComponent<PlayerAddReferenceToManager>().id);
            yield break;
        }
    }
}
