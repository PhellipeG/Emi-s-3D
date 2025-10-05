using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitadorDeCena : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Menu");
        }
    }
}
