using UnityEngine;

public class PlayerDeath : MonoBehaviour {

    private GameManager gameManager;
    private AudioManager audioManager;

    private Collider2D _collider;

    private void Awake() {
        _collider = GetComponent<BoxCollider2D>();
    }

    private void Start() {
        gameManager = GameManager.SharedInstance;
        audioManager = AudioManager.SharedInstance;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.K)) DoADeath();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Lethal")) DoADeath();
        if (other.gameObject.CompareTag("Reset")) gameManager.ManualReset();
    }

    private void DoADeath() {
        audioManager.Play("DeathSound" + Random.Range(1, 4));
        Vector3 deathPosition = _collider.bounds.center;
        gameManager.SpawnNewPlayer(deathPosition);
        Destroy(gameObject);
    }
}