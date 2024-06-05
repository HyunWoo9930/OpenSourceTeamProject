using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Character {
    public string EName, KName;
    public readonly int Price;
    public bool Selected, Purchased;

    public Character(string eName, string kName, int price, bool selected, bool purchased) {
        EName = eName;
        KName = kName;
        Price = price;
        Selected = selected;
        Purchased = purchased;
    }
}

public class Rankings {
    public int Score, CharacterIndex;

    public Rankings(int score, int characterIndex) {
        Score = score;
        CharacterIndex = characterIndex;
    }
}

public class Information {
    public int Money;
    public bool BgmOn, SoundEffectOn, VibrationOn, Retry;

    public Information(int money, bool bgmOn, bool soundEffectOn, bool vibrationOn, bool retry) {
        Money = money;
        BgmOn = bgmOn;
        SoundEffectOn = soundEffectOn;
        VibrationOn = vibrationOn;
        Retry = retry;
    }
}


public class DSLManager : MonoBehaviour {
    private List<Character> _characters = new List<Character>();
    private List<Rankings> _rankings = new List<Rankings>();
    private List<Information> _informs = new List<Information>();

    public GameManager gameManager;
    public CharacterManager characterManager;
    public Text[] rankingText;
    public Sprite[] characterSprite;
    public Image[] rankCharacterImg;

    private void Awake() {
        if (!File.Exists(Application.persistentDataPath + "/Characters.json")) {
            _characters.Add(new Character("Boo", "Boo", 0, true, true));
            _characters.Add(new Character("Rapper", "래퍼", 500, false, false));
            _characters.Add(new Character("Secretary", "비서", 500, false, false));
            _characters.Add(new Character("Boxer", "복서", 1000, false, false));
            _characters.Add(new Character("CheerLeader", "치어리더", 1000, false, false));
            _characters.Add(new Character("Sheriff", "보안관", 2000, false, false));
            _characters.Add(new Character("Plumber", "배관공", 2000, false, false));

            _rankings.Add(new Rankings(0, 7));
            _rankings.Add(new Rankings(0, 7));
            _rankings.Add(new Rankings(0, 7));
            _rankings.Add(new Rankings(0, 7));

            _informs.Add(new Information(0, true, true, true, false));

            SaveData();
        }

        LoadData();
        LoadRanking();
        gameManager.SettingBtnInit();
        gameManager.SoundInit();
        gameManager.SettingOnOff("BgmBtn");
        gameManager.SettingOnOff("SoundBtn");
        gameManager.SettingOnOff("VibrateBtn");
    }
    
    public void SaveData() {
        File.WriteAllText(Application.persistentDataPath + "/Characters.json", Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(_characters))));
        File.WriteAllText(Application.persistentDataPath + "/Rankings.json", Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(_rankings))));
        File.WriteAllText(Application.persistentDataPath + "/Informs.json", Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(_informs))));
    }


    private void LoadData() {
        _characters = JsonConvert.DeserializeObject<List<Character>>(System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(File.ReadAllText(Application.persistentDataPath + "/Characters.json"))));
        _rankings = JsonConvert.DeserializeObject<List<Rankings>>(System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(File.ReadAllText(Application.persistentDataPath + "/Rankings.json"))));
        _informs = JsonConvert.DeserializeObject<List<Information>>(System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(File.ReadAllText(Application.persistentDataPath + "/Informs.json"))));
    }
    
    public void SaveCharacterIndex() {
        for (int i = 0; i < _characters.Count; i++)
            _characters[i].Selected = false;
        _characters[characterManager.index].Selected = true;
        SaveData();
        SceneManager.LoadScene(0);
    }

    public int GetSelectedCharIndex() {
        LoadData();
        for (int i = 0; i < _characters.Count; i++)
            if (_characters[i].Selected) return i;
        return 0;
    }

    public bool IsPurchased(int index) {
        LoadData();
        return _characters[index].Purchased;
    }

    public void SaveCharacterPurchased(Animator obj) {
        if (_characters[characterManager.index].Price > _informs[0].Money)
            obj.GetComponent<Animator>().SetTrigger("notice");
        else {
            _characters[characterManager.index].Purchased = true;
            SaveData();
            LoadData();
            _informs[0].Money -= _characters[characterManager.index].Price;
            SaveData();
            characterManager.ArrowBtn("null");
        }
    }

    public int GetPrice() { return _characters[characterManager.index].Price; }
    
    public int GetMoney() {
        LoadData();
        return _informs[0].Money;
    }

    public void SaveMoney(int money) {
        LoadData();
        _informs[0].Money = money;
        SaveData();
    }

    public bool IsRetry() { return _informs[0].Retry; }

    public void ChangeRetry(bool isRetry) {
        LoadData();
        _informs[0].Retry = isRetry;
        SaveData();
    }
    
    private void LoadRanking() {
        for (int i = 0; i < rankingText.Length; i++) {
            rankingText[i].text = _rankings[i].Score == 0 ? " " : _rankings[i].Score.ToString();
            rankCharacterImg[i].sprite = characterSprite[_rankings[i].CharacterIndex];
        }
    }

    public void SaveRankScore(int finalScore) {
        _rankings[3].Score = finalScore;
        SaveData();
        
        int charIndex = GetSelectedCharIndex();
        _rankings[3].CharacterIndex = charIndex;
        
        _rankings.Sort(delegate (Rankings a, Rankings b) { return b.Score.CompareTo(a.Score); });

        SaveData();
        LoadData();
    }

    public int GetBestScore() {
        LoadData();
        return _rankings[0].Score;
    }
    
    public bool GetSettingOn(string type) {
        LoadData();
        switch (type) {
            case "BgmBtn":
                return _informs[0].BgmOn;
            case "SoundBtn":
                return _informs[0].SoundEffectOn;
            case "VibrateBtn":
                return _informs[0].VibrationOn;
        }
        return false;
    }

    public void ChangeOnOff(Button btn) {
        LoadData();
        if (btn.name == "BgmBtn") {
            _informs[0].BgmOn = !_informs[0].BgmOn;
        }
        if (btn.name == "SoundBtn") {
            _informs[0].SoundEffectOn = !_informs[0].SoundEffectOn;
        }
        if (btn.name == "VibrateBtn") {
            _informs[0].VibrationOn = !_informs[0].VibrationOn;
        }
        SaveData();
        gameManager.SettingOnOff(btn.name);
        gameManager.SettingBtnChange(btn);
    }
    
    private void OnApplicationQuit() {
        ChangeRetry(false);
    }
    
    private void OnApplicationPause() {
        if (gameManager.isGamePaused) return;
        ChangeRetry(false);
    }

}
