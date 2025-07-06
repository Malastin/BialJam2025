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
        int numberOfChests = 2; // Number of chests to spawn
        for (int i = 0; i < numberOfChests; i++)
        {
            float randomX = Random.Range(-transform.localScale.x, transform.localScale.x); // chujowo to dziala dlatego pierdole to teraz -> JEST JUZ GIT POZDRO
            float randomY = Random.Range(0, 5);
            Vector2 spawnPosition = new Vector2(randomX, randomY);
            var box = Instantiate(chestPrefab, transform.parent);
            box.transform.position = transform.position;
            box.transform.position = new Vector3(SpellManager.platformaX, 0, 0);
            box.transform.position += (Vector3)spawnPosition;
        }

    }
}
