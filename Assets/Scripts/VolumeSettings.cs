using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer musicMixer;
    [SerializeField] private Slider musicSlider;

    private void Start()
    {
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMusicVolume();
        }

        musicSlider.onValueChanged.AddListener(delegate { SetMusicVolume(); });
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        musicMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);


        if (AudioManager.instance != null)
        {
            AudioManager.instance.SetVolume(volume);
        }
    }

    public void LoadVolume()
    {
        float savedVolume = PlayerPrefs.GetFloat("musicVolume");
        musicSlider.value = savedVolume;

        SetMusicVolume();
    }
}
