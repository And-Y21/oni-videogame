using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class CharacterSelectManager : MonoBehaviour
{
    [Header("Paneles")]
    public GameObject characterSelectPanel;
    public GameObject menuPanel;

    [Header("Botones de personaje")]
    public Button btnChar1;
    public Button btnChar2;

    [Header("Botones de acción")]
    public Button btnStart;
    public Button btnBack;

    [Header("Indicador de selección")]
    public Image selectorChar1;
    public Image selectorChar2;

    private int selectedCharacter = -1;

    void Start()
    {
        characterSelectPanel.SetActive(false);
        btnStart.interactable = false;
    }

    public void OpenCharacterSelect()
    {
        menuPanel.SetActive(false);
        characterSelectPanel.SetActive(true);
        selectedCharacter = -1;
        btnStart.interactable = false;
        if (selectorChar1) selectorChar1.gameObject.SetActive(false);
        if (selectorChar2) selectorChar2.gameObject.SetActive(false);
    }

    public void SelectCharacter(int index)
    {
        selectedCharacter = index;
        btnStart.interactable = true;
        if (selectorChar1) selectorChar1.gameObject.SetActive(index == 0);
        if (selectorChar2) selectorChar2.gameObject.SetActive(index == 1);
        Debug.Log("Personaje seleccionado: " + index);
    }

    public void StartGame()
    {
        if (selectedCharacter == -1) return;
        PlayerPrefs.SetInt("SelectedCharacter", selectedCharacter);
        PlayerPrefs.SetString("EscenaDestino", "LevelOne");
        SceneManager.LoadScene("LoadingScreen");
    }

    public void BackToMenu()
    {
        characterSelectPanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}