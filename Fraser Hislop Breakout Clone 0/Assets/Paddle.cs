using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    // Caching
    private Transform _transform; public Transform _Transform { get { return _transform; } }
    private Rigidbody2D _rigidbody;
    [SerializeField]
    private Ball ball;
    
    private float defaultYPos; // Constant Y Position when moving paddle
    private float halfWidth;

    // Stats
    [SerializeField]
    [Range(1f, 100f)]
    private float xSpeed = 10f;

    private void Awake()
    {
        _transform = transform;
        _rigidbody = GetComponent<Rigidbody2D>();

        defaultYPos = _transform.position.y;
        halfWidth = 0.5f * _transform.localScale.x;
    }

    private void Start()
    {
        
    }
    
    private void Update()
    {
        LaunchBall(Input.GetKeyDown(KeyCode.Space));
    }

    // Launch Ball in 90 degree cone
    private void LaunchBall(bool input)
    {
        if (input)
        {
            ball.Launch();
        }
    }

    private void FixedUpdate()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Movement(mouseWorldPos);
    }

    // Move Paddle to position's x
    private void Movement(Vector3 position)
    {
        position.x = Mathf.Lerp(_transform.position.x, position.x, Time.fixedDeltaTime * xSpeed); // lerp BEFORE clamping to avoid slowing near walls
        position.x = Mathf.Clamp(position.x, -20 + halfWidth, 20 - halfWidth); // clamp to be inside walls

        _rigidbody.MovePosition(new Vector3(position.x, defaultYPos, 0));
    }
}
