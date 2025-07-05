using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour{
    public void StartGame(string sceneName){
        SceneManager.LoadScene(sceneName);
    }
    public void Exit(){
        Debug.Log("Nie pamiętam jaka klasa obsługiwała wychodzenie z gry");
        Debug.Log("A jednak pamiętam xd");
        Application.Quit();
    }
}
