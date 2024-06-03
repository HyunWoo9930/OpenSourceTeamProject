/* using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthSliderController : MonoBehaviour
{
    public Slider healthSlider;
    [SerializeField]
    public float decreaseRate;

    void Start()
    {
        healthSlider.value = 1.0f;
    }

    void Update()
    {
        if (healthSlider.value > 0)
        {
            healthSlider.value -= decreaseRate * Time.deltaTime;
            Debug.Log("health = " + healthSlider.value);
        }

        if (healthSlider.value == 0)
        {
            TaskOnClick();
        }
    }


    void TaskOnClick()
    {
        SceneManager.LoadScene("GameOverScene");
    }
}  */
