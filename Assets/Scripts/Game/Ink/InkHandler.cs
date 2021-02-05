using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;
using Ink.Runtime;

public class InkHandler : MonoBehaviour
{
    //ink related assets
    public static event Action<Story> OnCreateStory;
    public TextAsset inkJSONAsset;
    public Story story;

    //UI elements
	[SerializeField]
    private GameObject dialogue = null;
    [SerializeField]
    private Text mainText = null;
    [SerializeField]
  
    private Button option1 = null;
    [SerializeField]
    private Button option2 = null;
    [SerializeField]
    private Button option3 = null;

    public string currentLine;

	//hide dialogue on startup
	private void Awake() {
		HideConversation();
	}
    //initialize ink
    public void StartConversation () {
		// Remove the default message
		//RemoveChildren();
		StartStory();
		DisplayConversation();
	}
	public void EndConversation () {
		HideConversation();
	}

    //initialize story content
    void StartStory () {
		story = new Story (inkJSONAsset.text);
        if(OnCreateStory != null) OnCreateStory(story);
		RefreshView();
	}
	private void HideConversation () {
		dialogue.SetActive(false);
	}
	private void DisplayConversation() {
		dialogue.SetActive(true);
	}
    //run everytime we may want to change screen content 
    void RefreshView () {
		// Remove all the UI on screen
		//RemoveChildren ();
		// Read all the content until we can't continue any more
        
		while (story.canContinue) {
			// Continue gets the next line of the story
			string text = story.Continue();
			// This removes any white space from the text.
			text = text.Trim();
			// Display the text on screen!
			CreateContentView(text);
		}

		// Display all the choices, if there are any!
		if(story.currentChoices.Count > 0) {

			
            if (story.currentChoices.ElementAtOrDefault(0) != null) {
                Choice choice = story.currentChoices[0];
                option1.GetComponentInChildren<Text>().text = "• "+choice.text.Trim();
				option1.onClick.RemoveAllListeners();
                option1.onClick.AddListener (delegate {
					OnClickChoiceButton (choice);
				});
            } else {option1.GetComponentInChildren<Text>().text = "";}
              if (story.currentChoices.ElementAtOrDefault(1) != null) {
                Choice choice = story.currentChoices[1];
				option2.onClick.RemoveAllListeners();
                option2.GetComponentInChildren<Text>().text = "• "+choice.text.Trim();
                option2.onClick.AddListener (delegate {
					OnClickChoiceButton (choice);
				});
            } else {option2.GetComponentInChildren<Text>().text = "";}
              if (story.currentChoices.ElementAtOrDefault(2) != null) {
                Choice choice = story.currentChoices[2];
				option3.onClick.RemoveAllListeners();
                option3.GetComponentInChildren<Text>().text = "• "+choice.text.Trim();
                option3.onClick.AddListener (delegate {
					OnClickChoiceButton (choice);
				});
            } else {option3.GetComponentInChildren<Text>().text = "";}
		}
		// If we've read all the content and there's no choices, the story is finished!
		else {
			Debug.Log("Ending Conversation");
			EndConversation();

		}
	}
    //Click listener
	void OnClickChoiceButton (Choice choice) {
		Debug.Log(choice.index);
		story.ChooseChoiceIndex (choice.index);
		RefreshView();
	}
    //makes a text box that displays the current dialogue
    void CreateContentView (string text) {
        mainText.text = text;
	}
}
