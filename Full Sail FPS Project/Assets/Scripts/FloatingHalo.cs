using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class FloatingHalo : MonoBehaviour
{
    public GameObject backpack;  // Reference to the backpack
    public GameObject journal;  // Reference to the backpack

    private Bloom bloomEffect;  // Reference to the Bloom effect

    void Start()
    {
        // Get the Bloom effect from the Post-Processing Volume
        PostProcessVolume postProcessVolume = backpack.GetComponentInChildren<PostProcessVolume>();
        if (postProcessVolume != null)
        {
            postProcessVolume.profile.TryGetSettings(out bloomEffect);
        }

        // Start with the bloom effect disabled
        if (bloomEffect != null)
        {
            bloomEffect.intensity.value = 0f;
        }
    }

    // Function to enable glowing effect
    public void EnableBackpackGlow()
    {
        if (bloomEffect != null)
        {
            bloomEffect.intensity.value = 5f;  // Set desired intensity for glow
        }
    }

    // Optionally, hide the glow effect
    public void DisableBackpackGlow()
    {
        if (bloomEffect != null)
        {
            bloomEffect.intensity.value = 0f;  // Disable the glow effect
        }
    }
}
