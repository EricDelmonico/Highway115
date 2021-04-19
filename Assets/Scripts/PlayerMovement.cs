using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    private ControlsInput controls;

    [SerializeField]
    private Tilemap floor;
    [SerializeField]
    private Tilemap collideable;
    [SerializeField]
    private Tilemap props;

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
        controls.Movement.Move.performed += ctx => Move(ctx.ReadValue<Vector2>());
    }

    private void Move(Vector2 direction)
    {
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
}
