using UnityEngine;

public class QuitGame : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            LeaveGame();
        }
    }

    public void LeaveGame()
    {
        Application.Quit();
    }
}
