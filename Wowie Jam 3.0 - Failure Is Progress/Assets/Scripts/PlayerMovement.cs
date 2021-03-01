using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    private GameManager gameManager;

    private Rigidbody2D _rigidbody;
    private Collider2D _collider;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private float movementDeltaX;
    private float movementSpeed = 5f;
    private Vector3 currentVelocity = Vector3.zero;
    private float movementDampening = 0.1f;
    private float jumpForce = 5.5f;
    private bool isJumping = false;
    private bool gravityBoost = true;
    private bool isGrounded;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start() {
        gameManager = GameManager.SharedInstance;
    }

    private void Update() {
        movementDeltaX = Input.GetAxisRaw("Horizontal") * movementSpeed;

        if (Input.GetKeyDown(KeyCode.Space)) isJumping = true;

        if ((_spriteRenderer.flipX && movementDeltaX > 0) || (!_spriteRenderer.flipX && movementDeltaX < 0)) {
            _spriteRenderer.flipX = !_spriteRenderer.flipX;
        }

        _animator.SetFloat("Speed", Mathf.Abs(movementDeltaX));
        _animator.SetBool("IsGrounded", isGrounded);
        _animator.SetFloat("yDirection", _rigidbody.velocity.y);
    }

    private void FixedUpdate() {

        Bounds playerBounds = _collider.bounds;

        bool willCollideRightTop = Physics2D.Raycast(new Vector2(playerBounds.center.x + playerBounds.extents.x, playerBounds.center.y + playerBounds.extents.y), Vector2.right, 0.1f);
        bool willCollideRightCenter = Physics2D.Raycast(new Vector2(playerBounds.center.x + playerBounds.extents.x, playerBounds.center.y), Vector2.right, 0.1f);
        bool willCollideRightBottom = Physics2D.Raycast(new Vector2(playerBounds.center.x + playerBounds.extents.x, playerBounds.center.y - playerBounds.extents.y), Vector2.right, 0.1f);
        bool willCollideRight = willCollideRightTop || willCollideRightCenter;

        bool willCollideLeftTop = Physics2D.Raycast(new Vector2(playerBounds.center.x - playerBounds.extents.x, playerBounds.center.y + playerBounds.extents.y), Vector2.left, 0.1f);
        bool willCollideLeftCenter = Physics2D.Raycast(new Vector2(playerBounds.center.x - playerBounds.extents.x, playerBounds.center.y), Vector2.left, 0.1f);
        bool willCollideLeftBottom = Physics2D.Raycast(new Vector2(playerBounds.center.x - playerBounds.extents.x, playerBounds.center.y - playerBounds.extents.y), Vector2.left, 0.1f);
        bool willCollideLeft = willCollideLeftTop || willCollideLeftCenter || willCollideLeftBottom;

        if (willCollideRight && movementDeltaX > 0 || willCollideLeft && movementDeltaX < 0) _rigidbody.velocity = Vector3.SmoothDamp(_rigidbody.velocity, new Vector2(0f, _rigidbody.velocity.y), ref currentVelocity, movementDampening);
        else _rigidbody.velocity = Vector3.SmoothDamp(_rigidbody.velocity, new Vector2(movementDeltaX, _rigidbody.velocity.y), ref currentVelocity, movementDampening);

        bool isGroundedLeft = Physics2D.Raycast(new Vector2(playerBounds.center.x - playerBounds.extents.x, playerBounds.center.y - playerBounds.extents.y), Vector2.down, 0.1f);
        bool isGroundedCenter = Physics2D.Raycast(new Vector2(playerBounds.center.x, playerBounds.center.y - playerBounds.extents.y), Vector2.down, 0.1f);
        bool isGroundedRight = Physics2D.Raycast(new Vector2(playerBounds.center.x + playerBounds.extents.x, playerBounds.center.y - playerBounds.extents.y), Vector2.down, 0.1f);
        isGrounded = isGroundedLeft || isGroundedCenter || isGroundedRight;

        if (_rigidbody.velocity.y < 0 && gravityBoost) {
            _rigidbody.AddForce(new Vector2(0f, -1f), ForceMode2D.Impulse);
            gravityBoost = false;
        }

        if (isJumping && isGrounded) {
            _rigidbody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            isJumping = false;
            gravityBoost = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Chest")) {
            _animator.SetTrigger("LevelComplete");
            StartCoroutine(gameManager.LevelComplete());
            StartCoroutine(gameManager.NextLevel());
        }
    }
}