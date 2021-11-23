using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    // Caching
    private Transform _transform;
    private Rigidbody2D _rigidbody;
    [SerializeField]
    private Paddle paddle;
    private GUIController guiController;

    // When not in play, follow the paddle
    private bool inPlay = false;
    public bool InPlay { get { return inPlay; } set { inPlay = value; } }

    private float startY;

    // Stats
    [SerializeField]
    [Range(1f, 100f)]
    private float launchSpeed = 10f;
    [SerializeField]
    [Range(1f, 170f)]
    private float launchAngleRange = 90f;

    private void Awake()
    {
        _transform = transform;
        _rigidbody = GetComponent<Rigidbody2D>();

        startY = _transform.position.y;
    }

    private void Start()
    {
        guiController = GUIController.Instance;
    }

    private void Update()
    {
        guiController.SetSpeedText(_rigidbody.velocity.magnitude);
    }

    private void FixedUpdate()
    {
        if (!inPlay)
        {
            _rigidbody.MovePosition(new Vector2(paddle._Transform.position.x, startY));
        }
    }

    public void Launch()
    {
        // if (inPlay) return;

        Vector2 launch = Util.DegreeToVector2(Random.Range(-0.5f * launchAngleRange, 0.5f * launchAngleRange)) * launchSpeed;
        _rigidbody.AddForce(launch, ForceMode2D.Impulse);

        inPlay = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("BoundsBottom"))
        {
            _rigidbody.velocity = Vector2.zero; // Reset velocity because rb.MovePosition() maintains old velocity
            _transform.position = new Vector2(paddle._Transform.position.x, startY); // Use instead of rb.MovePosition() to avoid hitting paddle on the way
            inPlay = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Bounce Dir = Ball Centre - Paddle Centre to replicate Breakout's bounce behaviour
        Vector2 bounceDir = _transform.position - collision.transform.position;
        bounceDir.x *= 0.5f; // Halve x to avoid horizontal/very shallow bounces
        _rigidbody.velocity = bounceDir.normalized * launchSpeed;
    }
}
