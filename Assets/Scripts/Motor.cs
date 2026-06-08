using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Motor : MonoBehaviour
{
    public Snake snake;
    public GameObject enemys;
    private int numEnemys;
    private int nivel = 1;
    private int score = 0;
    private bool gameOver = false;
    private Transform pos;
    public Text textLevel;
    public Text textLevelUp;
    public Text textScore;
    public int maxNivel = 10;
    public int scorePerEnemy = 10;
    //body prefab
   // public GameObject Body;
    

    void Awake()
    {
        snake = GameObject.Find("Snake").GetComponent<Snake>();
        pos = GameObject.Find("Posicion").transform;
    }

    IEnumerator MakeEnemys()
    {
        numEnemys = nivel * 2;
        for (int i = 0; i < numEnemys; i++)
        {
            if (gameOver)
            {
                yield break;
            }

            Instantiate(enemys, pos.GetChild(Random.Range(0,pos.childCount)).position, Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
        }
         
    }

    void Start()
    {
        CreateScoreText();
        UpdateScoreText();
        if (textLevel != null)
        {
            textLevel.color = Color.white;
        }
       // StartCoroutine(Move());
        StartCoroutine(MakeEnemys());
        StartCoroutine(SearchEnemy());
    }

    IEnumerator SearchEnemy()
    {
        while (!gameOver)
        {
            while (GameObject.FindWithTag("enemy"))
            {
                if (gameOver)
                {
                    yield break;
                }
                yield return new WaitForSeconds(0.5f);
            }

            if (nivel >= maxNivel)
            {
                yield return StartCoroutine(EndGame("You Win"));
            }
            else
            {
                nivel++;
                snake.LevelUp();
                textLevel.text = "Level " + nivel;
                StartCoroutine(ShowLevelUp());
                numEnemys = nivel * 2;
                StartCoroutine(MakeEnemys());
            }
            yield return null;
        }
    }

    IEnumerator ShowLevelUp()
    {
        textLevelUp.enabled = true;
        yield return new WaitForSeconds(1);
        textLevelUp.enabled = false;
    }

    public void AddScore()
    {
        if (gameOver)
        {
            return;
        }

        score += scorePerEnemy;
        UpdateScoreText();
    }

    public void GameOver()
    {
        if (gameOver)
        {
            return;
        }

        StartCoroutine(EndGame("Game Over"));
    }

    IEnumerator EndGame(string title)
    {
        gameOver = true;
        SaveBestScore();

        if (textLevelUp != null)
        {
            RectTransform rectTransform = textLevelUp.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.sizeDelta = new Vector2(360f, 160f);
            }

            textLevelUp.fontSize = 32;
            textLevelUp.text = title + "\nScore: " + score + "\nBest: " + PlayerPrefs.GetInt("BestScore", 0);
            textLevelUp.enabled = true;
        }

        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(0);
    }

    private void SaveBestScore()
    {
        int bestScore = PlayerPrefs.GetInt("BestScore", 0);
        if (score > bestScore)
        {
            PlayerPrefs.SetInt("BestScore", score);
            PlayerPrefs.Save();
        }
    }

    private void CreateScoreText()
    {
        if (textScore != null)
        {
            return;
        }

        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            return;
        }

        GameObject scoreObject = new GameObject("TextScore");
        scoreObject.transform.SetParent(canvas.transform, false);

        RectTransform rectTransform = scoreObject.AddComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0f, 1f);
        rectTransform.anchorMax = new Vector2(0f, 1f);
        rectTransform.pivot = new Vector2(0f, 1f);
        rectTransform.anchoredPosition = new Vector2(18f, -12f);
        rectTransform.sizeDelta = new Vector2(220f, 40f);

        textScore = scoreObject.AddComponent<Text>();
        textScore.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        textScore.fontSize = 24;
        textScore.color = Color.white;
        textScore.alignment = TextAnchor.MiddleLeft;
    }

    private void UpdateScoreText()
    {
        if (textScore != null)
        {
            textScore.text = "Score: " + score;
        }
    }
}
