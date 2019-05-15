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
    private Transform pos;
    public Text textLevel;
    public Text textLevelUp;
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
            Instantiate(enemys, pos.GetChild(Random.Range(0,pos.childCount)).position, Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
        }
         
    }

    void Start()
    {
       // StartCoroutine(Move());
        StartCoroutine(MakeEnemys());
        StartCoroutine(SearchEnemy());
    }

    IEnumerator SearchEnemy()
    {
        while (true)
        {
            while (GameObject.FindWithTag("enemy"))
            {
                yield return new WaitForSeconds(0.5f);
            }

            if (nivel == 5)
            {
                textLevelUp.text = "You Win";
                textLevelUp.enabled = true;
                yield return new WaitForSeconds(2);
                textLevelUp.enabled = false;
                SceneManager.LoadScene(0);
                
            }
            else
            {
                nivel++;
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
}
