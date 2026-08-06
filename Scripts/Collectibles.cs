using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectibles : MonoBehaviour
{
    public float collectDistance = 10f;

    private Transform player;
    private GameManager gameManager;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (player == null || gameManager == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance < collectDistance)
        {
        Debug.Log("Picked up treasure");
        gameManager.CollectItem();
         Destroy(gameObject);
        }       
    }
}