using UnityEngine;
using UnityEngine.UI;
using Photon.Chat;
public class Friend : MonoBehaviour
{
    [SerializeField] Text nameText;

    [SerializeField] Text statusText;

    [SerializeField] Text msg;

    Transform parent;

    public void SetData(string name,Transform _parent)
    {
        parent = _parent;

        nameText.text = name;

        gameObject.name = name;

    }
    public void OnSelect()
    {
        GameObject go_FriendPanel = parent.gameObject;

        go_FriendPanel.GetComponent<FriendPanel>().SetMessageState();

        go_FriendPanel.SetActive(false);

        Chat.privateChatTarget = nameText.text.ToString();
    }

    public void OnFriendStatusUpdate(int status, bool gotMessage, object message)
    {
        string _status;

        switch (status)
        {
            case 1:
                _status = "Invisible";
                break;
            case 2:
                _status = "Online";

                statusText.color = Color.green;

                break;
            case 3:
                _status = "Away";
                break;
            case 4:
                _status = "Do not disturb";
                break;
            case 5:
                _status = "Looking For Game/Group";
                break;
            case 6:
                _status = "Playing";
                break;
            default:
                _status = "Offline";

                statusText.color = Color.red;

                break;
        }

        statusText.text = _status;

        if(gotMessage && message !=null)
        {
            Debug.Log("got messsage---> " + message.ToString());

            msg.text = message.ToString();
        }
    }
}
