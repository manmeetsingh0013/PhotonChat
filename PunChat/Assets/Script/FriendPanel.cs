using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendPanel : MonoBehaviour
{
    [SerializeField] GameManager gameManager;

    [SerializeField] GameObject friendPrefab;

    [SerializeField] Text user;

    [SerializeField] Transform friendParent;

    public List<Friend> friends = new List<Friend>();

    public void SetData(string userName,List<string> friendsList)
    {
        user.text = "Welcome " + userName + "!";

        for (int i = 0; i < friendsList.Count; i++)
        {
            GameObject go_Friend = Instantiate(friendPrefab,friendParent) as GameObject;

            go_Friend.name = friendsList[i];

            Friend friend  = go_Friend.GetComponent<Friend>();

            friend.SetData(go_Friend.name,transform);

            friends.Add(friend);
        }
    }

    public void SetMessageState()
    {
        gameManager.SetGameState(GAMESTATE.MESSAGESCREEN);
    }
}
