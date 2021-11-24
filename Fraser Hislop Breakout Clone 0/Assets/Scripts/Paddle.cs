using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Paddle : NetworkBehaviour
{
    // Caching
    private Transform _transform; public Transform _Transform { get { return _transform; } }
    private Rigidbody2D _rigidbody;
    public Ball ball;
    
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
        if (!isLocalPlayer) return; // exit from update if not local player

        if (Input.GetKeyDown(KeyCode.Space)) CmdLaunchBall();
    }

    // Launch Ball in 90 degree cone
    [Command]
    private void CmdLaunchBall()
    {
        ball.Launch();
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer) return; // exit from update if not local player

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
