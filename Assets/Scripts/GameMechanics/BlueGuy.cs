using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueGuy : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite newSprite;
    private BoxCollider2D boxCollider;
    private PlayerController playerController;
    public bool goalsCapturedCheck = false;
    public GameObject completeLevelUI;
    private ParticleSystem particle;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        particle = GetComponent<ParticleSystem>();
        particle.Stop();
    }

    // Update is called once per frame
    public void ChangeSprite()
    {
        particle.Play();
        spriteRenderer.sprite = newSprite; //Change sprites when function is called
        goalsCapturedCheck = true;
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Hits with Blue");
            if (goalsCapturedCheck == true)
            {
                Debug.Log("Execute");
                completeLevelUI.SetActive(true);

            }
            else
            {
                Debug.Log("Execute Game Over");

                FindObjectOfType<LevelManager>().GameOver();
            }
        }
    }
}
