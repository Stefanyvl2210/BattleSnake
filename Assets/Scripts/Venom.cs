using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Venom : MonoBehaviour
{
    private Rigidbody2D rig;
    public float velocidad;
   

    void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rig.velocity = transform.up * velocidad;
        Destroy(gameObject, 2);//con g minuscula hace referencia al objeto del script
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.tag == "enemy")
        {
            Destroy(coll.gameObject);
        }
    }
}
