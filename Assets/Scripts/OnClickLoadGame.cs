using UnityEngine;
using UnityEngine.SceneManagement;

public class OnClickLoadGame : MonoBehaviour
{
    public void OnClick()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene("LoadInScene", LoadSceneMode.Single);
    }
}
