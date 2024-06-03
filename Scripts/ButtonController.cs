/* using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    public Button yourButton;

    void Start()
    {
        yourButton.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        if (yourButton.name.Equals("Replay") || yourButton.name.Equals("Play"))
        {
            SceneManager.LoadScene("InGameScene");
        } else if (yourButton.name.Equals("Home"))
        {
            SceneManager.LoadScene("MainScene");
        }
    }
} */
