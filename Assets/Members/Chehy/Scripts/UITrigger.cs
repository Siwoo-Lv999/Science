using UnityEngine;
using UnityEngine.Serialization;

public class UITrigger : MonoBehaviour
{
    [SerializeField] private PlayerSet player;
    [FormerlySerializedAs("SpeakerTrampolineUI")]
    [SerializeField] private GameObject speakerTrampolineUI;
    [FormerlySerializedAs("ThrowMachineUI")]
    [SerializeField] private GameObject throwMachineUI;

    private void OnEnable()
    {
        if (player == null)
            return;

        player.OnStateChanged += HandleStateChanged;
        HandleStateChanged(player.CurrentState);
    }

    private void OnDisable()
    {
        if (player != null)
            player.OnStateChanged -= HandleStateChanged;
    }

    private void HandleStateChanged(PlayerState state)
    {
        speakerTrampolineUI.SetActive(state == PlayerState.OnSpeaker);
        throwMachineUI.SetActive(state == PlayerState.OnThrowMachine);
    }
}
