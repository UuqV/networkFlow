using UnityEngine;
using UnityEngine.InputSystem;

public class QuitGame : MonoBehaviour
{
    private @InputSystem_Actions controls;

    void Awake()
    {
        controls = new @InputSystem_Actions();

        // When Escape is pressed
        controls.UI.Quit.performed += ctx => Quit();
    }

    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }

    void Quit()
    {
        Debug.Log("Quit action triggered");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // For editor
#else
        Application.Quit(); // For build
#endif
    }
}
