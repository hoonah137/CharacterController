using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Shoulder : MonoBehaviour
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

    [SerializeField] private AxisState xAxis;
    [SerializeField] private AxisState yAxis;
    private Transform _lookAtTransform;



    // Start is called before the first frame update
    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _camera = Camera.main.transform;
        _lookAtTransform = GameObject.Find("LookAt").transform;
    }

    // Update is called once per frame
    void Update()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical");

      
       Jump(); 
       Movement();  
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

    void Movement()
    {
        Vector3 move = new Vector3 (_horizontal, 0, _vertical).normalized;
        xAxis.Update(Time.deltaTime);
        yAxis.Update(Time.deltaTime);

        transform.rotation = Quaternion.Euler(0,xAxis.Value,0);
        _lookAtTransform.eulerAngles = new Vector3(yAxis.Value, xAxis.Value, 0);

    //HAce que se mueva con la camara 
        if(move != Vector3.zero)
        {
            float _targetAngle= Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg+_camera.eulerAngles.y;
            Vector3 moveDirection = Quaternion.Euler(0,_targetAngle,0) * Vector3.forward;
            _controller.Move(moveDirection.normalized * _speed * Time.deltaTime);
        }     

    }
}
