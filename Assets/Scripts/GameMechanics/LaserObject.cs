using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserObject : MonoBehaviour
{
    public LayerMask player;
    private PlayerController playerController;
    private float rotSpeed;
    private Quaternion targetRotation;
    
    // Start is called before the first frame update
    void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
    }
    //I want to die x2
    private void OnEnable()
    {
        playerController.OnMove += Rotate;
    }

    private void OnDisable()
    {
        playerController.OnMove -= Rotate;
    }

    // Update is called once per frame
    void Rotate()
    {  
        transform.rotation = transform.rotation * Quaternion.Euler(0, 0, -90);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 1000f, player);

        if (hit.collider != null)
        {
            Debug.Log("Hits");
            Debug.DrawLine(transform.position, hit.point, Color.red, 1f);

            if(hit.collider.tag == "InvisHitBox")
            {
                Debug.Log("Hits Player");
                playerController.LaserDamage();
            }
        }

       
          
    }

    
}
