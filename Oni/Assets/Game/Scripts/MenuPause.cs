using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuPause : MonoBehaviour
{
    [Header("Paneles")]
    public GameObject menuPause;
    public GameObject settingsPanel;

    [Header("Audio")]
    public AudioSource musicaAudioSource;
    public TextMeshProUGUI btnMusicaTexto;

    public bool isPaused = false;
    private bool musicaActiva = true;

    private void Start()
    {
        menuPause.SetActive(false);
        settingsPanel.SetActive(false);
        musicaActiva = PlayerPrefs.GetInt("Musica", 1) == 1;
        AplicarMusica();
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused)
                Continue();
            else
                Pause();
        }
    }

    public void Continue()
    {
        menuPause.SetActive(false);
        settingsPanel.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
    }

    public void Pause()
    {
        menuPause.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
    }

    public void Restart()
    {
        Time.timeScale = 1;
        // Guarda el nivel actual para que LoadingScreen sepa a dónde ir
        PlayerPrefs.SetString("EscenaDestino", SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("LoadingScreen");
    }

    public void OpenSettings()
    {
        menuPause.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void BackFromSettings()
    {
        settingsPanel.SetActive(false);
        menuPause.SetActive(true);
    }

    public void BackToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
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
            btnMusicaTexto.text = musicaActiva ? "ON" : "OFF";
    }
}