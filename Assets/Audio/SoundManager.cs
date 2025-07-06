using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour{
    [SerializeField] private AudioClip[] soundList;
    private static SoundManager instance;
    public AudioSource audioSource;
    private void Awake(){
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public static void StopSound(){
        instance.audioSource.Stop();
    }
    public static void PlaySound(SoundType sound, AudioSource source = null, float volume = 1){
        AudioClip clip = instance.soundList[(int)sound];
        Debug.Log(sound.ToString());
        if (source) {
            source.clip = clip;
            source.Play();
        }
        else
        {
            instance.audioSource.PlayOneShot(clip, volume);
        }
    }
    void Update(){
        if (Input.GetKeyDown(KeyCode.P)) {
            audioSource.Stop();
        }
    }
}
