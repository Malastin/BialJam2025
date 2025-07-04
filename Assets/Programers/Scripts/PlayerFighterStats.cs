using UnityEngine;

public class PlayerFighterStats : MonoBehaviour
{
    //wszystkie zmienne narazie publiczne potem b�dziemy kombinowa� by zmniejszy� ilo�� syfu
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
        //wczytanie stat gracza na pocz�tek by mia� te staty
        LoadAllBaseStats();
    }

    private void LoadAllBaseStats()
    {
        health = 40;
        healthMax = health;

        //podstawka 2 dmg aby mocny atak robi� ze 4 a ten szybki 1 dmg
        baseDamage = 2;

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

    public void DealDamageToPlayer(int damageAmount)
    {
        //funkcja kt�ra b�dzie do zadawania obra�e� gracz�
        health -= damageAmount;
        Debug.Log("I go dmg " + damageAmount + " | Hp: " + health + "/" + healthMax);

        if (health <= 0)
        {
            PlayerDeath();
        }
    }

    private void PlayerDeath()
    {
        //tutaj damy wywo�anie funkcji do jakiegos cora aby zatrzyam� walk� i da� ekran podsumowuj�cy gre
        //co� w stylu �e walka trwa�a x czasu, mag u�y� x czar�w i mo�liwo�c zacz�cia kolejnej walki
    }
}
