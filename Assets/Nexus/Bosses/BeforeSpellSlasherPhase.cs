// Code snippet for the Slasher boss unique phase
//
// This phase is a simple example of a phase that marks an area and then damages it after a delay.
// The phase has a cooldown for the entire process and a delay before the damage is applied.
// The phase can be used to create a telegraphed attack that players can avoid.

using BossSystem;
using UnityEngine;

public class BeforeSpellSlasherPhase : IBossPhase
{
    public BossPhaseType PhaseType => BossPhaseType.BeforeSpell;

    private float slashCooldown = 5f;
    private float slashDelay = 2f;
    private float slashTimer;
    private bool areaMarked;

    public GameObject slashAreaPrefab;

    public void EnterPhase(BossController controller)
    {
        slashTimer = slashCooldown;
        areaMarked = false;
        Debug.Log("Entering BeforeSpellSlasherPhase");

        // Instantiate the slash area prefab if needed
        GameObject slash = Object.Instantiate(Resources.Load("slashArea")) as GameObject;
        slashAreaPrefab = slash;
        slashAreaPrefab.SetActive(false);
    }

    public void UpdatePhase(BossController controller)
    {
        // Pseudo-code for timing, replace as needed
        slashTimer -= 1f * Time.deltaTime;
        if (slashTimer <= 0f)
        {
            if (!areaMarked)
            {
                // Mark the slash area
                areaMarked = true;
                slashTimer = slashDelay;
                // Possibly show some VFX or indicator
                slashAreaPrefab.SetActive(true);
            }
            else
            {
                // Damage the area
                areaMarked = false;
                slashTimer = slashCooldown;
            }
        }
    }

    public void ExitPhase(BossController controller)
    {
        // Cleanup or reset
        Debug.Log("Exiting BeforeSpellSlasherPhase");
    }

    
}