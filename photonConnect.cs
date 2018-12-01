using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class photonConnect : MonoBehaviour {

    public string versionName = "v0.1";
    public static bool host = false;
    public InputField createRoomInput, joinRoomInput;

    public GameObject start, ConnectedScreen;

    //Call to connect to server
    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings(versionName);
        Debug.Log("Connecting to photon...");
    }


    //Call when connected to server
    private void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
        Debug.Log("You are connected to master server.");
    }

    //Call when connected to general lobby and scene change
    private void OnJoinedLobby()
    {
        start.SetActive(false);
        ConnectedScreen.SetActive(true);
        Debug.Log("Joined Lobby.");
    }   

    //function used for button
    //when button is pressed, function is called
    //checks to see if input is of proper format and creates a 4-player room with the input as password/room name
    public void onClickCreateRoom()
    {
        host = true;
        if (createRoomInput.text.Length >= 1)
            PhotonNetwork.CreateRoom(createRoomInput.text, new RoomOptions() { MaxPlayers = 4 }, null);
    }

    //function called when button is pressed
    //If the input matches the created room, allows player to join the room
    public void onClickJoinRoom()
    {
        host = false;
        PhotonNetwork.JoinRoom(joinRoomInput.text);
    }

    //call when successful connection to joined room
    public void OnJoinedRoom()   {      

       
        Debug.Log("Connected to room.");
        Debug.Log(PhotonNetwork.playerList.ToString());
        
    }
   

}
