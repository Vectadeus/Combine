using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowScript : MonoBehaviour
{
    public Transform PlayerTransform;
    public Vector3 offset;
    // Start is called before the first frame update


    private void OnEnable()
    {
        
        if(GameObject.FindGameObjectWithTag("Player") != null)
        {
            PlayerTransform = FindObjectOfType<PlayerMovement>().transform;
        }
        else
        {
            StartCoroutine(FindPlayerMovement());

        }
    }


    // Update is called once per frame
    void Update()
    {
        if(PlayerTransform != null)
        {
            Vector3 PlayerPos = PlayerTransform.position;
            transform.position = PlayerPos + offset;
        }
    }

    IEnumerator FindPlayerMovement()
    {
        yield return new WaitForSeconds(0.5f);

        Debug.LogWarning("Seraching for Player object");

        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            PlayerTransform = FindObjectOfType<PlayerMovement>().transform;
        }
        else
        {
            StartCoroutine(FindPlayerMovement());
        }
    }


}//CLASS
