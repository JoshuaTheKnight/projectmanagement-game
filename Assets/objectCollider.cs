using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class objectCollider : MonoBehaviour
{
    public GameObject text;
    public GameObject userInterface;
    public movement movement;

    // Start is called before the first frame update
    void Start()
    {
        text.SetActive(false);
    }

    void Update(){
        //If text is in hierarchy, that means player is near object, so initiate UI if E is pressed
        if(text.activeInHierarchy && Input.GetKeyDown(KeyCode.E)){
            userInterface.SetActive(true);
            movement.enabled = false;
        }
    }
    void OnTriggerEnter(Collider obj)
    {
        //activate text if player collides with object
        if(obj.gameObject.CompareTag("Player")){
            text.SetActive(true);
        }
    }

    void OnTriggerExit(Collider obj){
        //deactivate text if user isn't near object
        if(obj.gameObject.CompareTag("Player")){
            text.SetActive(false);
        }
    }
}
