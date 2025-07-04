using UnityEngine;

public class PlayerFighterStats : MonoBehaviour
{
    //wszystkie zmienne narazie publiczne potem bêdziemy kombinowaæ by zmniejszyæ iloœæ syfu
    //animacje chodzenie skalowac z movespeedem a niamacjie ataku z attack speedem

    public int health;
    public int healthMax;
    public int baseDamage;
    public float baseMovementSpeed;
    public float movementSpeed;
    public float baseAttackSpeed;
    public float attackSpeed;

    private void Awake()
    {
        //wczytanie stat gracza na pocz¹tek by mia³ te staty
        LoadAllBaseStats();
    }

    private void LoadAllBaseStats()
    {
        health = 40;
        healthMax = health;

        //podstawka 2 dmg aby mocny atak robi³ ze 4 a ten szybki 1 dmg
        baseDamage = 2;

        baseMovementSpeed = 2f;
        movementSpeed = baseMovementSpeed;
        baseAttackSpeed = 1f;
        attackSpeed = baseAttackSpeed;
    }

    public void SetPlayerMovespeedToBase()
    {
        //ta funkcja bedzie potrzebna jak bedziemy chcieli przywrucici graczowi normaln¹ prêdkoœæ po debufie
        movementSpeed = baseMovementSpeed;
    }

    public void SetPlayerAttackSpeedToBase()
    {
        //ta funkcja bedzie potrzebna jak bedziemy chcieli przywrucici graczowi normaln¹ prêdkoœæ po debufie
        attackSpeed = baseAttackSpeed;
    }

    public void DealDamageToPlayer(int damageAmount)
    {
        //funkcja która bêdzie do zadawania obra¿eñ gracz¹
        health -= damageAmount;
        Debug.Log("I go dmg " + damageAmount + " | Hp: " + health + "/" + healthMax);

        if (health <= 0)
        {
            PlayerDeath();
        }
    }

    private void PlayerDeath()
    {
        //tutaj damy wywo³anie funkcji do jakiegos cora aby zatrzyamæ walkê i daæ ekran podsumowuj¹cy gre
        //coœ w stylu ¿e walka trwa³a x czasu, mag u¿y³ x czarów i mo¿liwoœc zaczêcia kolejnej walki
    }
}
