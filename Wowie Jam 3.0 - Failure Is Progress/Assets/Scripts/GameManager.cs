using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour {
    public static GameManager SharedInstance;

    [Header("Game Objects")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerCorpse;
    [SerializeField] private Transform startPosition;
    [SerializeField] private TextMeshProUGUI tipReset;
    [SerializeField] private Texture deadKingUI;
    [SerializeField] private TextMeshProUGUI completeText;
    [SerializeField] private TextMeshProUGUI failedText;

    [Header("Level-Specific Attributes")]
    [SerializeField] public int numberOfLives;

    private void Awake() {
        SharedInstance = this;

        Instantiate(player, startPosition.position, Quaternion.identity);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.R)) ResetLevel();
    }

    public void SpawnNewPlayer(Vector3 deadPlayerPosition) {
        numberOfLives -= 1;
        if (numberOfLives >= 0) {

            //Adjust the lives sprite
            GameObject lifeSprite = GameObject.Find("Life" + (numberOfLives + 1));
            lifeSprite.GetComponent<RawImage>().texture = deadKingUI;
            Color currentColour = lifeSprite.GetComponent<RawImage>().color;
            lifeSprite.GetComponent<RawImage>().color = new Color(currentColour.r, currentColour.g, currentColour.b, 0.5f);

            //Bring in a new platform/corpse
            Instantiate(playerCorpse, deadPlayerPosition, Quaternion.identity);

            //Spawn a new actual player
            Instantiate(player, startPosition.position, Quaternion.identity);
        }
        else if (numberOfLives == 0) {
            StartCoroutine(TT_Reset());
        }
        else if (numberOfLives < 0) {
            StartCoroutine(LevelFailed());
            StartCoroutine(RestartAfterFail());
        }
    }

    public void ManualReset() {
        StartCoroutine(LevelFailed());
        StartCoroutine(RestartAfterFail());
    }

    public IEnumerator LevelComplete() {
        Color tmpColour = completeText.color;
        while (tmpColour.a < 1f) {
            completeText.color = new Color(completeText.color.r, completeText.color.g, completeText.color.b, completeText.color.a + Time.deltaTime);
            yield return null;
        }
    }

    public IEnumerator LevelFailed() {
        Color tmpColour = failedText.color;
        while (tmpColour.a < 1f) {
            failedText.color = new Color(failedText.color.r, failedText.color.g, failedText.color.b, failedText.color.a + Time.deltaTime);
            yield return null;
        }
    }

    public IEnumerator RestartAfterFail() {
        yield return new WaitForSeconds(2f);
        ResetLevel();
    }

    public IEnumerator NextLevel() {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void ResetLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator TT_Reset() {
        yield return new WaitForSeconds(10f);
        Color tmpColour = tipReset.color;
        while (tmpColour.a < 1f) {
            tipReset.color = new Color(tipReset.color.r, tipReset.color.g, tipReset.color.b, tipReset.color.a + Time.deltaTime);
            yield return null;
        }
    }
}