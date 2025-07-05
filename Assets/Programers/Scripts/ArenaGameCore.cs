using System.Collections;
using UnityEngine;

public class ArenaGameCore : MonoBehaviour
{
    private int timer = 31;

    private void Start()
    {
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        while (true)
        {
            timer--;
            yield return new WaitForSeconds(1f);
        }
    }
}



public enum PlayerStates
{
    idle,
    run,
    jump,
    dash,
    death,
    normalAttack,
    skyAttack,
    fall,
}
