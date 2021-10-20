using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float _speed = 12f;
    float _gravity = -9.81f;
    float _groundDistance = 0.4f;
    float _jumpHeight = 2f;
    bool _isGrounded;

    Vector3 _velocity;

    CharacterController _controller = null;
    [SerializeField] Transform _groundCheck = null;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Movement();
        Gravity();
    }
    void Movement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        _controller.Move(move * _speed * Time.deltaTime);

    }
    void Gravity()
    {
        _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundDistance, 12);

        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }

        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
        }

        _velocity.y += _gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);
    }

}
