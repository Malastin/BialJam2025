using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MusicSlider : MonoBehaviour
{
    public AudioMixer mixer;
    [SerializeField] public Slider music_volume;
    [SerializeField] private Slider sfx_volume;
    public void Start(){
        float musicVol, SFXVol;
        mixer.GetFloat("MusicVolume", out musicVol);
        mixer.GetFloat("EffectsVolume", out SFXVol);
        music_volume.value = TransferdBToNumber(musicVol);
        sfx_volume.value = TransferdBToNumber(SFXVol);
    }
    public void SoundtrackVolume()
    {
        if (music_volume.value == 0)
        {
            mixer.SetFloat("MusicVolume", -100);
        }
        else
        {
            mixer.SetFloat("MusicVolume", TransferNumberTodB(music_volume.value));
        }
    }
    public void GeneralVolume(){
        if(sfx_volume.value == 0){
            mixer.SetFloat("EffectsVolume", -100);
        }else{
            mixer.SetFloat("EffectsVolume", TransferNumberTodB(sfx_volume.value));
        }
    }
    public float TransferNumberTodB(float num)
    {
        return (num * (20 + 30)) - 30;
    }
    public float TransferdBToNumber(float num){
        return (num + 30) / (20 + 30);
    }
}
