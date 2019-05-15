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
        culebrita = GameObject.Find("Snake").transform;
    }

    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        rig.position = Vector2.MoveTowards(rig.position, culebrita.position, velocidad * Time.deltaTime);
        //rig.position = Vector2.SmoothDamp(rig.position, culebrita.position, ref velocidadActual, velocidad);
    }
}
