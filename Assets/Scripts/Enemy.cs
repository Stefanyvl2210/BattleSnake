using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Vector2 velocidadActual;
    private Rigidbody2D rig;
    public float velocidad;
    private Transform culebrita;
    private Vector3 punto;

    void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
        FindSnake();
    }

    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (culebrita == null)
        {
            FindSnake();
        }

        if (culebrita != null)
        {
            Vector2 nextPosition = Vector2.MoveTowards(rig.position, culebrita.position, velocidad * Time.fixedDeltaTime);
            rig.MovePosition(nextPosition);
        }
        //rig.position = Vector2.SmoothDamp(rig.position, culebrita.position, ref velocidadActual, velocidad);
    }

    private void FindSnake()
    {
        GameObject snake = GameObject.Find("Snake");
        if (snake != null)
        {
            culebrita = snake.transform;
        }
    }
}
