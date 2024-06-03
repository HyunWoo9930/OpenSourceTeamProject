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
    public int Price;
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
    List<Character> Characters = new List<Character>();
    List<Rankings> Rankings = new List<Rankings>();
    List<Information> Informs = new List<Information>();

    public GameManager gameManager;
    public CharacterManager CharacterManager;
    public Text[] rankingText;
    public Sprite[] characterSprite;
    public Image[] rankCharacterImg;

    private void Awake() {
        if (!File.Exists(Application.persistentDataPath + "/Characters.json")) {
            Characters.Add(new Character("Boo", "Boo", 0, true, true));
            Characters.Add(new Character("Rapper", "래퍼", 500, false, false));
            Characters.Add(new Character("Secretary", "비서", 500, false, false));
            Characters.Add(new Character("Boxer", "복서", 1000, false, false));
            Characters.Add(new Character("CheerLeader", "치어리더", 1000, false, false));
            Characters.Add(new Character("Sheriff", "보안관", 2000, false, false));
            Characters.Add(new Character("Plumber", "배관공", 2000, false, false));

            Rankings.Add(new Rankings(0, 7));
            Rankings.Add(new Rankings(0, 7));
            Rankings.Add(new Rankings(0, 7));
            Rankings.Add(new Rankings(0, 7));

            Informs.Add(new Information(0, true, true, true, false));

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
        File.WriteAllText(Application.persistentDataPath + "/Characters.json", Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(Characters))));
        File.WriteAllText(Application.persistentDataPath + "/Rankings.json", Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(Rankings))));
        File.WriteAllText(Application.persistentDataPath + "/Informs.json", Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(Informs))));
    }


    private void LoadData() {
        Characters = JsonConvert.DeserializeObject<List<Character>>(System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(File.ReadAllText(Application.persistentDataPath + "/Characters.json"))));
        Rankings = JsonConvert.DeserializeObject<List<Rankings>>(System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(File.ReadAllText(Application.persistentDataPath + "/Rankings.json"))));
        Informs = JsonConvert.DeserializeObject<List<Information>>(System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(File.ReadAllText(Application.persistentDataPath + "/Informs.json"))));
    }
    
    public void SaveCharacterIndex() {
        for (int i = 0; i < Characters.Count; i++)
            Characters[i].Selected = false;
        Characters[CharacterManager.index].Selected = true;
        SaveData();
        SceneManager.LoadScene(0);
    }

    public int GetSelectedCharIndex() {
        LoadData();
        for (int i = 0; i < Characters.Count; i++)
            if (Characters[i].Selected) return i;
        return 0;
    }




    //#.Purchase Character
    public bool IsPurchased(int index) {
        LoadData();
        return Characters[index].Purchased;
    }

    public void SaveCharacterPurchased(Animator obj) {
        if (Characters[CharacterManager.index].Price > Informs[0].Money)
            obj.GetComponent<Animator>().SetTrigger("notice");
        else {
            Characters[CharacterManager.index].Purchased = true;
            SaveData();
            LoadData();
            Informs[0].Money -= Characters[CharacterManager.index].Price;
            SaveData();
            CharacterManager.ArrowBtn("null");
        }
    }

    public int GetPrice() { return Characters[CharacterManager.index].Price; }
    
    public int GetMoney() {
        LoadData();
        return Informs[0].Money;
    }

    public void SaveMoney(int money) {
        LoadData();
        Informs[0].Money = money;
        SaveData();
    }

    public bool IsRetry() { return Informs[0].Retry; }

    public void ChangeRetry(bool isRetry) {
        LoadData();
        Informs[0].Retry = isRetry;
        SaveData();
    }




    //#.Ranking
    public void LoadRanking() {
        for (int i = 0; i < rankingText.Length; i++) {
            rankingText[i].text = Rankings[i].Score == 0 ? " " : Rankings[i].Score.ToString();
            rankCharacterImg[i].sprite = characterSprite[Rankings[i].CharacterIndex];
        }
    }

    public void SaveRankScore(int finalScore) {
        Rankings[3].Score = finalScore;
        SaveData();
        
        int charIndex = GetSelectedCharIndex();
        Rankings[3].CharacterIndex = charIndex;
        
        Rankings.Sort(delegate (Rankings a, Rankings b) { return b.Score.CompareTo(a.Score); });

        SaveData();
        LoadData();
    }

    public int GetBestScore() {
        LoadData();
        return Rankings[0].Score;
    }
    
    public bool GetSettingOn(string type) {
        LoadData();
        switch (type) {
            case "BgmBtn":
                return Informs[0].BgmOn;
            case "SoundBtn":
                return Informs[0].SoundEffectOn;
            case "VibrateBtn":
                return Informs[0].VibrationOn;
        }
        return false;
    }

    public void ChangeOnOff(Button btn) {
        LoadData();
        if (btn.name == "BgmBtn") {
            Informs[0].BgmOn = !Informs[0].BgmOn;
        }
        if (btn.name == "SoundBtn") {
            Informs[0].SoundEffectOn = !Informs[0].SoundEffectOn;
        }
        if (btn.name == "VibrateBtn") {
            Informs[0].VibrationOn = !Informs[0].VibrationOn;
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
