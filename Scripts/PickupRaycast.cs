using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PickupRaycast : MonoBehaviour
{
    public float pickupRange = 2f;
    public TMP_Text promptText;

    void Update()
    {
        GameObject[] treasures = GameObject.FindGameObjectsWithTag("Treasure");

        bool nearTreasure = false;

        foreach (GameObject treasure in treasures)
        {
            float distance = Vector3.Distance(transform.position, treasure.transform.position);

            if (distance <= pickupRange)
            {
                nearTreasure = true;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    FindObjectOfType<GameManager>().CollectItem();
                    Destroy(treasure);
                    return;
                }
            }
        }

        // Show / hide prompt
        if (promptText != null)
        {
            promptText.gameObject.SetActive(nearTreasure);
        }
    }
}