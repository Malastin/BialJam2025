using UnityEngine;

public class TargetMarkerController : MonoBehaviour
{
    GameObject player1;
    GameObject player2;
    public Transform target;

    void Start()
    {
        player1 = GameManager.GetPlayerReference(0);
        player2 = GameManager.GetPlayerReference(1);
        target = player1.transform;
    }

    public void ChangeTarget()
    {
        if (target == player1.transform)
        {
            target = player2.transform;
        }
        else if (target == player2.transform)
        {
            target = player1.transform;
        }
    }

    void Update()
    {
        if (target != null)
        {
            Vector3 targetPosition = target.position;
            targetPosition.y += 1.0f;
            transform.position = targetPosition;
        }
        else
        {
            Debug.LogWarning("Target is not assigned for MarkerController.");
        }    
    }
}
