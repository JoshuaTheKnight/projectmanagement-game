using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;

public class endDayManager : MonoBehaviour
{
    public gameManager gameManager;
    public scenarioManager scenarioManager;
    public uiManager quizManager;
    public movement movement;

    private void OnEnable(){

        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
        
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        Label text = root.Q<Label>("EndDayText");

        Button btn = root.Q<Button>("EndDayButton");

        if(scenarioManager.attempts != 0){
            text.text = "The day isn't over yet goofy, you still have some scenarios left at the whiteboard!";
            btn.text = "Return";
        }else if(quizManager.attempts != 0){
            text.text = "Don't think you can leave without doing your quiz bubby. Get to the computer now";
            btn.text = "Return";
        }else{
            text.text = "Good job today. Hopefully tomorrow is even more successful.";
            scenarioManager.attempts = 3;
            quizManager.attempts = 1;
            btn.text = "Start Next Day";
        }

        AnimatePopIn(root);

        btn.clicked += () => {
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
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
