using UnityEngine;

public class ChestRain : AreaOfEffectSpell
{
    public GameObject chestPrefab;

    public override void CastSpell()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("ChestRain spell requires a SpriteRenderer component to define the area of effect.");
            return;
        }
        Vector2 center = spriteRenderer.bounds.center;
        Vector2 size = spriteRenderer.bounds.size;
        int numberOfChests = 5; // Number of chests to spawn
        for (int i = 0; i < numberOfChests; i++)
        {
            float randomX = Random.Range(center.x - size.x / 2, center.x + size.x / 2); // chujowo to dziala dlatego pierdole to teraz
            float randomY = Random.Range(center.y - size.y / 2, center.y + size.y / 2);
            Vector2 spawnPosition = new Vector2(randomX, randomY);
            Instantiate(chestPrefab, spawnPosition, Quaternion.identity);
        }

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
