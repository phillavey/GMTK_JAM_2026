using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMana : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI manaText;
    [SerializeField] private TextMeshProUGUI spellProgressText;
    [SerializeField] private Image focusBar;
    [SerializeField] private Image spellProgressBar;
    [SerializeField] private TextMeshProUGUI focusValueText;
    [SerializeField] private TextMeshProUGUI progressValueText;

    private Spellcasting spellcasting;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spellcasting = FindObjectOfType<Spellcasting>();
        EVENT_BUS.Subscribe(EventType.TASK_COMPLETED, OnManaChanged);
        EVENT_BUS.Subscribe(EventType.SPELL_CAST_SUCCESS, OnManaChanged);
        EVENT_BUS.Subscribe(EventType.MANA_CHANGED, OnManaChanged);
        EVENT_BUS.Subscribe(EventType.SPELL_PROGRESS_UPDATED, OnManaChanged);
        // Use the focus/progress bar as the primary display. Hide the old mana text label if assigned.
        UpdateFocusBar();
        if (manaText != null) manaText.gameObject.SetActive(false);
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
        UpdateFocusBar();
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

    private void UpdateFocusBar()
    {
        if (spellcasting == null)
        {
            spellcasting = FindObjectOfType<Spellcasting>();
            if (spellcasting == null) return;
        }

        if (focusBar != null)
        {
            focusBar.fillAmount = (float)spellcasting.focusMeter / (float)Mathf.Max(1, Spellcasting.manaRequiredToCast);
        }

        if (spellProgressText != null)
        {
            spellProgressText.text = $"Spell Progress: {spellcasting.spellProgress} / {spellcasting.SpellProgressRequired}";
        }

        if (focusValueText != null)
        {
            focusValueText.text = $"{spellcasting.focusMeter} / {Spellcasting.manaRequiredToCast}";
        }

        if (progressValueText != null)
        {
            progressValueText.text = $"{spellcasting.spellProgress} / {spellcasting.SpellProgressRequired}";
        }

        if (spellProgressBar != null)
        {
            float pfill = spellcasting.SpellProgressRequired > 0 ? Mathf.Clamp01((float)spellcasting.spellProgress / (float)spellcasting.SpellProgressRequired) : 0f;
            spellProgressBar.fillAmount = pfill;
        }
    }
}
