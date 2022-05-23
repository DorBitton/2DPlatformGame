using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    public float raycastDistance = 0.2f;
    public LayerMask layerMask;

    // Flags
    public bool below;

    private Vector2 _moveAmount;
    private Vector2 _currentPosition;
    private Vector2 _lastPosition;


    private Rigidbody2D _rigidBody;
    private CapsuleCollider2D _capsuleCollider;

    private Vector2[] _rayCastPosition = new Vector2[3];
    private RaycastHit2D[] _raycastHits = new RaycastHit2D[3];

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = gameObject.GetComponent<Rigidbody2D>();
        _capsuleCollider = gameObject.GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Moving the player
        _lastPosition = _rigidBody.position;
        _currentPosition = _lastPosition + _moveAmount;
        _rigidBody.MovePosition(_currentPosition);
        _moveAmount = Vector2.zero;

        CheckGrounded();
    }

    public void Move(Vector2 movment)
    {
        _moveAmount += movment;
    }

    private void CheckGrounded()
    {
        Vector2 raycastOrigin = _rigidBody.position - new Vector2(0, _capsuleCollider.size.y * 0.5f);
        _rayCastPosition[0] = raycastOrigin + (Vector2.left * _capsuleCollider.size.x * 0.25f + Vector2.up * 0.1f);
        _rayCastPosition[1] = raycastOrigin;
        _rayCastPosition[2] = raycastOrigin + (Vector2.right * _capsuleCollider.size.x * 0.25f + Vector2.up * 0.1f);
        DrawDebugRays(Vector2.down, Color.green);

        int numberOfGroundHits = 0;

        for (int i = 0; i < _rayCastPosition.Length; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(_rayCastPosition[i], Vector2.down, raycastDistance, layerMask);

            if (hit.collider)
            {
                _raycastHits[i] = hit;
                numberOfGroundHits++;
            }
        }
        if (numberOfGroundHits > 0)
            below = true;
        else
            below = false;

    }

    private void DrawDebugRays(Vector2 direction, Color color)
    {
        for (int i = 0; i < _rayCastPosition.Length; i++)
        {
            Debug.DrawRay(_rayCastPosition[i], direction * raycastDistance, color);
        }
    }
}
