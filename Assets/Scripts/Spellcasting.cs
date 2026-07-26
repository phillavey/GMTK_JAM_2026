using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Spellcasting : MonoBehaviour
{
    // Mana / spell progress settings
    public static int manaRequiredToCast = 100; // total mana required to cast via direct cast (kept for reference)
    [SerializeField] private int manaPerTask = 25; // mana gained when a task completes
    [SerializeField] private int manaCostPerSwing = 5; // mana consumed per swing in seance room
    [SerializeField] private int spellProgressPerSwing = 5; // progress added per swing
    [SerializeField] private int spellProgressRequired = 50; // progress required to complete the spell

    [SerializeField] private GameObject dialogObj;
    [SerializeField] Light2D globalLight;

    public int focusMeter;
    public int spellProgress;
    public int SpellProgressRequired => spellProgressRequired;

    private PlayerInventory playerInventory;

    private void Awake()
    {
        EVENT_BUS.Subscribe(EventType.TASK_COMPLETED, IncreaseMana);
        EVENT_BUS.Subscribe(EventType.SPRINTED, DecreaseManaForSprinting);
        playerInventory = GetComponent<PlayerInventory>();

        spellProgress = 0;
    }

    void DecreaseManaForSprinting(PublishEventArgs args)
    {
        focusMeter -= 2;
        EVENT_BUS.Publish(EventType.MANA_CHANGED, null);
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
            StartCoroutine(ShowDialog());
        }
    }

    //void DipGlobalLight()
    //{
    //    globalLight.intensity -= 0.1f;
    //}

    IEnumerator ShowDialog()
    {
        dialogObj.SetActive(true);
        globalLight.intensity = 0.3f;
        yield return new WaitForSeconds(3);
        globalLight.intensity = 1f;
        dialogObj.SetActive(false);
    }

    void setRandomSpellName()
    {
        // TODO
        ArrayList phrases1 = new ArrayList();
        ArrayList phrases2 = new ArrayList();
        phrases1.Add(" f l a t_");
        phrases2.Add("    t i r e");
        phrases1.Add("");
        phrases2.Add("");
        phrases1.Add("");
        phrases2.Add("");
       
        System.Random r = new System.Random();
        r.Next(phrases1.Count);

    }
}
