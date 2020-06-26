using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum GAMESTATE
{
    INTROSCREEN,
    FRIENDSSCREEN,
    MESSAGESCREEN
}


public class GameManager : MonoBehaviour
{

    #region SERIALIZE FIELDS

    [SerializeField] GameObject go_BackButton;

    [SerializeField] GameObject go_IntroPanel;

    [SerializeField] GameObject go_FreindPanel;

    [SerializeField] GameObject go_MessagePanel;

    #endregion

    #region PRIAVTE FEILDS

    string filePath;

    GAMESTATE currentGameState;

    #endregion

    #region UNITY METHODS
    void Start()
    {
        filePath = Application.persistentDataPath + "/Message";
    }
    private void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidBackButtonAction();
        }
    }
    #endregion

    #region PUBLIC METHODS AND BUTTON ACTION

    public GAMESTATE GetCurrentGameState()
    {
        return currentGameState;
    }

    public void SetGameState(GAMESTATE currentState)
    {
        ExitState();

        EnterState(currentState);
    }

    public void BackButton()
    {
        switch (currentGameState)
        {
            case GAMESTATE.MESSAGESCREEN:

                ExitState();

                go_FreindPanel.SetActive(true);

                break;

            default:

                Debug.LogError("Invalid Back Action");

                break;
        }
    }
    public void SaveMessageFile(string messageContents)
    {
        string file = Path.Combine(filePath + "/Message");

        if (File.Exists(filePath))
        {
            File.WriteAllText(filePath, messageContents);
        }
        else
        {
            Directory.CreateDirectory(filePath);

            File.WriteAllText(file, messageContents);
        }
    }
    public string GetMessageFile()
    {
        string messages=string.Empty;

        string file = Path.Combine(filePath + "/Message");

        if (File.Exists(file))
        {
            messages = File.ReadAllText(file);
        }

        return messages;
    }

    #endregion

    #region PRIAVTE METHODS

    private void ExitState()
    {
        go_BackButton.SetActive(false);

        go_FreindPanel.SetActive(false);

        go_IntroPanel.SetActive(false);

        go_MessagePanel.SetActive(false);
    }

    private void EnterState(GAMESTATE state)
    {

        currentGameState = state;

        switch(state)
        {
            case GAMESTATE.INTROSCREEN:

                go_IntroPanel.SetActive(true);

                break;

            case GAMESTATE.FRIENDSSCREEN:

                go_FreindPanel.SetActive(true);

                break;

            case GAMESTATE.MESSAGESCREEN:

                go_MessagePanel.SetActive(true);

                go_BackButton.SetActive(true);

                break;

            default:

                Debug.Log("Invalid State or State now found");

                break;

        }
    }

    private void AndroidBackButtonAction()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();

            return;
        }
    }

    #endregion
}
[Serializable]
public class ChatHistotry
{
    public List<ChatMessages> chatMessages = new List<ChatMessages>();
}

[Serializable]
public class ChatMessages
{
    public string message;

    public bool isMine;

    public ChatMessages()
    {

    }
}
