using TMPro;
using UnityEngine;

public class ShowTimeToEnd : MonoBehaviour
{
    private TextMeshProUGUI textMesh;

    private void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    private void FixedUpdate()
    {
        textMesh.text = "Time:" + ArenaGameCore.timer;
    }
}
