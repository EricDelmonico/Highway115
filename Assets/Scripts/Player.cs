using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
	private ControlsInput controls;

	public enum Control	{ UP, DOWN, LEFT, RIGHT, SHOOT, JUMP }

	//movement
	//Vector2 direction;
	public static Vector2 Forward;
	Vector2 right;
	public float speed;

	public GameObject projectilePrefab;

	public EnergyBar energyBar;

	//int hp;
	public int maxDamage = 2;

	//controlls for easier editting
	//public Dictionary<Control, KeyCode> controls = new Dictionary<Control, KeyCode>(){
	//	{Control.UP, KeyCode.UpArrow},
	//	{Control.DOWN, KeyCode.DownArrow},
	//	{Control.LEFT, KeyCode.LeftArrow},
	//	{Control.RIGHT, KeyCode.RightArrow},
	//	{Control.SHOOT, KeyCode.Space}
	//};

	private void Awake()
	{
		controls = new ControlsInput();
	}

	private void OnEnable()
	{
		controls.Enable();
	}

	private void OnDisable()
	{
		controls.Disable();
	}

    // Start is called before the first frame update
    void Start()
    {
        Forward = new Vector2(0, 1);
        right = new Vector2(Forward.y, Forward.x);

		controls.Player.Shoot.performed += ctx => Shoot(ctx.ReadValue<Vector2>());
		controls.Player.Move.performed += Calibrate;
	}

	//void Move()
	//{
	//	//Movement stuff
	//	int beatAccuracy = (int)conductor.CheckBeatAccuracy();
	//	energy += 3-beatAccuracy;
	//	transform.position += (Vector3)direction * speed;
	//}

	/// <summary>
	/// Shoots a projectile in the direction the player is facing with damage corresponding to the beat accuracy.
	/// </summary>
	void Shoot(Vector2 direction)
	{
		int beatAccuracy = (int)Conductor.Instance.CheckBeatAccuracy();

		GameObject bullet = Instantiate(projectilePrefab);
		bullet.transform.position = transform.position;
		bullet.GetComponent<Projectile>().direction = direction;
		bullet.GetComponent<Projectile>().power = maxDamage - (beatAccuracy % 2);
		
		energyBar.Energy--;
		if (energyBar.Energy <= 0)
		{
			Die();
		}
	}

    // Update is called once per frame
    void Update()
	{
		// Old input
        //if(Input.GetKey(controls[Control.UP]))
		//{
		//	direction = Forward;
		//	Move();
		//}
        //else if(Input.GetKey(controls[Control.DOWN]))
		//{
		//	direction = -Forward;
		//	Move();
		//}
		//else if (Input.GetKey(controls[Control.LEFT]))
		//{
		//	direction = -right; 
		//	Move();
		//}
		//else if(Input.GetKey(controls[Control.RIGHT]))
		//{
		//	direction = right;
		//	Move();
		//}
		//else if (Input.GetKey(controls[Control.SHOOT]))
		//{
		//	Shoot();
		//}
	}

	public void TakeDamage(int damage)
	{
		energyBar.Energy -= damage;
		if(energyBar.Energy <= 0)
		{
			Die();
		}

	}

	void Die()
	{
		//dies--load game over
		SceneManager.LoadScene(1);
	}

	#region Player Movement

	[SerializeField]
	private Tilemap floor;
	[SerializeField]
	private Tilemap collideable;
	[SerializeField]
	private Tilemap props;

	private void Calibrate(UnityEngine.InputSystem.InputAction.CallbackContext _)
	{
		bool calibrated = Conductor.Instance.CalibrateOffset();

		if (calibrated)
		{
			controls.Player.Move.performed += ctx => Move(ctx.ReadValue<Vector2>());
			controls.Player.Move.performed -= Calibrate;
		}
	}

	private void Move(Vector2 direction)
	{
		HitFeedback feedback = Conductor.Instance.CheckBeatAccuracy();

        switch (feedback)
        {
			case HitFeedback.Perfect:
				energyBar.Energy += 4;
				break;
			case HitFeedback.Great:
				energyBar.Energy += 2;
				break;
			case HitFeedback.Early:
			case HitFeedback.Late:
				energyBar.Energy -= 1;
				break;
        }

		if (energyBar.Energy <= 0)
		{
			Die();
		}

		if (CanMove(direction))
		{
			transform.position += (Vector3)direction;
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

	#endregion
}