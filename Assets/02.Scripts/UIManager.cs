using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance()
    {
        return instance;
    }

    public GameObject stagePanel;
    public GameObject stageClearPanel;
    public GameObject gameOverPanel;
    public CanvasGroup[] hpSliders;
    public GameObject[] stageActiveImages;

    public GameObject optionPanel;
    public Sprite[] optionSprites;
    public Image[] optionImages;

    public Text stageText;
    public Text stageFinalTimeText;
    public Text timeText;
    public float timer = 0;

    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void Update()
    {
        if (GameManager.Instance().isStart)
        {
            TimeControll();
        }
    }

    public void OptionClick()
    {
        if (optionPanel.activeSelf)
        {
            optionPanel.SetActive(false);
            optionImages[0].sprite = optionSprites[0];
        }
        else
        {
            optionPanel.SetActive(true);
            optionImages[0].sprite = optionSprites[1];
        }
    }

    public void StopButton()
    {
        if(Time.timeScale == 0)
        {
            Time.timeScale = 1;
            optionImages[1].sprite = optionSprites[2];
        }
        else
        {
            Time.timeScale = 0;
            optionImages[1].sprite = optionSprites[3];
        }
    }

    public void SoundButton()
    {
        AudioSource backGroundAudio = GameObject.Find("BackgroundSound").GetComponent<AudioSource>();
        AudioSource sfxAudio = GameObject.Find("AudioManager").GetComponent<AudioSource>();

        if(sfxAudio.volume == 0)
        {
            backGroundAudio.volume = 0.35f;
            sfxAudio.volume = 1;

            optionImages[2].sprite = optionSprites[4];
        }
        else
        {
            backGroundAudio.volume = 0;
            sfxAudio.volume = 0;

            optionImages[2].sprite = optionSprites[5];
        }
    }

    public void TimeControll()
    {
        timer += Time.deltaTime;
        float min = timer / 60 % 60;
        float sec = timer % 60;

        timeText.text = string.Format("{0:00}:{1:00}", min, sec);
    }

    public void HPSlider(int backNum)
    {
        StartCoroutine(HPSliderControll(backNum));
    }

    IEnumerator HPSliderControll(int backNum)
    {
        Slider _slider = hpSliders[backNum - 1].GetComponent<Slider>();

        while(_slider.value != 0)
        {
            _slider.value -= 0.2f;
            hpSliders[backNum - 1].alpha = 0;

            yield return new WaitForSeconds(0.15f);
            hpSliders[backNum - 1].alpha = 1;
            yield return new WaitForSeconds(0.15f);
        }
    }

    public void StageClear()
    {
        PlayerPrefs.SetInt("ClearLv", GameManager.Instance().currentLv);

        stageFinalTimeText.text = string.Format("Time {0:00}:{1:00}", timer / 60 % 60, timer % 60);
        GameManager.Instance().isStart = false;
        stageClearPanel.SetActive(true);
    }

    public void GameOver()
    {
        GameManager.Instance().isStart = false;
        gameOverPanel.SetActive(true);
    }

    public void GameStart()
    {
        SceneManager.LoadScene(1);
        PlayerPrefs.SetInt("ClearLv", 1);
        PlayerPrefs.SetInt("CurrentLv", 1);
    }

    public void GameContinueStart()
    {
        stagePanel.SetActive(true);
        int clearStage = PlayerPrefs.GetInt("ClearLv");

        for (int i = 1; i <= 10; i++)
        {
            if (clearStage < i)
            {
                stageActiveImages[i-1].SetActive(true);
            }
            else
            {
                stageActiveImages[i - 1].SetActive(false);
            }
        }
    }

    public void StageStart(int _stage)
    {
        SceneManager.LoadScene(1);
        PlayerPrefs.SetInt("CurrentLv", _stage);
    }

    public void HomeButton()
    {
        SceneManager.LoadScene(0);
    }

    public void OnExit()
    {
        Application.Quit();
    }
}
