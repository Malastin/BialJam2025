using UnityEngine;

public class PlayerFighterStats : MonoBehaviour, IHealth
{
    //wszystkie zmienne narazie publiczne potem b�dziemy kombinowa� by zmniejszy� ilo�� syfu
    //animacje chodzenie skalowac z movespeedem a niamacjie ataku z attack speedem

    private int health;
    public int healthMax;
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
        //tutaj damy wywo�anie funkcji do jakiegos cora aby zatrzyam� walk� i da� ekran podsumowuj�cy gre
        //co� w stylu �e walka trwa�a x czasu, mag u�y� x czar�w i mo�liwo�c zacz�cia kolejnej walki
    }

    public void Damage(int damage) {
        health -= damage;
        Debug.Log("I go dmg " + damage + " | Hp: " + health + "/" + healthMax);

        if (health <= 0){
            PlayerDeath();
        }
    }
    public void Heal(int heal){
        health = Mathf.Clamp(health + heal, 0, healthMax);
    }
    public void Resurrect(){}
}
