using UnityEngine;

public class KarzelMode : MonoBehaviour
{
    public GameObject obiektManipulacji;

    private void Start()
    {
        obiektManipulacji.transform.localScale = new Vector3(0.5f, 0.5f, 1);
        Destroy(gameObject, 5f);
    }

    private void OnDestroy()
    {
        obiektManipulacji.transform.localScale = new Vector3(1, 1, 1);
    }
}
