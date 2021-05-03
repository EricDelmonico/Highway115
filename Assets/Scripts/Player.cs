using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public enum Direction
{
	Up,
	Down,
	Left,
	Right
}

public class Player : MonoBehaviour
{
	private ControlsInput controls;

	private Direction direction;

	//movement
	//Vector2 direction;
	public static Vector2 Forward;
	Vector2 right;
	public float speed;

	public GameObject projectilePrefab;

	public EnergyBar energyBar;
	public UnityEngine.UI.Image energyBarFill;

	public GameObject trapPrefab;

	public Sprite frontSprite;
	public Sprite leftSprite;
	public Sprite backSprite;

	private List<ParticleSystem> particles;

	//int hp;
	public int maxDamage = 2;

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
		energyBarFill = energyBar.gameObject.transform.GetChild(1).GetComponent<UnityEngine.UI.Image>();
		particles = new List<ParticleSystem>()
		{
			transform.GetChild(3).GetComponent<ParticleSystem>(),
			transform.GetChild(1).GetComponent<ParticleSystem>(),
			transform.GetChild(2).GetComponent<ParticleSystem>(),
			transform.GetChild(1).GetComponent<ParticleSystem>()
		};
	}

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

	private void ShootInCurrentDirection(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
		switch (direction)
        {
			case Direction.Up:
				Shoot(new Vector2(0, 1));
				break;
			case Direction.Down:
				Shoot(new Vector2(0, -1));
				break;
			case Direction.Left:
				Shoot(new Vector2(-1, 0));
				break;
			case Direction.Right:
				Shoot(new Vector2(1, 0));
				break;
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
		SceneManager.LoadScene("GameOver");
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
			controls.Player.ShootStraight.performed += ShootInCurrentDirection;
			controls.Player.Trap.performed += ctx => PlaceTrap();

			controls.Player.Move.performed -= Calibrate;

			// Move since calibration is done
			ChangeSpriteDirection(context.ReadValue<Vector2>());
			Move(context.ReadValue<Vector2>());
		}
	}

	private void ChangeSpriteDirection(Vector2 vecDirection)
    {
		// Change direction
		if (vecDirection == new Vector2(0, 1))
		{
			direction = Direction.Up;
		}
		if (vecDirection == new Vector2(0, -1))
		{
			direction = Direction.Down;
		}
		if (vecDirection == new Vector2(1, 0))
		{
			direction = Direction.Right;
		}
		if (vecDirection == new Vector2(-1, 0))
		{
			direction = Direction.Left;
		}

		SpriteRenderer sr = GetComponent<SpriteRenderer>();

		// don't flip by default
		sr.flipX = false;
        
		// Change sprite
		switch (direction)
        {
			case Direction.Up:
				sr.sprite = backSprite;
				break;
			case Direction.Down:
				sr.sprite = frontSprite;
				break;
			case Direction.Left:
				sr.sprite = leftSprite;
				break;
			case Direction.Right:
				sr.sprite = leftSprite;
				sr.flipX = true;
				break;
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

		particles[(int)feedback].Play();

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

		float secPerBeat = Conductor.Instance.secondsPerBeat;
		float secondsSinceLastBeat = Conductor.Instance.songPosition - Conductor.Instance.lastBeatSeconds;
		float brightness = Mathf.Pow(Mathf.Sin(secondsSinceLastBeat / secPerBeat * Mathf.PI), .5f);


		//I originally tried this, but it's hard to feel the beat with. 
		//if yall still want it pick this one, but otherwise I think the one-color pulse looks better
		#region optionOneRainbow
		//int beatsSoFar = (int)(Conductor.Instance.songPosition / Conductor.Instance.secondsPerBeat);

		//energyBarFill.color = new Color(
		//	brightness * ((beatsSoFar % 3) == 0 ? 0 : 1),
		//	brightness * ((beatsSoFar % 3) == 1 ? 0 : 1),
		//	brightness * ((beatsSoFar % 3) == 2 ? 0 : 1)
		//);

		//GetComponent<SpriteRenderer>().color = new Color(
		//	1-brightness * ((beatsSoFar % 3) == 0 ? 0 : 1),
		//	1-brightness * ((beatsSoFar % 3) == 1 ? 0 : 1),
		//	1-brightness * ((beatsSoFar % 3) == 2 ? 0 : 1)
		//);
		#endregion

		#region optionTwoPulse
		//pulses from white to blue (blue is on-beat)
		energyBarFill.color = new Color(
			brightness * (.5f),
			brightness * (.5f),
			255
		);

		//pulses from blue to normal color (blue is on-beat)
		GetComponent<SpriteRenderer>().color = new Color(
			brightness,
			brightness,
			255
		);
		#endregion
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