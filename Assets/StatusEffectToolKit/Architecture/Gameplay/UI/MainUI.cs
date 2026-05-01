/*
 * Description: Handles main UI logic
 */

using Service.Framework.StatusSystem;
using UnityEngine;
using Service.Framework.Tools;
using UnityEngine.UI;

namespace Gameplay.UI
{
    public class MainUI : MonoBehaviour
    {
        [SerializeField]
        private Text statusLevelText;
        [SerializeField]
        private Text statusEffectText;

        [SerializeField]
        private GameObject uiContent;
        [SerializeField]
        private GameObject contextualButtonUi;

        private bool isContentVisible = false;

        private void Start()
        {
            StatusManager.Instance.OnStatusLevelChanged.AddListener(UpdateStatusLevel);
            StatusEffectManager.Instance.OnStatusEffectSelected.AddListener(UpdateStatusEffect);
        }

        public void ToggleContent()
        {
            isContentVisible = !isContentVisible;

            uiContent.SetActive(isContentVisible);
        }

        private void UpdateStatusLevel(int level, StatusEffectSettings.StatusLevelConfig effect)
        {
            statusLevelText.text = level.ToString();
        }

        private void UpdateStatusEffect(StatusEffectsData effect)
        {
            if (StatusManager.Instance.StatusLevel < StatusManager.Instance.StatusSettings.statusLevels.Count)
            {
                statusEffectText.text = effect.statusEffect.ToString();
            }
        }

        public void SetContextualUiVisible(bool state)
        {
            contextualButtonUi.SetActive(state);
        }

        private void OnDestroy()
        {
            StatusManager.Instance.OnStatusLevelChanged.RemoveListener(UpdateStatusLevel);
            StatusEffectManager.Instance.OnStatusEffectSelected.RemoveListener(UpdateStatusEffect);
        }
    }
}