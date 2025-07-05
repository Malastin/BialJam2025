using UnityEngine;

public class PlayerAddReferenceToManager : MonoBehaviour{
    public byte id = 0;
    void Awake(){
        GameManager.AddPlayer(gameObject, id);
    }
}
