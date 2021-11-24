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
    [SerializeField] [Range(1f, 100f)]
    private float launchSpeedStart = 20f; // Initial ball speed
    [SerializeField]
    [Range(5f, 100f)]
    private float launchSpeedIncrement = 5f; // Increase ball speed by this each round
    private float launchSpeedCurrent;
    [SerializeField] [Range(1f, 170f)]
    private float launchAngleRange = 90f; // Launch Ball randomly in this range

    private void Awake()
    {
        _transform = transform;
        _rigidbody = GetComponent<Rigidbody2D>();
        launchSpeedCurrent = launchSpeedStart;
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

    // Launch ball in x- degree range
    public void Launch()
    {
        if (inPlay) return;

        Vector2 launch = Util.DegreeToVector2(Random.Range(-0.5f * launchAngleRange, 0.5f * launchAngleRange)) * launchSpeedCurrent;
        _rigidbody.AddForce(launch, ForceMode2D.Impulse);

        inPlay = true;
    }

    public void NextRound()
    {
        launchSpeedCurrent += launchSpeedIncrement;

        ReturnToPaddle();
    }

    private void ReturnToPaddle()
    {
        _rigidbody.velocity = Vector2.zero; // Reset velocity bc rb.MovePosition() maintains old velocity
        _transform.position = new Vector2(paddle._Transform.position.x, startY); // Use instead of MovePosition() to avoid hitting paddle
        inPlay = false;
    }

    // Collision with bottom => return to paddle, decrement lives
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("BoundsBottom"))
        {
            ReturnToPaddle();
        }
    }

    // Collision with paddle => bounce
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Vector2 bounceDir = _transform.position - collision.transform.position; // Ball - Paddle to replicate Breakout's bounce
        bounceDir.x *= 0.5f; // Halve x to avoid horizontal/very shallow bounces
        _rigidbody.velocity = bounceDir.normalized * launchSpeedCurrent;
    }
}
