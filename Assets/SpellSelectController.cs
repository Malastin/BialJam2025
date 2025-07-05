using UnityEngine;

public class SpellSelectController : MonoBehaviour
{
    public GameObject[] spellItems;

    public int selectedSpellIndex = 0;
    private SpriteRenderer selectedSpellSprite;
    

    void Start()
    {
        selectedSpellSprite.sprite = spellItems[selectedSpellIndex].GetComponent<SpriteRenderer>().sprite;
    }

    void Update()
    {

    }

    public void SelectNextSpell()
    {
        selectedSpellIndex++;
        if (selectedSpellIndex >= spellItems.Length)
        {
            selectedSpellIndex = 0;
        }
        selectedSpellSprite.sprite = spellItems[selectedSpellIndex].GetComponent<SpriteRenderer>().sprite;
    }

    public void SelectPreviousSpell()
    {
        selectedSpellIndex--;
        if (selectedSpellIndex < 0)
        {
            selectedSpellIndex = spellItems.Length - 1;
        }
        selectedSpellSprite.sprite = spellItems[selectedSpellIndex].GetComponent<SpriteRenderer>().sprite;
    }

    public SpellBehavior GetSelectedSpell()
    {
        return spellItems[selectedSpellIndex].GetComponent<SpellBehavior>();
    }
}
