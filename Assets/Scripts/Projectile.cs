using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 direction;
    public int tilesPerBeat = 2;
    public int power = 1;
    public string target = "Player";
    // ^^^ All set by creator (or prefab)
    //private int tileLength;

    private void Start()
    {
        Conductor.Instance.BeatOccurred += OnBeat;
        // Start moving instantly
        StartLerpMovement(transform.position + direction * tilesPerBeat);
    }

    /// <summary>
    /// Is supposed to be triggered once every beat by BeatOccurred, but I don't know how to do that.
    /// </summary>
    protected virtual void OnBeat(object sender, System.EventArgs e)
    {
        Move();
    }

    /// <summary>
    /// Moves
    /// </summary>
    protected virtual void Move()
    {
        StartLerpMovement(transform.position + direction * tilesPerBeat); //Normalize(direction) + tileLength;
    }

    private void OnDestroy()
    {
        Conductor.Instance.BeatOccurred -= OnBeat;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == target)
        {
            //collision.gameObject.GetComponent(Entity).Damage(power); // Shouldn't they both inherit from an entity class?
            if (target == "Player")
            {
                collision.gameObject.GetComponent<Player>().TakeDamage(power);
            }
            else
            {
                collision.gameObject.GetComponent<Enemy>().Damage(power);
            }
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "Obstacle")
        {
            Destroy(gameObject);
        }
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
