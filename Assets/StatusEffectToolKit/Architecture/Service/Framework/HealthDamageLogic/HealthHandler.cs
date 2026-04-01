
/*
Description: An ambiguous reusable class to manage health logic
*/

using UnityEngine;
using UnityEngine.Events;

namespace Service.Framework.Health
{
    public class HealthHandler : MonoBehaviour
    {
        public enum CurrentState
        {
            Alive,
            Dead,
            Stunned,
            Fainted,
        }
        public CurrentState State;

        public class OnHealthChangedEvent : UnityEvent<float, GameObject> { }
        public OnHealthChangedEvent OnHealthUpdated = new OnHealthChangedEvent();

        [SerializeField]
        private float maxHealth = 100.0f;
        private float currentHealth;
        public float CurrentHealth => currentHealth;

        private void Start()
        {
            currentHealth = maxHealth;
        }

        public void SetMaxHealth(float value)
        {
            maxHealth = value;
        }

        public void AdjustMaxHealth(float value)
        {
            maxHealth += value;
        }

        public void SetHealth(float value)
        {
            currentHealth = value;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

            if (currentHealth <= 0)
            {
                SetState(CurrentState.Dead);
            }
            OnHealthUpdated.Invoke(currentHealth, this.gameObject);
        }

        public void AdjustHealth(float value)
        {
            currentHealth += value;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

            if (currentHealth <= 0)
            {
                SetState(CurrentState.Dead);
            }
            OnHealthUpdated.Invoke(currentHealth, this.gameObject);
        }

        public void SetState(CurrentState state)
        {
            if (State == state)
            {
                return;
            }
            State = state;
        }
    }
}