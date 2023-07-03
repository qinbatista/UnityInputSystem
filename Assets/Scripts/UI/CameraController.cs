using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] Collider _edgeCollider;
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    [SerializeField] float MaxFoV = 100;
    [SerializeField] float MinFoV = 60;
    Vector3 _moveDirection = new Vector3(0, 0, 0);
    Vector3 _inputDirection = new Vector3(0, 0);
    float _moveSpeed = 20.0f;
    Vector3 _leftBottom;
    Vector3 _rightTop;
    bool _finger1touch;
    float _previousDistance;
    Vector2 _finger0position;
    Vector2 _finger1position;
    void Start()
    {
        _leftBottom = _edgeCollider.bounds.min;
        _rightTop = _edgeCollider.bounds.max;
    }
    void Update()
    {
        if (_inputDirection != Vector3.zero)//move horizontally and vertically
        {
            _moveDirection = transform.forward * -_inputDirection.x;
            if ((transform.position.x > _leftBottom.x && transform.position.x < _rightTop.x))
            {
                // if(transform.position.z > _leftBottom.z && transform.position.z < _rightTop.z)
                if ((transform.position.z > _leftBottom.z && transform.position.z < _rightTop.z))
                {
                    // print("in the z-zone");
                    transform.position += _moveDirection * Time.deltaTime;
                }
                else
                {
                    // print("out the z-zone");
                    if (transform.position.z <= _leftBottom.z && _moveDirection.z > 0)//check if the camera is within the boundary left and right
                    {
                        // print("bottom most but moving up a bit");
                        transform.position += _moveDirection * Time.deltaTime;
                    }
                    else if (transform.position.z >= _rightTop.z && _moveDirection.z < 0)//check if the camera is within the boundary left and right
                    {
                        // print("top most but moving down a bit");
                        transform.position += _moveDirection * Time.deltaTime;
                    }
                }
            }
            else
            {
                if (transform.position.x <= _leftBottom.x && _moveDirection.x > 0)//check if the camera is within the boundary forward and back
                {
                    // print("left most but moving right a bit");
                    transform.position += _moveDirection * Time.deltaTime;
                }
                else if (transform.position.x >= _rightTop.x && _moveDirection.x < 0)//check if the camera is within the boundary forward and back
                {
                    // print("right most but moving left a bit");
                    transform.position += _moveDirection * Time.deltaTime;
                }

            }
            _moveDirection = transform.right * _inputDirection.y;
            if ((transform.position.z > _leftBottom.z && transform.position.z < _rightTop.z))
            {
                if ((transform.position.x > _leftBottom.x && transform.position.x < _rightTop.x))
                {
                    // print("in the x-zone");
                    transform.position += _moveDirection * Time.deltaTime;
                }
                else
                {
                    // print("out the x-zone");
                    if (transform.position.x <= _leftBottom.x && _moveDirection.x > 0)//check if the camera is within the boundary left and right
                    {
                        // print("bottom most but moving up a bit");
                        transform.position += _moveDirection * Time.deltaTime;
                    }
                    else if (transform.position.x >= _rightTop.x && _moveDirection.x < 0)//check if the camera is within the boundary left and right
                    {
                        // print("top most but moving down a bit");
                        transform.position += _moveDirection * Time.deltaTime;
                    }
                }
            }
            else
            {
                if (transform.position.z <= _leftBottom.z && _moveDirection.z > 0)//check if the camera is within the boundary forward and back
                {
                    // print("left most but moving right a bit");
                    transform.position += _moveDirection * Time.deltaTime;
                }
                else if (transform.position.z >= _rightTop.z && _moveDirection.z < 0)//check if the camera is within the boundary forward and back
                {
                    // print("right most but moving left a bit");
                    transform.position += _moveDirection * Time.deltaTime;
                }

            }
        }
        if (_finger1touch)
        {
            float distance = Vector2.Distance(_finger0position, _finger1position);
            if(distance<10)return;
            if (distance > _previousDistance)//zoom in
            {
                virtualCamera.m_Lens.FieldOfView -= 70 * Time.deltaTime;
                if (virtualCamera.m_Lens.FieldOfView < 60)
                    virtualCamera.m_Lens.FieldOfView = 60;

            }
            else if (distance < _previousDistance)//zoom out
            {
                virtualCamera.m_Lens.FieldOfView += 70 * Time.deltaTime;
                if (virtualCamera.m_Lens.FieldOfView > 100)
                    virtualCamera.m_Lens.FieldOfView = 100;
            }
            _previousDistance = distance;
        }
    }
    public void KeyboardMove(InputAction.CallbackContext context)
    {
        // Debug.Log("KeyboardMove=" + context.ReadValue<Vector2>());
        _inputDirection = new Vector3(-context.ReadValue<Vector2>().normalized.x, -context.ReadValue<Vector2>().normalized.y) * _moveSpeed;
    }
    public void TouchMove(InputAction.CallbackContext context)
    {
        if (_finger1touch)
        {
            _inputDirection = Vector3.zero;
            return;
        }
        // Debug.Log("TouchMove=" + context.ReadValue<Vector2>());
        if (Mathf.Abs(context.ReadValue<Vector2>().x) < 2)
            _inputDirection.x = 0;
        else
            _inputDirection.x = context.ReadValue<Vector2>().x - context.ReadValue<Vector2>().x > 0 ? 2 : -2;
        if (Mathf.Abs(context.ReadValue<Vector2>().y) < 2)
            _inputDirection.y = 0;
        else
            _inputDirection.y = context.ReadValue<Vector2>().y - context.ReadValue<Vector2>().y > 0 ? 2 : -2;
        _inputDirection = new Vector3(context.ReadValue<Vector2>().x, context.ReadValue<Vector2>().y);
    }
    public void Finger0Position(InputAction.CallbackContext context)
    {
        // Debug.Log("Finger0Position=" + context.ReadValue<Vector2>());
        _finger0position = context.ReadValue<Vector2>();
    }
    public void Finger1Position(InputAction.CallbackContext context)
    {
        // Debug.Log("Finger1Position=" + context.ReadValue<Vector2>());
        _finger1position = context.ReadValue<Vector2>();
    }
    public void Finger1Touch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _previousDistance = 0f;
            _finger1touch = true;
        }
        else
            _finger1touch = false;
        // Debug.Log("_finger1touch=" + _finger1touch);
    }
}
