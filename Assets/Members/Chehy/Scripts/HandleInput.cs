using UnityEngine;
using UnityEngine.UI;

namespace Members.Chehy.Scripts
{
    public class HandleInput : MonoBehaviour
    {
        [SerializeField] private CurrentModel currentModel;
        [SerializeField] private PlayerSet player;

        [SerializeField] private Button plusButton;
        [SerializeField] private Button minusButton;
        [SerializeField] private Button throwButton;

        private void Start()
        {
            plusButton.onClick.AddListener(IncreaseStrength);
            minusButton.onClick.AddListener(DecreaseStrength);
            throwButton.onClick.AddListener(Throw);
        }

        private void IncreaseStrength()
        {
            currentModel.ChangeStrength(1);
        }

        private void DecreaseStrength()
        {
            currentModel.ChangeStrength(-1);
        }

        private void Throw()
        {
            if (player.CanThrow)
            {
                player.Throw();
            }
        }

        private void OnDestroy()
        {
            plusButton.onClick.RemoveListener(IncreaseStrength);
            minusButton.onClick.RemoveListener(DecreaseStrength);
            throwButton.onClick.RemoveListener(Throw);
        }
    }
}