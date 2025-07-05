using UnityEngine;

public class WallSideDetection : MonoBehaviour
{
    [SerializeField] private bool isBigger;
    [SerializeField] private PlayerController playerController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            playerController.wallXisBigger = isBigger;
        }
    }
}
