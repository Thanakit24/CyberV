using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Portal : MonoBehaviour
{
    public int portalCounter = 8;
    public TMP_Text teleportText;
    public void DecrementCounter()
    {
        portalCounter--;
        teleportText.text = portalCounter.ToString();
        Debug.Log("portal count down");

        if (portalCounter <= 0)
        {
            
        }
    }

    
}