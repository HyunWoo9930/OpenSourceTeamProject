using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public Player player;
    public DSLManager dslManager;
    public DontDestory dontDestroy;
    public GameObject[] players, stairs, ui;
    public GameObject pauseBtn;
    public GameObject[] backGrounds;

    public AudioSource[] sound;
    public Animator[] anim;
    public Text finalScoreText, bestScoreText, scoreText;
    public Image gauge;
    public Button[] settingButtons;

    private int _score, _sceneCount, _selectedIndex;
    public bool gaugeStart, vibrationOn = true, isGamePaused;
    float _gaugeReductionRate = 0.0025f;
    public bool[] isChangeDir = new bool[20];

    private Vector3 _beforePosition;
    private readonly Vector3
    _startPosition = new (-0.8f, -1.2f, 0),
    _leftPosition = new (-0.8f, 0.4f, 0),
    _rightPosition = new (0.8f, 0.4f, 0),
    _leftDirection = new (0.8f, -0.4f, 0),
    _rightDirection = new (-0.8f, -0.4f, 0);

    private enum State { Start, LeftDir, RightDir }
    private State _state = State.Start;

    void Awake() {
        players[_selectedIndex].SetActive(true);
        player = players[_selectedIndex].GetComponent<Player>();

        StairsInit();
        GaugeReduce();
        StartCoroutine("CheckGauge");

        ui[0].SetActive(dslManager.IsRetry());
        ui[1].SetActive(!dslManager.IsRetry());        
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            BtnDown(GameObject.Find("ClimbBtn"));
        } else if (Input.GetKeyDown(KeyCode.Space)) {
            BtnDown(GameObject.Find("ChangeDirBtn"));
        }
    }
    
    void StairsInit() {
        for (int i = 0; i < 20; i++) {
            switch (_state) {
                case State.Start:
                    stairs[i].transform.position = _startPosition;
                    _state = State.LeftDir;
                    break;
                case State.LeftDir:
                    stairs[i].transform.position = _beforePosition + _leftPosition;
                    break;
                case State.RightDir:
                    stairs[i].transform.position = _beforePosition + _rightPosition;
                    break;
            }
            _beforePosition = stairs[i].transform.position;

            if (i != 0) {
                if (Random.Range(1, 9) < 3 && i < 19) {
                    if (_state == State.LeftDir) _state = State.RightDir;
                    else if (_state == State.RightDir) _state = State.LeftDir;
                    isChangeDir[i + 1] = true;
                }
            }
        }
    }
    
    void SpawnStair(int num) {
        isChangeDir[num + 1 == 20 ? 0 : num + 1] = false;
        _beforePosition = stairs[num == 0 ? 19 : num - 1].transform.position;
        switch (_state) {
            case State.LeftDir:
                stairs[num].transform.position = _beforePosition + _leftPosition;
                break;
            case State.RightDir:
                stairs[num].transform.position = _beforePosition + _rightPosition;
                break;
        }
        
        if (Random.Range(1, 9) < 3) {
            if (_state == State.LeftDir) _state = State.RightDir;
            else if (_state == State.RightDir) _state = State.LeftDir;
            isChangeDir[num + 1 == 20 ? 0 : num + 1] = true;
        }
    }
    
    public void StairMove(int stairIndex, bool isChange, bool isleft) {
        if (player.isDie) return;
        for (int i = 0; i < 20; i++) {
            if (isleft) stairs[i].transform.position += _leftDirection;
            else stairs[i].transform.position += _rightDirection;
        }
        
        for (int i = 0; i < 20; i++)
            if (stairs[i].transform.position.y < -5) SpawnStair(i);
        
        if(isChangeDir[stairIndex] != isChange) {
            GameOver();
            return;
        }
        
        scoreText.text = (++_score).ToString();
        gauge.fillAmount += 0.7f;
        for (int i = 0; i < backGrounds.Length; i++)
        {
            backGrounds[i].transform.position += new Vector3(0, -0.05f, 0);
        }
    }
    
    void GaugeReduce() {
        if (gaugeStart) {
            if (_score > 30) _gaugeReductionRate = 0.0033f;
            if (_score > 60) _gaugeReductionRate = 0.0037f;
            if (_score > 100) _gaugeReductionRate = 0.0043f;
            if (_score > 150) _gaugeReductionRate = 0.005f;
            if (_score > 200) _gaugeReductionRate = 0.005f;
            if (_score > 300) _gaugeReductionRate = 0.0065f;
            if (_score > 400) _gaugeReductionRate = 0.0075f;
            gauge.fillAmount -= _gaugeReductionRate;
        }
        Invoke("GaugeReduce", 0.017f);
    }

    IEnumerator CheckGauge() {
        while (gauge.fillAmount != 0) {
            yield return new WaitForSeconds(0.4f);
        }
        GameOver();
    }

    void GameOver() {
        anim[0].SetBool("GameOver", true);
        player.anim.SetBool("Die", true);
        
        ShowScore();
        pauseBtn.SetActive(false);

        player.isDie = true;
        player.MovingAnimation();
        dslManager.SaveMoney(player.money);

        CancelInvoke();      
        Invoke("DisableUI", 1.5f);
    }
    
    void ShowScore() {
        finalScoreText.text = _score.ToString();
        dslManager.SaveRankScore(_score);
        bestScoreText.text = dslManager.GetBestScore().ToString();
        
        if (_score == dslManager.GetBestScore() && _score != 0)
            ui[2].SetActive(true);
    }

    public void BtnDown(GameObject btn) {
        btn.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        if (btn.name == "ClimbBtn")  player.Climb(false);
        else if (btn.name == "ChangeDirBtn") player.Climb(true);
    }

    public void BtnUp(GameObject btn) {
        btn.transform.localScale = new Vector3(1f, 1f, 1f);
        if (btn.name == "PauseBtn") {
            CancelInvoke();
            isGamePaused = true;
        }
        if (btn.name == "ResumeBtn") {
            GaugeReduce();
            isGamePaused = false;
        }
    }
    
    public void SoundInit() {
        _selectedIndex = dslManager.GetSelectedCharIndex();
        player = players[_selectedIndex].GetComponent<Player>();
        sound[3] = player.sound[0];
        sound[4] = player.sound[1];
        sound[5] = player.sound[2];
    }

    public void SettingBtnInit() {
        bool on;
        for (int i = 0; i < 2; i++) {
            on = dslManager.GetSettingOn("BgmBtn");
            if (on) settingButtons[i].image.color = new Color(1, 1, 1, 1f);
            else settingButtons[i].image.color = new Color(1, 1, 1, 0.5f);
        }

        for (int i = 2; i < 4; i++) {
            on = dslManager.GetSettingOn("SoundBtn");
            if (on) settingButtons[i].image.color = new Color(1, 1, 1, 1f);
            else settingButtons[i].image.color = new Color(1, 1, 1, 0.5f);
        }

        for (int i = 4; i < 6; i++) {
            on = dslManager.GetSettingOn("VibrateBtn");
            if (on) settingButtons[i].image.color = new Color(1, 1, 1, 1f);
            else settingButtons[i].image.color = new Color(1, 1, 1, 0.5f);
        }
    }

    public void SettingBtnChange(Button btn) {
        bool on = dslManager.GetSettingOn(btn.name);
        if (btn.name == "BgmBtn")
            for (int i = 0; i < 2; i++) {
                if (on) settingButtons[i].image.color = new Color(1, 1, 1, 1f);
                else settingButtons[i].image.color = new Color(1, 1, 1, 0.5f);
            }
        if (btn.name == "SoundBtn") {
            for (int i = 2; i < 4; i++) {               
                if (on) settingButtons[i].image.color = new Color(1, 1, 1, 1f);
                else settingButtons[i].image.color = new Color(1, 1, 1, 0.5f);
            }
        }
        if (btn.name == "VibrateBtn") {
            for (int i = 4; i < 6; i++) {
                if (on) settingButtons[i].image.color = new Color(1, 1, 1, 1f);
                else settingButtons[i].image.color = new Color(1, 1, 1, 0.5f);
            }
        }
    }

    public void SettingOnOff(string type) {
        switch (type) {
            case "BgmBtn":
                if (dslManager.GetSettingOn(type)) { dontDestroy.PlayBgm(); }
                else dontDestroy.BgmStop();
                break;
            case "SoundBtn":
                bool isOn = !dslManager.GetSettingOn(type);
                for (int i = 0; i < sound.Length; i++) 
                    sound[i].mute = isOn;
                break;
            case "VibrateBtn":
                vibrationOn = dslManager.GetSettingOn(type);
                break;
        }       
    }

    public void PlaySound(int index) {
        sound[index].Play();
    }

    void DisableUI() {
        ui[0].SetActive(false);
    }

    public void LoadScene(int i) {
        SceneManager.LoadScene(i);
    }

    private void OnApplicationQuit() {
        dslManager.SaveMoney(player.money);
    }
}
