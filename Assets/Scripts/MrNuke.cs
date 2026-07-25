using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static MasterGameController;

public class MrNuke : MonoBehaviour, IEnemy
{
    public bool isAngry;
    public ROOM taskRoom;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Light2D nukeLight;
    private Color originalLightColor;
    private float originalLightIntensity;

    // Use this for initialization
    void Start()
    {
        isAngry = false;
        EVENT_BUS.Subscribe(EventType.NUKEING_IS_NOW_LEGAL, MakeAngry);

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }

        nukeLight = GetComponentInChildren<Light2D>();
        if (nukeLight != null)
        {
            originalLightColor = nukeLight.color;
            originalLightIntensity = nukeLight.intensity;
        }
    }

    void MakeAngry(PublishEventArgs args)
    {
        // Play some SFX
        isAngry = true;

        // Change sprite tint to bright orange
        if (spriteRenderer != null)
        {
            spriteRenderer.color = new Color(1f, 0.5f, 0f, spriteRenderer.color.a);
        }

        // If there is a 2D light, make it bright orange
        if (nukeLight != null)
        {
            nukeLight.color = new Color(1f, 0.55f, 0f);
            nukeLight.intensity = Mathf.Max(1.5f, originalLightIntensity * 2f);
            nukeLight.enabled = true;
        }
    }

    public void HitWithWeapon(PlayerInventory.ItemType weapon)
    {
        SFX_Controller.instance?.playSFX("whiff", transform, 1f);
        if (weapon == PlayerInventory.ItemType.SCISSORS && isAngry)
        {
            // One shot kill
            Debug.Log("NUKE DEFUSED");
            EVENT_BUS.Publish(EventType.NUKE_DEFUSED, null);

            Dictionary<string, object> eventArgs = new()
            {
                { "task_name", GAME_TASK.NUKE_DIFFUSING }
            };
            EVENT_BUS.Publish(EventType.TASK_COMPLETED, new PublishEventArgs(eventArgs));

            // Revert visuals
            isAngry = false;
            if (spriteRenderer != null)
            {
                spriteRenderer.color = originalColor;
            }
            if (nukeLight != null)
            {
                nukeLight.color = originalLightColor;
                nukeLight.intensity = originalLightIntensity;
            }
        }
        else
        {
            // Increment an angriness meter?
        }
    }
}
