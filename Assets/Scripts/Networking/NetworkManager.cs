using UnityEngine;
using System.Collections;
using System;

using Dummy;

namespace Networking
{
    public class NetworkManager : Singleton<NetworkManager>
    {
        private RoomInfo[] rooms;

        public event Action onGameReady;
        public event Action onGameCancelled;
        public event Action onError;
        public event Action onMatchingFailed;

        private const int READY_PLAYER_COUNT = 2;

        public bool IsConnectedToServer
        {
            get
            {
                return PhotonNetwork.connected;
            }
        }

        protected override void Awake()
        {
            base.Awake();

            DontDestroyOnLoad(gameObject);
        }

        void Start()
        {
            PhotonNetwork.ConnectUsingSettings(ApplicationInfo.Version);
        }

        void OnPlayerJoined()
        {
            Debug.Log("Joined!!!!!!!!!!! Current players: " + PhotonNetwork.room.playerCount);

            if (PhotonNetwork.room.playerCount == READY_PLAYER_COUNT)
                if (onGameReady != null)
                    onGameReady();
        }

        #region PNU_Callbacks
        void OnJoinedLobby()
        {
        }

        void OnJoinedRoom()
        {
            OnPlayerJoined();
        }

        void OnPhotonPlayerConnected()
        {
            OnPlayerJoined();
        }

        void OnLeftRoom()
        {
            if (onGameCancelled != null)
                onGameCancelled();
        }

        void OnPhotonRandomJoinFailed()
        {
            if (onMatchingFailed != null)
                onMatchingFailed();
        }

        void OnPhotonJoinRoomFailed()
        {
            if (onMatchingFailed != null)
                onMatchingFailed();
        }

        void OnLeftLobby()
        {
        }

        void OnDisconnectedFromPhoton()
        {
        }
        #endregion

        public bool CreateNewGame()
        {
            if (IsConnectedToServer)
                return PhotonNetwork.CreateRoom(Guid.NewGuid().ToString());
            else
                return false;
        }

        public bool JoinExistingGame()
        {
            if (IsConnectedToServer)
                return PhotonNetwork.JoinRandomRoom();
            else
                return false;
        }

        public bool CancelGame()
        {
            if (IsConnectedToServer)
                return PhotonNetwork.LeaveRoom();
            else
                return false;
        }
    }
}