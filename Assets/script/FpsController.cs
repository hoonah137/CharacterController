using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsController : MonoBehaviour
{
    [SerializeField] float _speed;
    [SerializeField] float _jumpHeight;
    [SerializeField] float _sensivility = 50;

    [SerializeField] private Transform _sensorPosition;
    [SerializeField] float _sensorRadius = 0.2f;
    [SerializeField] LayerMask _groundLayer;
    bool _isGrounded;
    float _gravity = -9.81f;
    Vector3 _playerGravity;
    

    Transform _fpsCamera;
    float _xRotation = 0;
    CharacterController _controller;

    // Start is called before the first frame update
    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _fpsCamera = Camera.main.transform;

    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
        
    }

    void Move()
    {
        float mouseX = Input.GetAxis("Mouse X") * _sensivility * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _sensivility * Time.deltaTime;
        //Get mouse input

        _xRotation -= mouseY;
        //rotation
        _xRotation = Mathf.Clamp(_xRotation, -90, 90);
        //limit rotation
        
        _fpsCamera.localRotation = Quaternion.Euler(_xRotation,0,0);

        transform.Rotate(Vector3.up * mouseX);

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        
        Vector3 move = transform.right * x + transform.forward * z;
        _controller.Move(move.normalized * _speed * Time.deltaTime);
    }

     void Jump()
    {
        _isGrounded = Physics.CheckSphere(_sensorPosition.position, _sensorRadius, _groundLayer);

        if (_isGrounded && _playerGravity.y < 0)
        {
            _playerGravity.y = 0;
        }

        if (_isGrounded && Input.GetButtonDown("Jump"))
        {
            _playerGravity.y = Mathf.Sqrt(_jumpHeight * -2 * _gravity);
        }

        _playerGravity.y += _gravity * Time.deltaTime;

        _controller.Move(_playerGravity * Time.deltaTime);


    }
}
