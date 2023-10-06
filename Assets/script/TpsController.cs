using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TpsController : MonoBehaviour
{

    CharacterController _controller;
    float _horizontal;
    float _vertical;
    [SerializeField] float _speed = 6.0f;
    Transform _camera;

    [SerializeField] float _jumpHeight = 1;
    float _gravity = -9.81f;
    Vector3 _playerGravity;

    float _turnSmoothVelocity;
    [SerializeField] float turnSmoothTime =0.1f;

    [SerializeField] private Transform _sensorPosition;
    [SerializeField] float _sensorRadius = 0.2f;
    [SerializeField] LayerMask _groundLayer;
    bool _isGrounded;


    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _camera = Camera.main.transform;
        
    }

    // Update is called once per frame
    void Update()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical");

        if (Input.GetButton("Fire2"))
        {
            AimMovement();
        }
        else
        {
            Movement();
        }
       Jump();   
    }

   
    void Movement()
    {
        
        Vector3 direction = new Vector3 (_horizontal , 0 , _vertical);

        if (direction != Vector3.zero)
        {
            float _targetAngle = Mathf.Atan2(direction.x , direction.z) * Mathf.Rad2Deg + _camera.eulerAngles.y;
            float _smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetAngle , ref _turnSmoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0, _smoothAngle, 0);
            Vector3 _moveDirection = Quaternion.Euler(0, _targetAngle , 0) * Vector3.forward;
            _controller.Move(_moveDirection.normalized * _speed * Time.deltaTime);
        }

       
    }

     void AimMovement()
    {
        
        Vector3 direction = new Vector3 (_horizontal , 0 , _vertical);

        float _targetAngle = Mathf.Atan2(direction.x , direction.z) * Mathf.Rad2Deg + _camera.eulerAngles.y;
        float _smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _camera.eulerAngles.y , ref _turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0, _smoothAngle, 0);

        if (direction != Vector3.zero)
        {
            Vector3 _moveDirection = Quaternion.Euler(0, _targetAngle , 0) * Vector3.forward;
            _controller.Move(_moveDirection.normalized * _speed * Time.deltaTime);
        }

       
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
