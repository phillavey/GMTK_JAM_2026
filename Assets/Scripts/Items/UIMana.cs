using TMPro;
using UnityEngine;

public class UIMana : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI manaText;
    [SerializeField] private TextMeshProUGUI spellProgressText;
    private Spellcasting spellcasting;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spellcasting = FindObjectOfType<Spellcasting>();
        EVENT_BUS.Subscribe(EventType.TASK_COMPLETED, OnManaChanged);
        EVENT_BUS.Subscribe(EventType.SPELL_CAST_SUCCESS, OnManaChanged);
        EVENT_BUS.Subscribe(EventType.MANA_CHANGED, OnManaChanged);
        EVENT_BUS.Subscribe(EventType.SPELL_PROGRESS_UPDATED, OnManaChanged);
        UpdateManaText();
    }

    // Update is called once per frame
    void Update()
    {
        // No per-frame updates required
    }

    private void OnDestroy()
    {
        EVENT_BUS.Unsubscribe(EventType.TASK_COMPLETED, OnManaChanged);
        EVENT_BUS.Unsubscribe(EventType.SPELL_CAST_SUCCESS, OnManaChanged);
        EVENT_BUS.Unsubscribe(EventType.MANA_CHANGED, OnManaChanged);
        EVENT_BUS.Unsubscribe(EventType.SPELL_PROGRESS_UPDATED, OnManaChanged);
    }

    private void OnManaChanged(PublishEventArgs args)
    {
        UpdateManaText();
    }

    private void UpdateManaText()
    {
        if (manaText == null) return;
        if (spellcasting == null)
        {
            spellcasting = FindObjectOfType<Spellcasting>();
            if (spellcasting == null) return;
        }
        manaText.text = $"Mana: {spellcasting.focusMeter} / {Spellcasting.manaRequiredToCast}";
        if (spellProgressText != null)
        {
            spellProgressText.text = $"Spell Progress: {spellcasting.spellProgress} / {spellcasting.SpellProgressRequired}";
        }
    }
}
