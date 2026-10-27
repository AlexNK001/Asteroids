using UnityEngine;
using Zenject;

public class UserInput : MonoBehaviour
{
    private Ship _ship;

    private readonly KeyCode _moveForwardKey = KeyCode.W;
    private readonly KeyCode _moveBackwardKey = KeyCode.S;
    private readonly KeyCode _moveLeftKey = KeyCode.A;
    private readonly KeyCode _moveRightKey = KeyCode.D;

    private float _speed;
    private Vector2 _velocity;
    private bool _isOppositeMovement;

    private SignalBus _signalBus;

    private void Awake()
    {
        _velocity = Vector2.up;
    }

    [Inject]
    private void Construct(SignalBus signalBus, Ship ship) 
    {
        _signalBus = signalBus;
        _ship = ship;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _signalBus.Fire(new PauseButtonPressedSignal());
        }

        bool isMoveForwardKeyPressed = Input.GetKey(_moveForwardKey);
        bool isMoveBackwardKeyPressed = Input.GetKey(_moveBackwardKey);

        float accelerationForward = 4f;
        float decelerationBackward = 2f;
        float speedDropOppositeMovement = 15f;
        float frictionDrag = 1f;
        float maxSpeed = 10f;
        float minSpeed = 1f;

        if (isMoveForwardKeyPressed)
        {
            if (_isOppositeMovement)
            {
                _speed -= speedDropOppositeMovement * Time.deltaTime;
            }
            else
            {
                _speed += accelerationForward * Time.deltaTime;
            }
        }
        else if (isMoveBackwardKeyPressed)
        {
            _speed -= decelerationBackward * Time.deltaTime;
        }
        else
        {
            _speed -= frictionDrag * Time.deltaTime;
        }

        _speed = Mathf.Clamp(_speed, minSpeed, maxSpeed);
        _ship.SetSpeed(_speed);

        float speedRotation = 90f;
        // float signLeft = 1f;
        // float signRight = -1f;
        float tempSpeedRotation = speedRotation * Time.deltaTime;

        if (Input.GetKey(_moveLeftKey))
        {
            // Rotation(signLeft);
            _ship.transform.Rotate(Vector3.forward,  tempSpeedRotation);
            _velocity = Quaternion.AngleAxis(tempSpeedRotation, Vector3.forward) * _velocity;
        }
        else if (Input.GetKey(_moveRightKey))
        {
            // Rotation(signRight);
            _ship.transform.Rotate(Vector3.back,  tempSpeedRotation);
            _velocity = Quaternion.AngleAxis(tempSpeedRotation, Vector3.back) * _velocity;
        }

        // void Rotation(float sign)
        // {
        //     _ship.transform.Rotate(Vector3.forward, sign * tempSpeedRotation);
        //     _velocity = Quaternion.AngleAxis(sign * tempSpeedRotation, Vector3.forward) * _velocity;
        // }

        float angleBackwardMovement = 90f;
        float minRotationSpeed = 1f;
        float maxRotationSpeed = 10f;

        if (isMoveForwardKeyPressed)
        {
            float reverseSpeedFactor = Mathf.InverseLerp(maxSpeed, minSpeed, _speed);
            float rotationSpeed = Mathf.Lerp(minRotationSpeed, maxRotationSpeed, reverseSpeedFactor);
            float maxDistanceDelta = rotationSpeed * Time.deltaTime;
            Vector2 direction = Vector2.MoveTowards(_ship.LookDirection, _velocity, maxDistanceDelta);

            float angle = Vector2.Angle(_velocity, direction);

            _isOppositeMovement = angle > angleBackwardMovement;
            bool isLittleSpeed = _speed < minSpeed + 0.1f;

            if (isLittleSpeed && _isOppositeMovement)
            {
                _ship.LookBack();
            }

            if (_isOppositeMovement == false)
            {
                _ship.LookAt(direction);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _signalBus.Fire(new RocketFiredSignal());
        }

        float normalizedSpeed = Mathf.InverseLerp(minSpeed, maxSpeed, _speed);
        _signalBus.Fire(new ShipSpeedChangedSignal(_speed, normalizedSpeed));
    }
}