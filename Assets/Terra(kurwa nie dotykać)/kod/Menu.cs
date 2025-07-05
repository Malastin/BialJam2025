using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour{
    public void StartGame(){
        GameManager.NextMap();
    }
    public void Exit(){
        Debug.Log("Nie pamiętam jaka klasa obsługiwała wychodzenie z gry");
        Debug.Log("A jednak pamiętam xd");
        Application.Quit();
    }
}
