using UnityEngine;

public class Spellcasting : MonoBehaviour
{
    // Mana / spell progress settings
    public static int manaRequiredToCast = 100; // total mana required to cast via direct cast (kept for reference)
    [SerializeField] private int manaPerTask = 25; // mana gained when a task completes
    [SerializeField] private int manaCostPerSwing = 10; // mana consumed per swing in seance room
    [SerializeField] private int spellProgressPerSwing = 10; // progress added per swing
    [SerializeField] private int spellProgressRequired = 50; // progress required to complete the spell

    public int focusMeter;
    public int spellProgress;
    public int SpellProgressRequired => spellProgressRequired;

    private PlayerInventory playerInventory;

    private void Awake()
    {
        EVENT_BUS.Subscribe(EventType.TASK_COMPLETED, IncreaseMana);
        playerInventory = GetComponent<PlayerInventory>();

        focusMeter = 0;
        spellProgress = 0;
    }

    void IncreaseMana(PublishEventArgs args)
    {
        Debug.Log("MANA INCREASED!!!!!!");
        focusMeter += manaPerTask;
        if (focusMeter > 100)
        {
            focusMeter = 100;
        }
        EVENT_BUS.Publish(EventType.MANA_CHANGED, null);
    }

    public void TrySpellCasting()
    {
        // Require enough mana to cast
        if (focusMeter < manaRequiredToCast)
        {
            // Tell player they need more focus
            Debug.Log("Not enough mana to cast the spell");
            return;
        }

        if (playerInventory.CurrentItem == PlayerInventory.ItemType.NONE)
        {
            // Tell player they need an item
            Debug.Log("Need an item to cast the spell");
            return;
        }

        focusMeter -= manaRequiredToCast;
        if (focusMeter < 0) focusMeter = 0;
        EVENT_BUS.Publish(EventType.MANA_CHANGED, null);
        DoSpellCasting();
        EVENT_BUS.Publish(EventType.SPELL_CAST_SUCCESS, null);
    }

    private void DoSpellCasting()
    {
        // Play a sound, do animation or screen shake
    }

    public void ApplySeanceSwing()
    {
        if (focusMeter < manaCostPerSwing) return; // not enough mana to convert

        focusMeter -= manaCostPerSwing;
        if (focusMeter < 0) focusMeter = 0;

        spellProgress += spellProgressPerSwing;
        if (spellProgress > spellProgressRequired) spellProgress = spellProgressRequired;

        EVENT_BUS.Publish(EventType.MANA_CHANGED, null);
        EVENT_BUS.Publish(EventType.SPELL_PROGRESS_UPDATED, null);

        if (spellProgress >= spellProgressRequired)
        {
            // Spell completed
            spellProgress = 0;
            EVENT_BUS.Publish(EventType.SPELL_PROGRESS_UPDATED, null);
            EVENT_BUS.Publish(EventType.SPELL_CAST_SUCCESS, null);
        }
    }
}
