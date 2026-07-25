using UnityEngine;

public class Spellcasting : MonoBehaviour
{
    public static int spellFocusRequired = 25;
    private int spellProgress;
    public int focusMeter;

    private PlayerInventory playerInventory;

    private void Awake()
    {
        playerInventory = GetComponent<PlayerInventory>();
        spellProgress = spellFocusRequired;
        focusMeter = 25;
    }

    public void TrySpellCasting()
    {
        if (focusMeter == 0)
        {
            // Tell player they more focus
            return;
        }
        if (playerInventory.CurrentItem == PlayerInventory.ItemType.NONE)
        {
            // Tell player they need an item
            return;
        }
        DoSpellCasting();
    }

    private void DoSpellCasting()
    {
        Debug.Log("Casting a spell!");
        // Play a sound
        // Do a little animation or screen shake
        focusMeter--;
        if (--spellProgress == 0)
        {
            EVENT_BUS.Publish(EventType.SPELL_CAST_SUCCESS, null);
            spellProgress = spellFocusRequired;
        }
    }
}
