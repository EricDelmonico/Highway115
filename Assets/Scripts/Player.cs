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

	public GameObject trapPrefab;

	public Sprite frontSprite;
	public Sprite leftSprite;
	public Sprite backSprite;

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

		controls.Player.Move.performed += Calibrate;
		controls.Player.ToggleGUI.performed += (_) => Conductor.Instance.showGUI = !Conductor.Instance.showGUI;
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
		if (Menu.isPaused || moving) return;

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
		SceneManager.LoadScene(2);
	}

	#region Player Movement

	[SerializeField]
	private Tilemap floor;
	[SerializeField]
	private Tilemap collideable;
	[SerializeField]
	private Tilemap props;

	private void Calibrate(UnityEngine.InputSystem.InputAction.CallbackContext context)
	{
		if (Menu.isPaused) return;

		bool calibrated = Conductor.Instance.CalibrateOffset();

		if (calibrated)
		{
			// Enable moving and shooting, and change direction on moving and shooting
			controls.Player.Move.performed += ctx =>
			{
				ChangeSpriteDirection(ctx.ReadValue<Vector2>());
				Move(ctx.ReadValue<Vector2>());
			};
			controls.Player.Shoot.performed += ctx =>
			{
				ChangeSpriteDirection(ctx.ReadValue<Vector2>());
				Shoot(ctx.ReadValue<Vector2>());
			};
			controls.Player.Trap.performed += ctx => PlaceTrap();

			controls.Player.Move.performed -= Calibrate;

			// Move since calibration is done
			ChangeSpriteDirection(context.ReadValue<Vector2>());
			Move(context.ReadValue<Vector2>());
		}
	}

	private void ChangeSpriteDirection(Vector2 direction)
    {
		Vector2 up = new Vector2(0, 1);
		Vector2 down = new Vector2(0, -1);
		Vector2 right = new Vector2(1, 0);
		Vector2 left = new Vector2(-1, 0);

		SpriteRenderer sr = GetComponent<SpriteRenderer>();

		// don't flip by default
		sr.flipX = false;
		if (direction == up)
        {
            sr.sprite = backSprite;
        }
        if (direction == down)
        {
            sr.sprite = frontSprite;
        }
        if (direction == right)
        {
            sr.sprite = leftSprite;
			sr.flipX = true;
        }
        if (direction == left)
        {
            sr.sprite = leftSprite;
        }
    }

    private void PlaceTrap()
    {
		if (Menu.isPaused || moving) return;

		// TODO: Notify the player somehow??
		if (energyBar.Energy > 6)
        {
			GameObject newTrap = Instantiate(trapPrefab);
			newTrap.transform.position = transform.position;

			energyBar.Energy -= 5;
			if (energyBar.Energy <= 0)
			{
				Die();
			}
		}
	}

	private void Move(Vector2 direction)
	{
		if (Menu.isPaused || moving) return;

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
			// set the target and move
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

	#endregion
}