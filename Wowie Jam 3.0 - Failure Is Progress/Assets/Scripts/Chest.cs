using UnityEngine;

public class Chest : MonoBehaviour {
    
    private Animator _animator;
    private AudioManager audioManager;

    private void Awake() {
        _animator = GetComponent<Animator>();
    }

    private void Start() {
        audioManager = AudioManager.SharedInstance;
    }
    
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player")) {
            _animator.SetTrigger("Opened");
            audioManager.Play("ChestSound");
        }
    }
}
