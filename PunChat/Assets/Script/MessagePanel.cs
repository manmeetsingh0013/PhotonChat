using UnityEngine;
using UnityEngine.UI;
using Photon.Chat;
using System.Collections;

public class MessagePanel : MonoBehaviour
{
    #region SERIALIZE FIELDS

    [SerializeField] InputField messageTypeField;

    [SerializeField] GameObject messagePrefab;

    [SerializeField] ScrollRect scrollRect;

    [SerializeField] GameManager gameManager;
    #endregion

    #region PRIVATE FIELDS

    public ChatHistotry chatHistotry = new ChatHistotry();

    RectTransform rectTransform;

    bool isMine;

    const int minChildCanBeOnScreen = 13;

    #endregion

    private void Start()
    {
        rectTransform = scrollRect.content.GetComponent<RectTransform>();

        LoadMessage();
    }

    #region PRIVATE METHODS

    private void ShowMessage(string message,bool isMine)
    {
        GameObject go_Message = Instantiate(messagePrefab,scrollRect.content);

        go_Message.GetComponent<Message>().ShowMessage(message,isMine);
    }

    private void LoadMessage()
    {
        string allPreviousMsg = gameManager.GetMessageFile();

        chatHistotry = JsonUtility.FromJson<ChatHistotry>(allPreviousMsg);

        foreach (var item in chatHistotry.chatMessages)
        {
            ShowMessage(item.message, item.isMine);
        }

        int child = chatHistotry.chatMessages.Count - minChildCanBeOnScreen;

        StartCoroutine(SetContentPosition(child,0.2f));

    }

    #endregion

    #region COROUTINES

    IEnumerator SetContentPosition(int childCount=1,float duration=1)
    {
        float time = 0;

        Vector2 newPosition = new Vector2(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y + childCount * 120);

        while (time < duration)
        {
            time += Time.deltaTime;

            rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, newPosition, time*5);

            yield return null;
        }
    }

    #endregion

    #region BUTTON ACTIONS

    public void SendMessageToFriend()
    {
        string msg = messageTypeField.text;

        if (!string.IsNullOrEmpty(msg))
        {
            Chat.chatClient.SendPrivateMessage(Chat.privateChatTarget, msg);
        }

        messageTypeField.text = "";
    }

    #endregion

    #region PUBLIC METHODS

    public void GetMessageFromFriend(string sender,object message,string _channelName)
    {
        try
        {
            isMine = sender == PlayerPrefs.GetString(Chat.playerNameKey);

            ShowMessage(message.ToString(), isMine);

            ChatMessages chat = new ChatMessages();

            chat.isMine = isMine;

            chat.message = message.ToString();

            chatHistotry.chatMessages.Add(chat);

            StartCoroutine(SetContentPosition());
        }
        catch(System.Exception e)
        {
            Debug.LogError(e.Message);
        }

    }

    public void OnDisConnection()
    {
        if (chatHistotry.chatMessages.Count != 0)
        {
            string messageJson = JsonUtility.ToJson(chatHistotry);

            gameManager.SaveMessageFile(messageJson);
        }
    }
    #endregion

}

