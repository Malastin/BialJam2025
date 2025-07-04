using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{

    public GameObject player1;
    public GameObject player2;
    public GameObject spellManager;


    void Start()
    {

    }

    void Update()
    {

    }

    public void P1Movement(InputAction.CallbackContext input)
    {
        player1.GetComponent<PlayerController>().Movement(input);
    }

    public void P2Movement(InputAction.CallbackContext input)
    {
        player2.GetComponent<PlayerController>().Movement(input);
    }

    public void SpellAim(InputAction.CallbackContext input)
    {
        spellManager.GetComponent<SpellController>().Aim(input);
    }


}
