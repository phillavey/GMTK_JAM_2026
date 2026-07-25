using UnityEngine;

public class Spellcasting : MonoBehaviour
{
    public static int spellFocusRequired = 25;
    [SerializeField] private static int manaIncreaseAmount = 5;
    private int spellProgress;
    public int focusMeter;

    private PlayerInventory playerInventory;

    private void Awake()
    {
        EVENT_BUS.Subscribe(EventType.TASK_COMPLETED, IncreaseMana);

        playerInventory = GetComponent<PlayerInventory>();
        spellProgress = spellFocusRequired;
        focusMeter = 0;
    }

    void IncreaseMana(PublishEventArgs args)
    {
        Debug.Log("MANA INCREASED!!!!!!");
        focusMeter += manaIncreaseAmount;
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
