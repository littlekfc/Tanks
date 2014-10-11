using UnityEngine;
using System.Collections;
using System;

using Dummy;
using Players;
using Battle;

namespace Networking
{
    public class NetworkManager : Singleton<NetworkManager>
    {
        private RoomInfo[] rooms;

        public event Action onGameReady;
        public event Action onGameCancelled;
        public event Action onMatchingFailed;

        private const int READY_PLAYER_COUNT = 2;

        private PhotonView photonView;

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

            PhotonNetwork.automaticallySyncScene = true;
            photonView = PhotonView.Get(this);

            PhotonNetwork.logLevel = PhotonLogLevel.Full;

            DontDestroyOnLoad(transform.parent);
        }

        void Start()
        {
            PhotonNetwork.ConnectUsingSettings(ApplicationInfo.Version);
        }

        private void SetMyTeam(Team.TeamID id)
        {
            BattleManager.Instance.MyTeamID = id;

            if (!PhotonNetwork.isMasterClient)
                photonView.RPC("OnGameReady", PhotonTargets.MasterClient);
        }

        [RPC]
        private void SetMyTeam(int id)
        {
            SetMyTeam((Team.TeamID)id);
        }

        #region Networking_Callbacks
        void OnPlayerJoined()
        {
            Debug.Log("Joined!!!!!!!!!!! Current players: " + PhotonNetwork.room.playerCount);

            if (PhotonNetwork.room.playerCount == READY_PLAYER_COUNT && PhotonNetwork.isMasterClient)
            {
                SetMyTeam(Team.TeamID.BLUE);
                photonView.RPC("SetMyTeam", PhotonNetwork.otherPlayers[0], Team.TeamID.RED);
            }
        }

        [RPC]
        void OnGameReady()
        {
            if (onGameReady != null)
                onGameReady();
        }
        #endregion

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

        public void LoadLevel(string level_name, bool is_sync = true)
        {
            //PhotonNetwork.automaticallySyncScene = is_sync;
            PhotonNetwork.LoadLevel(level_name);
        }

        public void LoadLevel(int level_id, bool is_sync = true)
        {
            //PhotonNetwork.automaticallySyncScene = is_sync;
            PhotonNetwork.LoadLevel(level_id);
        }
    }
}