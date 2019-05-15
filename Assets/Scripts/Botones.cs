using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Botones : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [HideInInspector]
    public bool presionado = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        presionado = true;
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }
    
    public void Exit()
    {
        Application.Quit();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        presionado = false;
    }
}
