using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Snake : MonoBehaviour
{
    
    private Rigidbody2D rig;
    private int life = 100;
    public Slider vida;
    public Text textLose;
    public float velocidad;
    public GameObject bodyPrefab;
    public List<Rigidbody2D> body = new List<Rigidbody2D>();
    //venom prefab
    public GameObject Venom;
    private bool left = false;
    private bool right = false;
    private float dir;
    public Botones derecha;
    public Botones izquierda;
    public Botones shoot;
    private float ultimoDisparo;
    private bool band = false;
    
   
    void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rig.velocity = Vector2.down * velocidad;
        body.Add(rig);
        StartCoroutine("Disparo");
   
    }

    // Update is called once per frame
    void Update()
    {    
        Move();
        
    }
    public void Shoot()
    {
        Instantiate(Venom, rig.position, Quaternion.Euler(Vector3.forward * rig.rotation));
    }
    
     IEnumerator Disparo()
    {
        while(true)
        {
            if (shoot.presionado)
            {
                Instantiate(Venom, rig.position, Quaternion.Euler(Vector3.forward * rig.rotation));
            }
            
            yield return new WaitForSeconds(0.7f);
        }
    }

    void Move()
    {
        if (derecha.presionado)
        {
            dir = 1;
        }

        else if (izquierda.presionado)
        {
            dir = -1;
        }

        else{
            dir = 0;
        }
        
        rig.rotation += dir * (velocidad+2.5f);
        rig.velocity = transform.up * velocidad;

        for (int i = 1; i < body.Count; i++)
         {
            body[i].rotation = rig.rotation;
            //body[i].velocity = transform.up * velocidad;
            Vector2 dire = body[i - 1].transform.up.normalized;
            body[i].position = body[i-1].position + (-dire * new Vector2(-0.03f, 0.55f));

        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Borders")
        {
            life -= 5;
            rig.position = Vector2.zero;
            if(body.Count > 0)
            {
                for (int i = 0; i < body.Count; i++)
                {
                    body[i].position = Vector2.zero;
                }
            }       
        }
        else if (coll.tag == "enemy")
        {
            life -= 5;
        }
        if (life <= 0)
        {
      
            SceneManager.LoadScene(0);
        }

        vida.value = life;

    }

    /*public void levelUp()
    {
        Rigidbody2D obj = Instantiate(bodyPrefab, rig.position - (transform.up.normalized * new Vector2(-0.03f, 0.55f)), Quaternion.identity).GetComponent<Rigidbody2D>();
        body.Add(obj);
    }*/
}
