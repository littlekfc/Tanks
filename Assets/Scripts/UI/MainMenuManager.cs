using UnityEngine;
using System.Collections;

using Tanks.Networking;

namespace Tanks.UI
{
    public class MainMenuManager : Singleton<MainMenuManager>
    {
        public GameObject waitingIndicator;

        private void Start()
        {
            NetworkManager.Instance.onGameReady += OnReady;
            NetworkManager.Instance.onGameCancelled += OnCancelled;
            NetworkManager.Instance.onMatchingFailed += OnCancelled;
        }

        private void OnDestroy()
        {
            NetworkManager.Instance.onGameReady -= OnReady;
            NetworkManager.Instance.onGameCancelled -= OnCancelled;
            NetworkManager.Instance.onMatchingFailed -= OnCancelled;
        }

        private void ToggleWaiting(bool is_enabled)
        {
            waitingIndicator.SetActive(is_enabled);
        }

        private void OnReady()
        {
            ToggleWaiting(false);
            NetworkManager.Instance.LoadLevel("Battle");
        }

        private void OnCancelled()
        {
            ToggleWaiting(false);
        }

        public void OnCreateNewGame()
        {
            if (NetworkManager.Instance.CreateNewGame())
                ToggleWaiting(true);
        }

        public void OnJoinExistingGame()
        {
            if (NetworkManager.Instance.JoinExistingGame())
                ToggleWaiting(true);
        }

        public void OnExitGame()
        {
            Application.Quit();
        }

        public void OnCancel()
        {
            if (!NetworkManager.Instance.CancelGame())
                ToggleWaiting(false);
        }
    }
}