using TMPro;
using UnityEngine;

public class UIStamina : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI staminaText;

    void Start()
    {
        UpdateStaminaText();
    }

    private void UpdateStaminaText()
    {
        if (staminaText == null) return;
        staminaText.text = "Stamina: N/A";
    }
}
