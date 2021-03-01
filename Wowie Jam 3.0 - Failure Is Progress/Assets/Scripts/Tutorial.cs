using UnityEngine;
using TMPro;

public class Tutorial : MonoBehaviour {
    private GameManager gameManager;

    [SerializeField] private TextMeshProUGUI firstText;
    [SerializeField] private TextMeshProUGUI secondText;

    private void Start() {
        gameManager = GameManager.SharedInstance;
    }

    private void Update() {
        if (gameManager.numberOfLives == 0) {
            firstText.gameObject.SetActive(false);
            secondText.gameObject.SetActive(true);
        }
    }
}