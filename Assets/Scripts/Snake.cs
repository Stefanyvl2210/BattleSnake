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
    public float turnSpeed = 130f;
    public GameObject bodyPrefab;
    public int initialBodyParts = 2;
    public float bodySpacing = 0.38f;
    public float shootCooldown = 0.35f;
    public float positionRecordDistance = 0.04f;
    public List<Rigidbody2D> body = new List<Rigidbody2D>();
    //venom prefab
    public GameObject Venom;
    private float dir;
    public Botones derecha;
    public Botones izquierda;
    public Botones shoot;
    private bool shootWasPressed = false;
    private float nextShootTime = 0f;
    private bool isDead = false;
    private Motor motor;
    private readonly List<Vector2> positionHistory = new List<Vector2>();
    
   
    void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
        motor = FindObjectOfType<Motor>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rig.velocity = Vector2.down * velocidad;
        body.Clear();
        body.Add(rig);
        AddBodyParts(initialBodyParts);
        SeedPositionHistory();
   
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            return;
        }

        Move();
        HandleShoot();
        
    }
    public void Shoot()
    {
        Instantiate(Venom, rig.position, Quaternion.Euler(Vector3.forward * rig.rotation));
    }
    void Move()
    {
        dir = GetTurnDirection();

        rig.rotation += dir * turnSpeed * Time.deltaTime;
        rig.velocity = transform.up * velocidad;

        RecordHeadPosition();
        UpdateBodyPositions();
    }

    private float GetTurnDirection()
    {
        if (derecha != null && derecha.usarComoJoystick)
        {
            return derecha.horizontal;
        }

        if (izquierda != null && izquierda.usarComoJoystick)
        {
            return izquierda.horizontal;
        }

        if (derecha != null && derecha.presionado)
        {
            return 1f;
        }

        if (izquierda != null && izquierda.presionado)
        {
            return -1f;
        }

        return 0f;
    }

    private void UpdateBodyPositions()
    {
        for (int i = 1; i < body.Count; i++)
        {
            Vector2 nextPosition = GetHistoryPoint(i * bodySpacing);
            Vector2 forward = body[i - 1].position - nextPosition;

            body[i].position = nextPosition;
            if (forward.sqrMagnitude > 0.001f)
            {
                body[i].rotation = Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg - 90f;
            }
        }
    }

    private void HandleShoot()
    {
        bool isPressed = shoot != null && shoot.presionado;

        if (isPressed && !shootWasPressed && Time.time >= nextShootTime)
        {
            Shoot();
            nextShootTime = Time.time + shootCooldown;
        }

        shootWasPressed = isPressed;
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (isDead)
        {
            return;
        }

        if (coll.tag == "Borders")
        {
            life -= 5;
            ResetSnakeToCenter();
        }
        else if (coll.tag == "enemy")
        {
            life -= 5;
        }
        if (life <= 0)
        {
            isDead = true;
            rig.velocity = Vector2.zero;
            if (motor != null)
            {
                motor.GameOver();
            }
            else
            {
                SceneManager.LoadScene(0);
            }
        }

        vida.value = life;

    }

    public void LevelUp()
    {
        AddBodyPart();
    }

    public void levelUp()
    {
        LevelUp();
    }

    private void AddBodyParts(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            AddBodyPart();
        }
    }

    private void AddBodyPart()
    {
        if (bodyPrefab == null || body.Count == 0)
        {
            return;
        }

        Rigidbody2D previous = body[body.Count - 1];
        Vector2 dire = previous.transform.up.normalized;
        Vector2 spawnPosition = previous.position - (dire * bodySpacing);
        Rigidbody2D obj = Instantiate(bodyPrefab, spawnPosition, previous.transform.rotation).GetComponent<Rigidbody2D>();

        if (obj == null)
        {
            return;
        }

        obj.rotation = previous.rotation;
        body.Add(obj);
    }

    private void RecordHeadPosition()
    {
        if (positionHistory.Count == 0)
        {
            positionHistory.Add(rig.position);
            return;
        }

        if (Vector2.Distance(positionHistory[0], rig.position) >= positionRecordDistance)
        {
            positionHistory.Insert(0, rig.position);
        }

        int maxPoints = Mathf.Max(40, body.Count * 30);
        while (positionHistory.Count > maxPoints)
        {
            positionHistory.RemoveAt(positionHistory.Count - 1);
        }
    }

    private Vector2 GetHistoryPoint(float distance)
    {
        if (positionHistory.Count == 0)
        {
            return rig.position;
        }

        float coveredDistance = 0f;
        for (int i = 1; i < positionHistory.Count; i++)
        {
            float segmentDistance = Vector2.Distance(positionHistory[i - 1], positionHistory[i]);
            if (segmentDistance <= 0.0001f)
            {
                continue;
            }

            if (coveredDistance + segmentDistance >= distance)
            {
                float amount = (distance - coveredDistance) / segmentDistance;
                return Vector2.Lerp(positionHistory[i - 1], positionHistory[i], amount);
            }

            coveredDistance += segmentDistance;
        }

        return positionHistory[positionHistory.Count - 1];
    }

    private void SeedPositionHistory()
    {
        positionHistory.Clear();

        Vector2 backDirection = -transform.up.normalized;
        int points = Mathf.Max(40, body.Count * 30);
        for (int i = 0; i < points; i++)
        {
            positionHistory.Add(rig.position + (backDirection * positionRecordDistance * i));
        }
    }

    private void ResetSnakeToCenter()
    {
        rig.position = Vector2.zero;
        rig.velocity = transform.up * velocidad;
        SeedPositionHistory();

        for (int i = 0; i < body.Count; i++)
        {
            body[i].position = GetHistoryPoint(i * bodySpacing);
            body[i].rotation = rig.rotation;
        }
    }
}
