using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public bool isMoving;
    private SpriteRenderer sprite;

    private Vector3 origPos, targetPos;
    private float timeToMove = 0.2f;
    private Rigidbody2D rb;
    private BlueGuy blueGuy;
    private LevelManager levelManager;
    public float inputBuffer = 0.25f;

    public LayerMask notCollidable;
    public LayerMask blueGoalMask;

    public int limitedOfMoves;
    public int goalsLeft;
    public TMP_Text batteryText;

    public delegate void MovementAction();
    public event MovementAction OnMove;
    [HideInInspector]
    public bool isWinning;
    public bool cheatsOn = false;
    //public Transform prefab;
    //private float destroyPrefab = 1f;


    // Update is called once per frame

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        blueGuy = FindObjectOfType<BlueGuy>();
        levelManager = FindObjectOfType<LevelManager>();

    }
    void Update()
    {
        if (Input.GetKey(KeyCode.W) && !isMoving)
            StartCoroutine(MovePlayer(Vector3.up));

        if (Input.GetKey(KeyCode.A) && !isMoving)
            StartCoroutine(MovePlayer(Vector3.left));

        if (Input.GetKey(KeyCode.S) && !isMoving)
            StartCoroutine(MovePlayer(Vector3.down));

        if (Input.GetKey(KeyCode.D) && !isMoving)
            StartCoroutine(MovePlayer(Vector3.right));

        if (Input.GetKey(KeyCode.R))
        {
            levelManager.Restart();
        }

        if (Input.GetKeyUp(KeyCode.C))
        {
            cheatsOn = !cheatsOn;
        }
       
        batteryText.text = $"MOVES: {limitedOfMoves}";
        if (cheatsOn)
        {
            batteryText.text = $"MOVES: ∞";
        }
    }

    private IEnumerator MovePlayer(Vector3 direction)
    {
        origPos = transform.position;
        targetPos = origPos + direction;
        //Debug.Log("Instantiated");
        //Instantiate(prefab, targetPos, Quaternion.identity);
        //Debug.Log("Prefab destroyed");
        //DestroyImmediate(prefab, true);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1f, notCollidable);

        if (hit.collider != null)
        {
            PushObject pushObject = hit.collider.gameObject.GetComponent<PushObject>();

            if (pushObject != null)
            {
                bool pushing = pushObject.BeingPushed(direction);
                float inputBufferTime = 0;
                if (pushing)
                {

                    isMoving = true;
                    DecrementMoves(1);
                    while (inputBufferTime < timeToMove + inputBuffer)
                    {
                        inputBufferTime += Time.deltaTime;
                        yield return null;
                    }
                    isMoving = false;
                }

            }
            yield break;

        }

        RaycastHit2D hitGoal = Physics2D.Raycast(transform.position, direction, 1f, blueGoalMask);
        if (hitGoal.collider != null)
        {
            if (hitGoal.collider.tag == "BlueGuy" && goalsLeft == 0)
            {
                Debug.Log("Winning True");
                isWinning = true;
            }
        }
        AudioManager.instance.Play("PlayerMove");

        isMoving = true;

        DecrementMoves(1);

        float elapsedTime = 0;

        while (elapsedTime < timeToMove)
        {
            rb.MovePosition(Vector3.Lerp(origPos, targetPos, (elapsedTime / timeToMove)));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
        while (elapsedTime < timeToMove + inputBuffer)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        isMoving = false;
    }

    public void Death()
    {
        Debug.Log("Game Over");
        levelManager.GameOver();
    }
    public void DecrementMoves(int moveAmount)
    {
        limitedOfMoves -= moveAmount;
        if (OnMove != null)
        {
            OnMove();
        }
        if (limitedOfMoves == 0 && !isWinning && !cheatsOn)
        {
            Death();
            Debug.Log("Death function called");
        }

    }
    public void LaserDamage()
    {
        limitedOfMoves -= 3;

        if (limitedOfMoves == 0 && !isWinning)
        {
            Death();
            Debug.Log("Death function called");
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Goal")
        {
            AudioManager.instance.Play("PlayerCollidesGoal");
            goalsLeft--;
            BlueGoal();
            Destroy(collision.gameObject);
            collision.GetComponent<BoxCollider2D>().enabled = false;
        }

        //    if (collision.gameObject.tag == "BlueGuy")
        //    {
        //        Debug.Log("Collision Hits with BlueGuy");
        //        if (goalsLeft <= 0)
        //        {   
        //            FindObjectOfType<LevelManager>().LoadNextLevel();
        //        }
        //        else
        //        {
        //            Death();
        //        }

        //    }

        //}
    }
    public void BlueGoal()
    {
        if (goalsLeft <= 0)
        {
            blueGuy.ChangeSprite();
        }
    }

}



