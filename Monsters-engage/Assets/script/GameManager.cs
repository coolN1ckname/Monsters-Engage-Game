using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public enum GameState
    {
        Gameplay,
        Paused,
        GameOver
    }

    public GameState currentState;
    public GameState previuosState;

    [Header("Урон в цифрах")]
    public Canvas damageTextCanvas;
    public float textFontSize = 32;
    public TMP_FontAsset textFont;
    public Camera referenceCamera;

    [Header("Экраны")]
    public GameObject pauseScreen;
    public GameObject resultsScreen;

    [Header("Отображение текущих статов")]
    //Отображение текущих статов
    public Text currentHealthDisplay;
    public Text currentRecoveryDisplay;
    public Text currentMoveSpeedDisplay;
    public Text currentMightDisplay;
    public Text currentProjectileSpeedDisplay;

    [Header("Результаты")]
    public Text levelReachedDisplay;
    public List<Image> chosenPassiveItemsUI = new List<Image>(25);
    public Text timeSurvivedDisplay;

    [Header("Таймер")]
    float stopwatchTime;
    public Text stopwatchDisplay;

    public bool isGameOver = false;
    void Awake() 
    {
        if(instance == null)
            instance = this;
        else
            Debug.LogWarning("EXTRA" + this + " DELETED");
        DisableScreens();
    }

    void Update() 
    {
        switch(currentState)
        {
            case GameState.Gameplay:
                CheckForPausedAndResume();
                UpdateStopwatch();
                break;

            case GameState.Paused:
                CheckForPausedAndResume();
                break;

            case GameState.GameOver:
                if(!isGameOver)
                {
                    isGameOver = true;
                    Time.timeScale = 0f;
                    Debug.Log("ИГРА ОКОНЧЕНА");
                    DisplayResults();
                }
                break;

            default:
                Debug.LogWarning("СТАДИИ НЕ СУЩЕСТВУЕТ");
                break;
        }
    }
    

    public void ChangeState(GameState newState)
    {
        currentState = newState;
    }

    public void PauseGame()
    {
        if(currentState != GameState.Paused)
        {
            previuosState = currentState;
            ChangeState(GameState.Paused);
            Time.timeScale = 0f;
            pauseScreen.SetActive(true);
            Debug.Log("ИГРА НА ПАУЗЕ");
        }
        
    }

    public void ResumeGame()
    {
        if(currentState == GameState.Paused)
        {
            ChangeState(previuosState);
            Time.timeScale= 1f;
            pauseScreen.SetActive(false);
            Debug.Log("ИГРА ВОЗОБНОВЛЕНА");
        }
    }

    void CheckForPausedAndResume()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(currentState==GameState.Paused)
                ResumeGame();
            else    
                PauseGame();
        }

    }

    void DisableScreens()
    {
        pauseScreen.SetActive(false);
        resultsScreen.SetActive(false);
    }

    public void GameOver()
    {
        timeSurvivedDisplay.text = stopwatchDisplay.text;
        ChangeState(GameState.GameOver);
    }

    void DisplayResults()
    {
        resultsScreen.SetActive(true);
    }

    public void AssignLevelReachedUI(int levelReachedData)
    {
        levelReachedDisplay.text = levelReachedData.ToString();
    }

    public void AssignChosenPassiveItemsUI(List<Image> chosenPassiveItemsData)
    {
        if(chosenPassiveItemsData.Count != chosenPassiveItemsUI.Count)
        {
            Debug.Log("Разные длинны");
            return;
        }

        for(int i = 0; i <chosenPassiveItemsUI.Count; i++)
        {
            if(chosenPassiveItemsData[i].sprite)
            {
                chosenPassiveItemsUI[i].enabled = true;
                chosenPassiveItemsUI[i].sprite = chosenPassiveItemsData[i].sprite;
            }
            else
                chosenPassiveItemsUI[i].enabled = false;
        }
    }

    void UpdateStopwatch()
    {
        stopwatchTime += Time.deltaTime;
        UpdateStopwatchDisplay();
    }

    void UpdateStopwatchDisplay()
    {
        int minutes = Mathf.FloorToInt(stopwatchTime / 60);
        int seconds = Mathf.FloorToInt(stopwatchTime % 60);

        stopwatchDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

}
