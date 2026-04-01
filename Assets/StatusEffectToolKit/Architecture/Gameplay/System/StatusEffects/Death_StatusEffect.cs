
/*
Description: Handles logic for when the player dies
*/

using Service.Framework.Health;
using Service.Framework.StatusSystem;

namespace Gameplay.System.StatusSystem
{
    public class Death_StatusEffect : StatusEffect
    {
        private HealthHandler playerHealth;

        private void Start()
        {
            playerHealth = ReferenceRegistry.Instance.Player.GetComponent<HealthHandler>();
        }

        public override void HandleRunStart()
        {
            //player is now dead
            playerHealth.SetHealth(0);
        }
    }
}