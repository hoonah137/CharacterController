using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcoController : MonoBehaviour
{

    CharacterController _controller;
    float _horizontal;
    float _vertical;
    [SerializeField] float _speed = 6.0f;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical");
        Movement();     
        Jump();   
    }

   
    void Movement()
    {
        
        Vector3 direction = new Vector3 (_horizontal , 0 , _vertical);

        Ray ray = Camera.main.ScreenPointPointToray(Input.mousePosition);
        Raycast hit;
        if(Physics.Raycast(ray, out hit, MathF.Infinity))
        {
            Debug.DrawLine(Camera.main.transform.position, hit.point);
        }

        if (direction != Vector3.zero)
        {
            float _targetAngle = Mathf.Atan2(direction.x , direction.z) * Mathf.Rad2Deg;
            float _smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetAngle, ref _turnSmoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0, _smoothAngle, 0);
        }
        _controller.Move(direction.normalized * _speed * Time.deltaTime);
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
