using UnityEngine;

public class PlsPersist : MonoBehaviour {
    private void Awake() {
        DontDestroyOnLoad(gameObject);
    }
}