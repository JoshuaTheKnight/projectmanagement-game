using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;


public class tutorialManager : MonoBehaviour
{
    public gameManager gameManager;
    public movement movement;
    public int page;

    //Creates the class to parse the JSON into. Variable names !!!!MUST!!!! coincide with the JSON Key names
    [System.Serializable]
    public class Tutorial
    {
        public int Page;
        public string Content;
    }
    //Creates the wrapper for the array of Questions because JSON Utility only works with objects. Variable name !!!!MUST!!!! Coincide with the list name
    [System.Serializable]
    public class TutorialList
    {
        public Tutorial[] Tutorial;
    }
    

    private void OnEnable(){
        //lock cursor and make invisible
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
        //Start on first page
        page = 0;

        //Grabs the JSON file from the resources folder and turns it into a string
        TextAsset jsonFile = Resources.Load<TextAsset>("tutorialText");
        string jsonData = jsonFile.text;
        //Use Json Utility to turn the string into the question list
        TutorialList tutorialList = JsonUtility.FromJson<TutorialList>(jsonData);
        //initialize UI Elements
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        Label text = root.Q<Label>("TutorialText");

        Button btnPrev = root.Q<Button>("ButtonPrev");
        Button btnNext = root.Q<Button>("ButtonNext");
        //Display first page
        DisplayPage(btnPrev, btnNext, text, root, page, tutorialList, 1);
        
        AnimatePopIn(root);
        //Have buttons add or remove page within bounds of list, then display page
        btnPrev.clicked += () => {
           if(page != 0)
           {
                page-= 1;
           }
           DisplayPage(btnPrev, btnNext, text, root, page, tutorialList, 0);
        };
        btnNext.clicked += () => {
            if(page != tutorialList.Tutorial.Length - 1)
            {
                page += 1;
            }
            DisplayPage(btnPrev, btnNext, text, root, page, tutorialList, 1);
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
    //buttonClicked 0 = buttonPrev, 1 = buttonNext
    public void DisplayPage(Button prev, Button next, Label text, VisualElement root, int page, TutorialList tutorialList, int buttonClicked)
    {
        //Exit UI if exit button is pressed
        if((prev.text == "Exit" && buttonClicked == 0) || (next.text == "Exit" && buttonClicked == 1))
        {
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
            AnimatePopOut(root);
        }
        else
        {
           //Display new tutorial page
           text.text = tutorialList.Tutorial[page].Content;
           
           //Change certain buttons to exit if on first or last page
           if(page == 0)
           {
            prev.text = "Exit";
           }
           else if(page == tutorialList.Tutorial.Length - 1)
           {
            next.text = "Exit";
           }
           else
           {
            prev.text = "Previous";
            next.text = "Next";
           }
           
        }
    }
}