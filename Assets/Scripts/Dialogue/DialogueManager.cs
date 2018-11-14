using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    // Dialogue System Reference Variables
    [SerializeField] private TextMeshProUGUI NameText;
    [SerializeField] private TextMeshProUGUI DialogueText;
    [SerializeField] private Image DialogueImage;
    [SerializeField] private List<Conversation> ConversationList;

    // Private Variables
    private int conversationCount;
    private bool conversationInProgress;

    // Constant Values
    private float SCROLL_TEXT_DELAY = 0.05f;

    // Singleton Pattern
    private static DialogueManager instance;
    public static DialogueManager Instance { get { return instance; } }

    // Establish Singleton Reference
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Use this for initialization
    void Start ()
    {
        conversationCount = 0;
        conversationInProgress = false;
	}


    // Starts the Next Conversation in the ConversationList
    public void NextConversation()
    {
        if (conversationInProgress) { return; }

        // Find the Next Conversation to Display
        Conversation nextConversation = ConversationList[conversationCount];
        conversationCount++;

        // If all Conversations have been used, reset ConversationCount
        if(conversationCount >= ConversationList.Count)
        {
            conversationCount = 0;
        }

        //Start next Conversation
        StartCoroutine(Converse(nextConversation));
    }

    // Starts a specific Conversation in the ConversationList
    public void SelectConversation(int index)
    {
        if (conversationInProgress) { return; }

        // If the given index exists in the ConversationList, start THAT Conversation
        if (index >= 0 && index < ConversationList.Count)
        {
            Conversation nextConversation = ConversationList[conversationCount];

            StartCoroutine(Converse(nextConversation));
        }
    }

    // Display all Messages in a Conversation
    IEnumerator Converse(Conversation newConversation)
    {
        // TODO: Stun / Unstun Player during Conversation

        ToggleWindow(true);
        conversationInProgress = true;
        
        // Iterate through all Messages in this Conversation
        for (int i = 0; i < newConversation.messages.Count; i++)
        { 
            Message newMessage = newConversation.messages[i];
            string newSpeaker = newMessage.speaker;
            string newText = newMessage.text;

            // Display the current Speaker
            NameText.text = newSpeaker;

            // Iterate through all Characters in this Message
            for (int j = 0; j <= newText.Length; j++)
            {
                // Skip to the end if "X" is pressed or the player right-clicks
                if (Input.GetKey(KeyCode.X) || Input.GetMouseButton(1)) { j = newText.Length; }

                // Display newText one Character at a time
                DialogueText.text = newText.Substring(0, j);
                yield return new WaitForSeconds(SCROLL_TEXT_DELAY);
            }

            // Wait to display next Message until "Z" or click is pressed or the player left-clicks
            while(!Input.GetKeyDown(KeyCode.Z) && !Input.GetMouseButtonDown(0))
            {
                yield return null;
            }
        }

        conversationInProgress = false;
        ToggleWindow(false);

    }

    // Hides or Displays the Dialogue UI
    private void ToggleWindow(bool status)
    {
        DialogueImage.gameObject.SetActive(status);
        NameText.gameObject.SetActive(status);
        DialogueText.gameObject.SetActive(status);
    }
}
