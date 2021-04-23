using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private Tilemap floor;
    [SerializeField]
    private Tilemap collideable;
    [SerializeField]
    private Tilemap props;

    // The player's gameobject
    public GameObject player;

    public GameObject projectilePrefab;

    // Distance within which the enemy can see the player
    public int projectileRange;

    public int health;
    private int currentHealth;

    public int power = 3;

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

        // TODO: Figure out why diagonals don't work...
        //// NE
        //cardinalDirections.Add(new Vector2(1, 1));
        //// SE
        //cardinalDirections.Add(new Vector2(1, -1));
        //// SW
        //cardinalDirections.Add(new Vector2(-1, -1));
        //// NW
        //cardinalDirections.Add(new Vector2(-1, 1));

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

        beatsUntilNextAction -= 1;
    }

    private void TakeAction()
    {
        // Whether the player is in the line of fire
        bool playerInLoF = false;
        RaycastHit2D hit;
        Vector2 directionToFire = Vector2.zero;
        for (int i = 0; i < cardinalDirections.Count && !playerInLoF; i++)
        {
            directionToFire = cardinalDirections[i];
            hit = Physics2D.Raycast(transform.position, directionToFire * projectileRange);
            playerInLoF = hit.collider.gameObject.tag == "Player";
        }
        if (playerInLoF)
        {
            FireProjectile(directionToFire);
        }
        // enemy needs to close the distance
        else
        {
            Move(cardinalDirections[Random.Range(0, 4)]);
        }
    }

    private void Move(Vector2 direction)
    {
        if (CanMove(direction))
        {
            StartLerpMovement(transform.position + (Vector3)direction);
        }
    }

    private bool CanMove(Vector2 direction)
    {
        Vector3Int gridPosition = floor.WorldToCell(transform.position + (Vector3)direction);
        if (!floor.HasTile(gridPosition) || collideable.HasTile(gridPosition) || props.HasTile(gridPosition))
        {
            return false;
        }
        else
        {
            return true;
        }

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
            newProj.GetComponent<Projectile>().direction = direction;
            newProj.GetComponent<Projectile>().power = power;
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

    private void OnDestroy()
    {
        Conductor.Instance.GetComponent<Conductor>().BeatOccurred -= OnBeat;
    }

    #region LerpMovement
    // Update is called once per frame
    void Update()
    {
        if (moving) LerpMovement();
    }

    // Fields needed for movement
    private bool moving = false;
    private Vector3 targetPosition = Vector3.zero;
    private Vector3 startingPosition = Vector3.zero;
    private float currentSecondsMoved = 0;
    /// <summary>
    /// Portable movememnt method that lerps from a starting position to a target position using
    /// the fields directly above this method.
    /// </summary>
    private void LerpMovement()
    {
        // If we haven't finished moving yet, move by lerping from the start position to the target position.
        if (moving && currentSecondsMoved < Conductor.Instance.secondsToMove)
        {
            currentSecondsMoved += Time.deltaTime;

            float t = Mathf.Clamp(currentSecondsMoved / Conductor.Instance.secondsToMove, 0, 1);
            transform.position = Vector3.Lerp(startingPosition, targetPosition, t);

            // Movement finished
            if (t == 1)
            {
                moving = false;
                targetPosition = Vector3.zero;
                startingPosition = Vector3.zero;
                currentSecondsMoved = 0;
            }
        }
    }

    /// <summary>
    /// Starts LerpMovement to target position
    /// </summary>
    /// <param name="targetPos">the position to lerp to</param>
    private void StartLerpMovement(Vector3 targetPos)
    {
        startingPosition = transform.position;
        targetPosition = targetPos;
        moving = true;
    }
    #endregion
}
