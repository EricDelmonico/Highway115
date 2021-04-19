using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector3 direction;
    public int tilesPerBeat = 1;
    public int power = 1;
    public string target = "Player";
    // ^^^ All set by creator (or prefab)
    //private int tileLength;

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
        transform.position += direction; //Normalize(direction) + tileLength;
    }

    /// <summary>
    /// Does damage to target collisions
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter(Collision collision) 
    {
        if (collision.gameObject.tag == target)
        {
            //collision.gameObject.GetComponent(Entity).Damage(power); // Shouldn't they both inherit from an entity class?
            if(target == "Player")
            {
                collision.gameObject.GetComponent<Player>().takeDamage(power);
            }
            else
            {
                collision.gameObject.GetComponent<Enemy>().Damage(power);
            }
            Destroy(gameObject);
        }
    }
}
