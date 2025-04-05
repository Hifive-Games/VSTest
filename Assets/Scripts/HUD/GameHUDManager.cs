using UnityEngine;
using UnityEngine.InputSystem;
using TMPro; // Eğer bir UI Text kullanıyorsan
public class GameHUDManager : MonoBehaviour
{
    [SerializeField] private PanelController gameHUDPanel;
    private void OnEnable()
    {
        var playerInput = new PlayerInputs();
        playerInput.PlayerHUD.Enable();
        playerInput.PlayerHUD.StopGame.performed += OnEscPressed; // ESC tuşuna basıldığında
    }
    private void OnEscPressed(InputAction.CallbackContext context)
    {
        GameEvents.OnGamePaused?.Invoke();
        OpengameHUDPanel();
    }
    private void OpengameHUDPanel()
    {
        if (gameHUDPanel != null && gameHUDPanel.gameObject.activeSelf==false)
        {
            Debug.Log("Açılacak panel: " + gameHUDPanel.name);
            PanelManager.Instance.OpenPanel(gameHUDPanel);
        }
        else
        {
            Debug.LogError("Hedef panel atanmamış veya açık: " + gameObject.name);
        }
    }
}
