using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    void Start()
    {
        if (ControllerManager.instance != null)
            ControllerManager.instance.AddPlayer(this);
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    public void AnalogMoved(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        ControllerManager.instance.analogStickMoved.Invoke(context.ReadValue<Vector2>());
    }

    public void ConfirmButtonClicked(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        ControllerManager.instance.confirmButtonPressed.Invoke();
    }

    public void RevertButtonClicked(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        ControllerManager.instance.revertButtonPressed.Invoke();
    }
}
