using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Player Properties
    public float walkSpeed = 10f;
    public float gravity = 20f;
    public float jumpSpeed = 15f;

    // Player state
    public bool isJumping;

    // Input flags
    private bool _startJump;
    private bool _releaseJump;


    private Vector2 _input;
    private Vector2 _moveDirection;
    private CharacterController2D _characterController;
    //Testing
    // Start is called before the first frame update
    void Start()
    {
        _characterController = gameObject.GetComponent<CharacterController2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _moveDirection.x = _input.x;
        _moveDirection.x *= walkSpeed;

        if (_characterController.below)
        {
            _moveDirection.y = 0f;
            isJumping = false;

            if (_startJump)
            {
                _startJump = false;
                _moveDirection.y = jumpSpeed;
                isJumping = true;
                _characterController.DisableGrounCheck();
            }
        }
        else // In the air
        {
            if (_releaseJump)
            {
                _releaseJump = false;
                if (_moveDirection.y > 0)
                {
                    _moveDirection.y *= 0.5f;
                }
            }
            _moveDirection.y -= gravity * Time.deltaTime;
        }

        _characterController.Move(_moveDirection * Time.deltaTime);
    }

    // Input methods
    public void OnMovement(InputAction.CallbackContext context)
    {
        _input = context.ReadValue<Vector2>(); // Any Input getting from the user like WASD
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _startJump = true;

        }
        else if (context.canceled)
        {
            _releaseJump = true;
        }
    }
}
