using UnityEngine;
using UnityEngine.SceneManagement;

public class EOGMenu : MonoBehaviour {
    public void BackToTitle() {
        SceneManager.LoadScene("Main Menu");
    }
}