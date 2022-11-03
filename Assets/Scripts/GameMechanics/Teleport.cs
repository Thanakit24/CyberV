using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Teleport : MonoBehaviour
{
    public Transform prefab;
    private Transform portalInst;

    private Transform player;

    [HideInInspector]
    public bool portalActivate = false;
    public int portalInventory;
    public int portalInventoryUI;
    public TMP_Text portalInventoryText;

    private PlayerController playerController;
    public int maxPortalusage = 8;
    private int portalCounter;
    private TMP_Text teleportText;

    // Start is called before the first frame update
    void Start()
    {
        portalCounter = maxPortalusage;
        player = GetComponent<Transform>();
        playerController = GetComponent<PlayerController>();
    }

     void Update()
    { 
        if (portalInventory == 0)
        {
            return;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && !portalActivate && !playerController.isMoving)
        {
            PlacePortal();
        }

        else if (Input.GetKeyDown(KeyCode.Space) && portalActivate && !playerController.isMoving)
        {
            AudioManager.instance.Play("PlayerTeleports");
            player.position = portalInst.position;
            ClosePortal();
        }

        portalInventoryText.text = $"PORTAL: {portalInventoryUI}"; //A bit messy but it should work

        //if (Input.GetKeyDown(KeyCode.Space) && !portalActivate)
        //{
        //    PlacePortal();
        //}
        //else if(Input.GetKeyDown(KeyCode.Space) && portalActivate)
        //{
        //    AudioManager.instance.Play("PlayerTeleports");
        //    player.position = portalInst.position;
        //    ClosePortal();
        //}

    }
    private void PlacePortal()
    {
        Debug.Log($"PControllerMoving{playerController.isMoving}");
        if (!playerController.isMoving)
        {
            AudioManager.instance.Play("PlayerPlaceTP");
            portalInventoryUI--;
            portalInst = Instantiate(prefab, player.position, Quaternion.identity);
            teleportText = portalInst.GetComponentInChildren<TMP_Text>();
            playerController.OnMove += DecrementCounter;
            portalActivate = true;
            portalCounter = maxPortalusage;
        }
    }

    public void ClosePortal()
    {
        playerController.OnMove -= DecrementCounter;
        portalInventory--;
        Destroy(portalInst.gameObject);
        
        portalActivate = false;
    }

    public void DecrementCounter()
    {
        portalCounter--;
        teleportText.text = portalCounter.ToString();
        Debug.Log("portal count down");

        if (portalCounter <= 0)
        {
            ClosePortal();
        }

    }

}


