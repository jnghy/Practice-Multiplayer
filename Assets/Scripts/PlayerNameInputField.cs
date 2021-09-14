using System;
using System.Collections;

using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

namespace Com.MyCompany.MyGame
{

    [RequireComponent(typeof(InputField))]
    public class PlayerNameInputField : MonoBehaviour
    {
        #region Private COnstrants

        private const string playerNamePrefKey = "PlayerName";

        #endregion

        #region Mono Behaviour CallBacks

        private void Start()
        {
            string defaultName = String.Empty;
            InputField _inputField = this.GetComponent<InputField>();

            if (_inputField != null)
            {
                if (PlayerPrefs.HasKey(playerNamePrefKey))
                {
                    defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                    _inputField.text = defaultName;
                }
            }

            PhotonNetwork.NickName = defaultName;

        }

        #endregion

        #region Public Methods

        public void SetPlayerName(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                Debug.LogError("Player Name is null or empty");
                return;
            }

            PhotonNetwork.NickName = value;
            
            PlayerPrefs.SetString(playerNamePrefKey,value);

        }

        #endregion
    }

}