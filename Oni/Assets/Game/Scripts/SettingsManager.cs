using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    [Header("Paneles")]
    public GameObject settingsPanel;
    public GameObject menuPanel;

    [Header("Música")]
    public AudioSource musicaAudioSource;
    public TextMeshProUGUI btnMusicaTexto;

    private bool musicaActiva = true;

    void Start()
    {
        settingsPanel.SetActive(false);

        // Recupera el estado guardado
        musicaActiva = PlayerPrefs.GetInt("Musica", 1) == 1;
        AplicarMusica();
    }

    public void OpenSettings()
    {
        menuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void BackToMenu()
    {
        settingsPanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    public void ToggleMusica()
    {
        musicaActiva = !musicaActiva;
        AplicarMusica();
        PlayerPrefs.SetInt("Musica", musicaActiva ? 1 : 0);
    }

    private void AplicarMusica()
    {
        if (musicaAudioSource != null)
            musicaAudioSource.mute = !musicaActiva;

        // Cambia el texto del botón
        if (btnMusicaTexto != null)
            btnMusicaTexto.text = musicaActiva ? "♪ ON" : "♪ OFF";
    }
    }