using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public int collected = 0;
    public int totalItems = 5;

    public float timeLeft = 120f;

    public TMP_Text collectText;
    public TMP_Text timerText;

    public GameObject startPanel;
    public GameObject winPanel;
    public GameObject losePanel;

    public GameObject[] treasureLocations;

    private bool gameRunning = false;

    void Start()
    {
        Time.timeScale = 0f;
        collected = 0;

        if (collectText != null)
        {
            collectText.text = "Treasures: 0/" + totalItems;
            collectText.gameObject.SetActive(false);
        }

        if (timerText != null)
        {
            timerText.text = "Time: " + Mathf.Ceil(timeLeft);
            timerText.gameObject.SetActive(false);
        }

        if (startPanel != null) startPanel.SetActive(true);
        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);


        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        if (!gameRunning) return;

        timeLeft -= Time.deltaTime;

        if (timerText != null)
        {
            timerText.text = "Time: " + Mathf.Ceil(timeLeft);
        }

        if (timeLeft <= 0)
        {
            LoseGame();
        }
    }

public void StartGame()
{
    Time.timeScale = 1f;

    collected = 0;
    timeLeft = 120f;
    gameRunning = true;

    collectText.text = "Treasures: 0/" + totalItems;
    timerText.text = "Time: " + Mathf.Ceil(timeLeft);

    startPanel.SetActive(false);
    winPanel.SetActive(false);
    if (losePanel != null) losePanel.SetActive(false);

    collectText.gameObject.SetActive(true);
    timerText.gameObject.SetActive(true);

    // 🔥 move it here instead
    RandomiseTreasures();

    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;
}

    public void CollectItem()
    {
        if (!gameRunning) return;

        collected++;
        collectText.text = "Treasures: " + collected + "/" + totalItems;

        if (collected >= totalItems)
        {
            WinGame();
        }
    }

    void WinGame()
    {
        gameRunning = false;
        Time.timeScale = 0f;

        winPanel.SetActive(true);
        collectText.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void LoseGame()
    {
        gameRunning = false;
        Time.timeScale = 0f;

        if (losePanel != null)
            losePanel.SetActive(true);

        collectText.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void HideAllTreasures()
    {
        foreach (GameObject treasure in treasureLocations)
        {
            if (treasure != null)
                treasure.SetActive(false);
        }
    }

    void RandomiseTreasures()
    {
        HideAllTreasures();

        List<GameObject> list = new List<GameObject>(treasureLocations);

        for (int i = 0; i < totalItems; i++)
        {
            if (list.Count == 0) return;

            int randomIndex = Random.Range(0, list.Count);
            list[randomIndex].SetActive(true);
            list.RemoveAt(randomIndex);
        }
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

