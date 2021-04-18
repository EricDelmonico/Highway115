using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
	public enum Control	{ UP, DOWN, LEFT, RIGHT, SHOOT, JUMP }
	
	//conductor
	public GameObject ConductorObject;
	Conductor conductor;

	//movement
	Vector2 direction;
	public static Vector2 Forward;
	Vector2 right;
	public float speed;

	GameObject projectile;
	int energy;
	public int maxDamage;

	//so player knows what projectiles to not take damage from
	List<GameObject> shotProjectiles = new List<GameObject>();

	//controlls for easier editting
	public Dictionary<Control, KeyCode> controls = new Dictionary<Control, KeyCode>(){
		{Control.UP, KeyCode.UpArrow},
		{Control.DOWN, KeyCode.DownArrow},
		{Control.LEFT, KeyCode.LeftArrow},
		{Control.RIGHT, KeyCode.RightArrow},
		{Control.SHOOT, KeyCode.Space}
	};
	

    // Start is called before the first frame update
    void Start()
    {
		Forward = new Vector2(1, 1);
		right = new Vector2(Forward.y, Forward.x);
	}

	void Move()
	{
		//Movement stuff
		//todo slerp it :P
		transform.position += (Vector3)direction * speed;
	}

	/// <summary>
	/// Shoots a projectile in the direction the player is facing with damage corresponding to the beat accuracy.
	/// </summary>
	void Shoot()
	{
		int beatAccuracy = (int)conductor.CheckBeatAccuracy();
		
		//if the player missed
		if (beatAccuracy == 3) return;

		GameObject bullet = Instantiate(projectile);

		//so player knows not to take damage from it.
		shotProjectiles.Add(bullet); 
		
		//bullet.GetComponent<Projectile>.direction = this.direction;
		//bullet.GetComponent<Projectile>.damage = maxDamage - beatAccuracy;
		energy--;
	}

    // Update is called once per frame
    void Update()
	{
        if(Input.GetKey(controls[Control.UP]))
		{
			direction = Forward;//transform.Translate(Forward);
			Move();
			//energy += (int)conductor.CheckBeatAccuracy() - 4;
		}
        else if(Input.GetKey(controls[Control.DOWN]))
		{
			direction = -Forward;//transform.Translate(-Forward);'
			Move();
			//energy += (int)conductor.CheckBeatAccuracy() - 4;
		}
		else if (Input.GetKey(controls[Control.LEFT]))
		{
			direction = -right; //transform.Translate(-right);
			Move();

			//energy += (int)conductor.CheckBeatAccuracy() - 4;

		}
		else if(Input.GetKey(controls[Control.RIGHT]))
		{
			direction = right; //transform.Translate(right);
			Move();

			//energy += (int)conductor.CheckBeatAccuracy() - 4;
		}
		else if (Input.GetKey(controls[Control.SHOOT]))
		{
			Shoot();
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		GameObject other = collision.otherCollider.gameObject;

		//if the other collision is a player projectile
		if (shotProjectiles.Contains(other)) return;

		Projectile otherProjectile = other.GetComponent<Projectile>();
		//Enemy otherEnemy = other.GetComponent<Enemy>();

		//hit by a projectile or enemy
		//perhaps check the tags instead
		if (otherProjectile) //&& other.origin != gameObject
		{
			energy -= otherProjectile.damage;
		}
		/*else if (otherEnemy)
		{
			energy -= otherEnemy.damage; //?
		}*/
	}
}