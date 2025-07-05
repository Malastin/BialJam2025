using System.Collections;
using UnityEngine;

public class ArenaGameCore : MonoBehaviour
{
    public static int timer = 31;

    private void Start()
    {
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        while (true)
        {
            timer--;
            if (timer == 0)
            {
                yield break;
            }
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
    grabedToWall,
    endSkyAttack,
}
