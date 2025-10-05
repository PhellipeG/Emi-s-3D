using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Iniciar();
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Sair();
        }
    }

    public void Iniciar()
    {
        SceneManager.LoadScene("mapa");
    }
    public void Sair()
    {
        Application.Quit();
    }
}
