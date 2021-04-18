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

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Conductor.Instance.GetComponent<Conductor>().BeatOccurred += OnBeat;

        currentHealth = health;
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
        if (Vector2.Distance(this.transform.position, player.transform.position) < projectileRange)
        {
            FireProjectile(((Vector2)(player.transform.position - this.transform.position)).normalized);
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
