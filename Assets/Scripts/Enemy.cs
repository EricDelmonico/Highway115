using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

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
    public int aggroRange;

    public int health;

    public int power = 3;

    // holds the 8 cardinal directions
    protected List<Vector2> cardinalDirections;

    [Tooltip("How many beats before the enemy does something")]
    public int beatsBetweenActions = 1;
    private int beatsUntilNextAction;

	public Vector2 targetPosition;
	private Queue<Vector2> path;

    public EnergyBar healthBar;
    public Slider healthSlider;

	// Start is called before the first frame update
	protected virtual void Start()
    {
        cardinalDirections = new List<Vector2>();
        // N
        cardinalDirections.Add(new Vector2(0, 1)); //0
        // S
        cardinalDirections.Add(new Vector2(0, -1));
        // E
        cardinalDirections.Add(new Vector2(1, 0)); //2
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

        healthSlider.maxValue = health;
        healthBar.Energy = health;
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
			//gets the closest position that puts the player in shooting range
			Vector2 newTargetPosition = getClosestInRangeOfPlayer();

			if(newTargetPosition != targetPosition || path == null || path.Count < 1)
			{
				targetPosition = newTargetPosition;
				path = planAStarPath();
			}

			//do a astar towards the target loc
			if (Vector2.Distance(player.transform.position, transform.position) < aggroRange) 
				Move(path.Dequeue()-(Vector2)transform.position);




			/*if(Vector2.Distance(player.transform.position, transform.position) < aggroRange)
			{
				//moves the enemy in the best direction
				Move(getNextAStarDirection());
			}*/
			//Move(cardinalDirections[Random.Range(0, 4)]);
		}
	}

	
    private void Move(Vector2 direction)
    {
        if (CanMove(direction))
        {
            StartLerpMovement(transform.position + (Vector3)direction);
        }
    }

	private bool IsOpenTile(Vector2 position)
	{
		Vector3Int gridPosition = floor.WorldToCell((Vector3)position);
		if (!floor.HasTile(gridPosition) || collideable.HasTile(gridPosition) || props.HasTile(gridPosition))
		{
			return false;
		}
		else
		{
			return true;
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
        healthBar.Energy -= amount;
        if (healthBar.Energy <= 0)
        {
            Die();
        }
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

	//finds out where around the player to try to get to that puts the player in firing range
	private Vector2 getClosestInRangeOfPlayer()
	{

		Vector2 closestTargetLoc = cardinalDirections[0] + (Vector2)player.transform.position;
		float closestDist = Vector2.Distance(transform.position, closestTargetLoc);
		foreach (Vector2 cardinalDir in cardinalDirections)
		{
			Vector2 posToCheck = (Vector2)player.transform.position + (cardinalDir * projectileRange);
			float dist = Vector2.Distance(transform.position, posToCheck);

			//RaycastHit2D hit = Physics2D.Raycast(posToCheck, cardinalDir);
			//playerInLoF = hit.collider.gameObject.tag == "Player";

			if (dist < closestDist/* && IsOpenTile(posToCheck)*/)
			{
				closestTargetLoc = posToCheck;
			}
		}
		return closestTargetLoc;
	}

	private Queue<Vector2> planAStarPath()
	{
		Queue<Vector2> pathNodes = new Queue<Vector2>();
		Vector2 cur = transform.position;
		Vector2 nextNode = transform.position;

		for(int p = 0; p < 100 && cur != targetPosition; p++)
		//while(!pathNodes.Contains(targetPosition))
		{
			//gets possible positions
			List<Vector2> possiblePos = new List<Vector2>();
			for (int i = 0; i < 4; i++)
			{
				Vector2 dir = cardinalDirections[i];
				Vector2 posToCheck = cur + dir;
				if (IsOpenTile(posToCheck)==true && !pathNodes.Contains(posToCheck)==true)
				{
					possiblePos.Add(posToCheck);
				}
			}

			//if the enemy cant go anywhere, return a null
			if (possiblePos.Count < 1) return pathNodes;

			float dist = Vector2.Distance(possiblePos[0], targetPosition);
			nextNode = possiblePos[0];

			//gets the next node
			foreach (Vector2 pos in possiblePos)
			{
				//gets the current spot to check and its distance to the targetlocation
				float distToCheck = Vector2.Distance(pos, targetPosition); 

				//if the enemy can move there, and the enemy hasn't moved there yet, and its the closest move
				if (distToCheck < dist && !pathNodes.Contains(pos))
				{
					//add th
					dist = distToCheck;
					nextNode = pos;
				}
			}

			//queues the position in the path
			cur = nextNode;
			pathNodes.Enqueue(nextNode);
		}
		return pathNodes;

	}
    
    #region LerpMovement
    // Update is called once per frame
    void Update()
    {
        if (moving) LerpMovement();
    }

    // Fields needed for movement
    private bool moving = false;
    private Vector3 nextSquarePosition = Vector3.zero;
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
            transform.position = Vector3.Lerp(startingPosition, nextSquarePosition, t);

            // Movement finished
            if (t == 1)
            {
                moving = false;
                nextSquarePosition = Vector3.zero;
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
        nextSquarePosition = targetPos;
        moving = true;
    }
    #endregion
}
