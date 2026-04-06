using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject gameOverPanel;
    public Button reiniciarButton;
    public Button menuButton;
    private bool gameOverActivo = false;

    public int currentHearts = 3;
    public int maxHearts = 3;
    public int totalRedCrystals = 0;
    public int totalGreenCrystals = 0;
    public int totalPurpleCrystals = 0;
    public int totalHerbs = 0;

    private int startRed;
    private int startGreen;
    private int startPurple;
    private int startHerb;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        gameOverActivo = false;
        gameOverPanel = null;
        StartCoroutine(BuscarPanelGameOver());
    }

    IEnumerator BuscarPanelGameOver()
    {
        yield return null; // espera un frame

        // Busca en todos los Transform de la escena
        foreach (GameObject obj in Resources.FindObjectsOfTypeAll<GameObject>())
        {
            if (obj.name == "PanelGameOver" && obj.scene.isLoaded)
            {
                gameOverPanel = obj;
                gameOverPanel.SetActive(false);

                // Reasigna botones
                Button[] botones = gameOverPanel.GetComponentsInChildren<Button>(true);
                foreach (Button btn in botones)
                {
                    btn.onClick.RemoveAllListeners();
                    if (btn.name == "BtnReiniciar")
                    {
                        btn.onClick.AddListener(ReiniciarEscena);
                        reiniciarButton = btn;
                    }
                    if (btn.name == "BtnSalir")
                    {
                        btn.onClick.AddListener(IrAlMenu);
                        menuButton = btn;
                    }
                }
                break;
            }
        }
    }

    void Start()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
        if (reiniciarButton != null)
            reiniciarButton.onClick.AddListener(ReiniciarEscena);
        if (menuButton != null)
            menuButton.onClick.AddListener(IrAlMenu);
    }

    public void GameOver()
    {
        if (gameOverActivo) return;
        gameOverActivo = true;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
        else
            Debug.LogError("PanelGameOver no encontrado!");
    }

    public void ReiniciarEscena()
    {
        //Time.timeScale = 1f;
        //gameOverActivo = false;
        //gameOverPanel = null;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //SceneManager.LoadScene("LoadingScreen");
        Time.timeScale = 1;
        // Guarda el nivel actual para que LoadingScreen sepa a dónde ir
        PlayerPrefs.SetString("EscenaDestino", SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("LoadingScreen");
    }

    public void IrAlMenu()
    {
        Time.timeScale = 1f;
        PlayerPrefs.SetString("EscenaDestino", "Menu");
        SceneManager.LoadScene("LoadingScreen");
    }

    public void SaveLevelStartSnapshot()
    {
        startRed = totalRedCrystals;
        startGreen = totalGreenCrystals;
        startPurple = totalPurpleCrystals;
        startHerb = totalHerbs;
    }

    public void DiscardLevelProgress()
    {
        totalRedCrystals = startRed;
        totalGreenCrystals = startGreen;
        totalPurpleCrystals = startPurple;
        totalHerbs = startHerb;
    }

    public void AddRedCrystal() { totalRedCrystals++; }
    public void AddGreenCrystal() { totalGreenCrystals++; }
    public void AddPurpleCrystal() { totalPurpleCrystals++; }
    public void AddHerb() { totalHerbs++; }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}