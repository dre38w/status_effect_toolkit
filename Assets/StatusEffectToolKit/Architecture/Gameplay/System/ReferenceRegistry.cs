/*
 * Description: Holds references to objects to better organize when some objects need to be referenced many times, dynamically, etc. 
 */

using Gameplay.System.Player;
using Gameplay.UI;
using Service.Framework;
using UnityEngine;

namespace Gameplay.System
{
    public class ReferenceRegistry : MonoBehaviour
    {
        public static ReferenceRegistry Instance;

        private PlayerController player;
        public PlayerController Player
        {
            get { return player; }
            set { player = value; }
        }

        private MainUI mainUi;
        public MainUI MainUi
        {
            get { return mainUi; }
            set { mainUi = value; }
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
            player = GameObject.FindWithTag(TagData.PLAYER_TAG).GetComponent<PlayerController>();
            mainUi = GameObject.FindWithTag(TagData.MAIN_UI_TAG).GetComponent<MainUI>();
        }
    }
}