using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // The player's gameobject
    public GameObject player;

    public GameObject projectilePrefab;

    // Distance within which the enemy can see the player
    public int projectileRange;

    public int health;
    private int currentHealth;

    // holds the 8 cardinal directions
    protected List<Vector2> cardinalDirections;

    [Tooltip("How many beats before the enemy does something")]
    public int beatsBetweenActions = 1;
    private int beatsUntilNextAction;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        cardinalDirections = new List<Vector2>();
        // N
        cardinalDirections.Add(new Vector2(0, 1));
        // S
        cardinalDirections.Add(new Vector2(0, -1));
        // E
        cardinalDirections.Add(new Vector2(1, 0));
        // W
        cardinalDirections.Add(new Vector2(-1, 0));
        // NE
        cardinalDirections.Add(new Vector2(1, 1).normalized);
        // SE
        cardinalDirections.Add(new Vector2(1, -1).normalized);
        // SW
        cardinalDirections.Add(new Vector2(-1, -1).normalized);
        // NW
        cardinalDirections.Add(new Vector2(-1, 1).normalized);

        Conductor.Instance.GetComponent<Conductor>().BeatOccurred += OnBeat;

        currentHealth = health;
        beatsUntilNextAction = beatsBetweenActions;
    }

    /// <summary>
    /// Called once every beat. Attached to the BeatOccurred event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected virtual void OnBeat(object sender, System.EventArgs e)
    {
        // If the player is close enough to fire at
        if (player == null)
        {
            throw new System.NullReferenceException($"'{gameObject.name}' does not have a reference to the player.");
        }

        if (beatsUntilNextAction <= 0)
        {
            beatsUntilNextAction = beatsBetweenActions;
            TakeAction();
        }
    }

    private void TakeAction()
    {
        // Whether the player is in the line of fire
        bool playerInLoF = false;
        Vector2 directionToFire = Vector2.zero;
        for (int i = 0; i < cardinalDirections.Count && !playerInLoF; i++)
        {
            directionToFire = cardinalDirections[i];
            playerInLoF = Physics2D.Raycast(transform.position, directionToFire * projectileRange);
        }
        if (playerInLoF)
        {
            FireProjectile(directionToFire);
        }
        // enemy needs to close the distance
        else
        {
            Move();
        }
    }

    protected void Move()
    {
        // ?
    }

    /// <summary>
    /// Fires the projectilePrefab in the given direction
    /// </summary>
    /// <param name="direction">Direction the projectile will travel in</param>
    protected void FireProjectile(Vector2 direction)
    {
        // Make a new projectile
        if (projectilePrefab != null)
        {
            GameObject newProj = Instantiate(projectilePrefab);
            newProj.transform.position = this.transform.position;
            newProj.GetComponent<ProjectilePrototype>().direction = direction;
        }
    }

    /// <summary>
    /// Does damage to this enemy
    /// </summary>
    /// <param name="amount">amount of damage to do</param>
    public void Damage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
            Die();
    }

    /// <summary>
    /// Called when this enemy dies.
    /// </summary>
    protected void Die()
    {
        Destroy(gameObject);
    }
}
