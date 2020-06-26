using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

namespace Photon.Chat
{
    using ExitGames.Client.Photon;
    using System.Collections.Generic;

    public class Chat : MonoBehaviourPunCallbacks, IChatClientListener
    {
        [SerializeField] GameObject friendPanel;

        [SerializeField] GameManager gameManager; 

        public static ChatClient chatClient;

        public InputField playerNameInputField;

        public TextMeshProUGUI connectionState;

        public Text chatState;

        public GameObject connectionStatus;

        private string worldChat="Friends";

        public GameObject msgPanel;

        public const string playerNameKey = "PlayerName";

        public static string privateChatTarget;

        private ChatChannel chatChannel;

        List<string> frindList = new List<string> { "Nitil" ,"Raj" };

        bool isGamePaused;

        #region UNITY METHODS

        private void Start()
        {

            if (string.IsNullOrEmpty(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat))
            {
                Debug.Log("No chat Id provided!");
            }

            if (PlayerPrefs.HasKey(playerNameKey))
            {
                connectionState.text = "connecting.....";

                StartChatClient();
            }
            else
            {
                gameManager.SetGameState(GAMESTATE.INTROSCREEN);

                connectionStatus.SetActive(false);
            }
        }
        private void Update()
        {
            if (chatClient != null)
                chatClient.Service();
        }

        private void OnApplicationPause(bool pause)
        {
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                if (pause)
                {
                    isGamePaused = pause;

                    StartChatClient();
                }
            }
        }

        private void OnApplicationQuit()
        {
            msgPanel.GetComponent<MessagePanel>().OnDisConnection();
        }

        #endregion

        void RemoveThisUserFromTheFriendList()
        {
            frindList.Remove(PlayerPrefs.GetString(playerNameKey));
        }

/*        public void SendMessage()
        {
            string msg = msgInput.text;

            if (!string.IsNullOrEmpty(msg))
            {
                chatClient.SendPrivateMessage(privateChatTarget, msg);
            }

            msgInput.text = "";

        }*/

        public void StartChatClient()
        {
            if (!PlayerPrefs.HasKey(playerNameKey))
            {
                PlayerPrefs.SetString(playerNameKey, playerNameInputField.text);

                PhotonNetwork.NickName = playerNameInputField.text;
            }

            chatClient = new ChatClient(this);

            chatChannel = new ChatChannel(worldChat);

            chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, "anything", new AuthenticationValues(PlayerPrefs.GetString(playerNameKey)));

            connectionState.text = "Connecting and loading friends...";

        }

        public void DeleteAllPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
        }

        #region CHAT CLIENT CALLBACKS

        public void DebugReturn(DebugLevel level, string message)
        {

        }
        public  void  OnDisconnected()
        {
            Debug.Log("Disconnected..");

            connectionStatus.SetActive(true);

            msgPanel.GetComponent<MessagePanel>().OnDisConnection();

            chatState.text = "Disconnected...";
        }

        public  void OnConnected()
        {
            Debug.Log("on connected ......");

            connectionState.text = "connected";

            if (!isGamePaused)
            {
                RemoveThisUserFromTheFriendList();

                gameManager.SetGameState(GAMESTATE.FRIENDSSCREEN);

                friendPanel.GetComponent<FriendPanel>().SetData(PlayerPrefs.GetString(playerNameKey), frindList);
            }

            chatClient.AddFriends(frindList.ToArray());

            chatClient.Subscribe(new string[] { worldChat });

            chatClient.SetOnlineStatus(ChatUserStatus.Online);

            HashSet<string> subscribers = chatChannel.Subscribers;

            Debug.Log("Hashset ________> " + subscribers.Count);

            foreach (var item in subscribers)
            {
                Debug.Log(item);
            }      

        }
        
        public void OnApplicationClose()
        {
            Application.Quit();
        }
        public  void OnChatStateChange(ChatState state)
        {
            chatState.text = state.ToString() + "...";

            connectionStatus.SetActive(true);

            if(state.ToString() == "ConnectedToFrontEnd")
            {
                connectionStatus.SetActive(false);
            }
        }

        public  void OnGetMessages(string channelName, string[] senders, object[] messages)
        {
            for(int i=0;i< senders.Length;i++)
            {
                //msgArea.text += senders[i] + ": " + messages[i] + "\n";
            }

        }

        public  void OnPrivateMessage(string sender, object message, string channelName)
        {
            msgPanel.GetComponent<MessagePanel>().GetMessageFromFriend(sender,message,channelName);
        }

        public void OnSubscribed(string[] channels, bool[] results)
        {

        }

        public void OnUnsubscribed(string[] channels)
        {

        }

        public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
        {
            Debug.Log("Status :---> " + string.Format("{0} is {1} , Message--- > {2}", user, status, message));

            Friend friend = friendPanel.GetComponent<FriendPanel>().friends.Find(x => user.Equals(x.name));

            if(friend !=null)
            {
                friend.OnFriendStatusUpdate(status, gotMessage, message);
            }

        }

        public void OnUserSubscribed(string channel, string user)
        {

        }

        public void OnUserUnsubscribed(string channel, string user)
        { 

        }

        #endregion
    }
}

