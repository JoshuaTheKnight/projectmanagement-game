using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;

public class GameOverScript : MonoBehaviour
{
    public gameManager gameManager;
    public movement movement;
    
    private void OnEnable(){
        
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
        
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        Button newGame = root.Q<Button>("GameOverButton");

        AnimatePopIn(root);

        newGame.clicked += () => {
            gameManager.Restart();
            AnimatePopOut(root);
        };
    }

    public void AnimatePopIn(VisualElement rootElement)
    {
        movement.enabled = false;
        //Sets the scale to zero, then does a .5 second pop in from 0 scale to 1
        rootElement.transform.scale = Vector3.zero;
        DOTween.To(() => rootElement.transform.scale, x => rootElement.transform.scale = x, Vector3.one, 0.5f);
    }

    public void AnimatePopOut(VisualElement rootElement)
    {
        movement.enabled = true;
        //Set scale to one, then do a .5 second pop out from 1 scale to 0, disable element on completion
        rootElement.transform.scale = Vector3.one;
        DOTween.To(() => rootElement.transform.scale, x => rootElement.transform.scale = x, Vector3.zero, 0.5f)
        .OnComplete(() => this.gameObject.SetActive(false));
    }
}
