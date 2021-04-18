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
	//int hp;
	public int maxDamage;

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
		int beatAccuracy = (int)conductor.CheckBeatAccuracy();
		energy += 3-beatAccuracy;
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

		bullet.transform.position = this.transform.position + (Vector3)direction;
		
		bullet.GetComponent<Projectile>.direction = this.direction;
		bullet.GetComponent<Projectile>.damage = maxDamage - beatAccuracy;
		
		energy--;
	}

    // Update is called once per frame
    void Update()
	{
        if(Input.GetKey(controls[Control.UP]))
		{
			direction = Forward;
			Move();
		}
        else if(Input.GetKey(controls[Control.DOWN]))
		{
			direction = -Forward;
			Move();
		}
		else if (Input.GetKey(controls[Control.LEFT]))
		{
			direction = -right; 
			Move();
		}
		else if(Input.GetKey(controls[Control.RIGHT]))
		{
			direction = right;
			Move();
		}
		else if (Input.GetKey(controls[Control.SHOOT]))
		{
			Shoot();
		}
	}

	void takeDamage(int damage)
	{
		energy -= damage;
		if(energy < 0)
		{
			Die();
		}

	}

	void Die()
	{
		//dies
	}
}