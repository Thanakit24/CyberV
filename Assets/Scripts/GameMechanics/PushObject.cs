using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushObject : MonoBehaviour
{
    public LayerMask notCollidable;
    private float timeToMove = 0.2f;
    public bool isMoving = false;
    public Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator MoveObject(Vector3 direction)
    {
        Vector3 origPos = transform.position;
        Vector3 targetPos = origPos + direction;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1000f, notCollidable);

        if (hit.collider != null)//if collider exists
        {
            AudioManager.instance.Play("PlayerCollidesWall");
            Debug.Log("Collider Exists Lmao");
            float distanceFromWall = Vector2.Distance(hit.point, origPos);
            float distanceCheck = distanceFromWall - direction.magnitude;

            if (distanceCheck < 0)//if the next move is going to hit a wall or block then don't move.
            {
                yield break;
            }
        }

        isMoving = true;

        float elapsedTime = 0;

        while (elapsedTime < timeToMove)
        {
            rb.MovePosition(Vector3.Lerp(origPos, targetPos, (elapsedTime / timeToMove)));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPos;

        isMoving = false;
    }
    public bool BeingPushed(Vector3 direction)
    {
        if (isMoving)
        {
            return false;
        }

        StartCoroutine(MoveObject(direction));
        Debug.Log($"pushed{direction}");

        return true;
    }
}

