using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


namespace Com.MyCompany.MyGame
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        #region Private Serializable Fields
        
        [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
        [SerializeField]
        private byte maxPlayersPerRoom = 4;

        [Tooltip("The UI Panel to let the user enter name, connect and plau")]
        [SerializeField]
        private GameObject controlPanel;

        [Tooltip("The UI Label to inform the user that the connection is in progress")] 
        [SerializeField]
        private GameObject progressLabel;
        
        
        #endregion
        
        #region Private Fields


        /// <summary>
        /// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
        /// </summary>
        string gameVersion = "1";

        private bool isConnecting;


        #endregion
        
        #region MonoBehaviour CallBacks
        
        void Awake()
        {
            // #Critical
            // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        
        void Start()
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
        }


        #endregion
        
        #region Public Methods
        
        /// <summary>
        /// Start the connection process.
        /// - If already connected, we attempt joining a random room
        /// - if not yet connected, Connect this application instance to Photon Cloud Network
        /// </summary>
        public void Connect()
        {
            progressLabel.SetActive(true);
            controlPanel.SetActive(false);

            isConnecting = true;
            
            // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
            if (PhotonNetwork.IsConnected)
            {
                // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                // #Critical, we must first and foremost connect to Photon Online Server.
                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }
        }

        #endregion
    
        #region MonoBehaviourPunCallbacks Callbacks

        public override void OnConnectedToMaster()
        {
            if (isConnecting)
            { 
                Debug.Log("PUN Basic Launcher: OnConnectedToMaster() was called by PUN");
                PhotonNetwork.JoinRandomRoom();
            }
         }

        public override void OnDisconnected(DisconnectCause cause)
        { 
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
            
            Debug.LogWarningFormat("PUN Basic Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
            isConnecting = false;

        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("PUN Basics Launcher: OnJoinRandomFailed() was called by PUN. No random room available so we can create on. \nCalling: PhotonNetwork.CreateRoom");

            PhotonNetwork.CreateRoom(null, new RoomOptions{ MaxPlayers = maxPlayersPerRoom});
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("PUN Basics Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");

            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                Debug.Log("We load the 'Room for 1'");
                PhotonNetwork.LoadLevel("Room for 1");
            }
        }

        #endregion
        

    }
}