using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    private enum BossAction
    {
        FireInAllDirections,
        FireInSpiral,
    }

    // Holds amountOfProjectilesToShootInSpiral of directions in the unit circle, equally spaced.
    private List<Vector2> unitCircleDirections;
    // Long name but it does what it says lol
    public int amountOfProjectilesToShootInSpiral;

    public int alertRadius;

    protected override void Start()
    {
        // Fill unit circle directions
        unitCircleDirections = new List<Vector2>();
        for (float i = 0; i < 2 * Mathf.PI; i += 2 * Mathf.PI / amountOfProjectilesToShootInSpiral)
        {
            unitCircleDirections.Add(new Vector2(Mathf.Cos(i), Mathf.Sin(i)));
        }

        currentBeatsUntlBossAction = beatsUntilBossAction;

        base.Start();
    }

    // How many beats until a boss action is taken
    public int beatsUntilBossAction = 16;
    private int currentBeatsUntlBossAction;

    private bool bossActionInProgress = false;

    /// <summary>
    /// Called once every beat. Attached to the BeatOccurred event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected override void OnBeat(object sender, System.EventArgs e)
    {
        bool bossBehaviors = currentBeatsUntlBossAction <= 0;

        // If the boss hasn't been alerted, do nothing
        if (Vector2.Distance(transform.position, player.transform.position) < alertRadius)
        {
            return;
        }

        if (bossBehaviors)
        {
            bossActionInProgress = true;

            int whichBossAction = Random.Range(0, 2);
            TakeBossAction((BossAction)whichBossAction);
        }
        else if (!bossActionInProgress)
        {
            base.OnBeat(sender, e);
        }

        currentBeatsUntlBossAction--;
    }

    private void TakeBossAction(BossAction action)
    {
        // Don't take another boss action for a bit
        currentBeatsUntlBossAction = beatsUntilBossAction;

        switch (action)
        {
            case BossAction.FireInAllDirections:
                FireProjectilesInAllDirections();
                break;
            case BossAction.FireInSpiral:
                StartCoroutine(FireProjectilesInSpiral());
                break;
            default:
                // ???
                break;
        }
    }

    #region Boss Actions

    private void FireProjectilesInAllDirections()
    {
        for (int i = 0; i < cardinalDirections.Count; i++)
        {
            FireProjectile(cardinalDirections[i]);
        }

        bossActionInProgress = false;
    }

    private IEnumerator FireProjectilesInSpiral(int nextDirectionIndex = 0)
    {
        FireProjectile(unitCircleDirections[nextDirectionIndex]);
        yield return new WaitForSeconds(Conductor.Instance.secondsPerBeat / 2);
        
        nextDirectionIndex++;
        if (nextDirectionIndex < unitCircleDirections.Count)
        {
            StartCoroutine(FireProjectilesInSpiral(nextDirectionIndex));
        }
        else
        {
            bossActionInProgress = false;
        }
    }

    #endregion
}
