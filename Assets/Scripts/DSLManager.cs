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
    public Text[] moneyText, rankingText;
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

            DataSave();
        }

        DataLoad();
        LoadRanking();
        gameManager.SettingBtnInit();
        gameManager.SoundInit();
        gameManager.SettingOnOff("BgmBtn");
        gameManager.SettingOnOff("SoundBtn");
        gameManager.SettingOnOff("VibrateBtn");
    }
    
    public void DataSave() {
        string jdata_0 = JsonConvert.SerializeObject(Characters);
        string jdata_1 = JsonConvert.SerializeObject(Rankings);
        string jdata_2 = JsonConvert.SerializeObject(Informs);
       
        byte[] bytes_0 = System.Text.Encoding.UTF8.GetBytes(jdata_0);
        byte[] bytes_1 = System.Text.Encoding.UTF8.GetBytes(jdata_1);
        byte[] bytes_2 = System.Text.Encoding.UTF8.GetBytes(jdata_2);

        string format_0 = System.Convert.ToBase64String(bytes_0);
        string format_1 = System.Convert.ToBase64String(bytes_1);
        string format_2 = System.Convert.ToBase64String(bytes_2);       

        File.WriteAllText(Application.persistentDataPath + "/Characters.json", format_0);
        File.WriteAllText(Application.persistentDataPath + "/Rankings.json", format_1);
        File.WriteAllText(Application.persistentDataPath + "/Informs.json", format_2);
    }


    public void DataLoad() {
        string jdata_0 = File.ReadAllText(Application.persistentDataPath + "/Characters.json");
        string jdata_1 = File.ReadAllText(Application.persistentDataPath + "/Rankings.json");
        string jdata_2 = File.ReadAllText(Application.persistentDataPath + "/Informs.json");
      
        byte[] bytes_0 = System.Convert.FromBase64String(jdata_0);
        byte[] bytes_1 = System.Convert.FromBase64String(jdata_1);
        byte[] bytes_2 = System.Convert.FromBase64String(jdata_2);

        string reformat_0 = System.Text.Encoding.UTF8.GetString(bytes_0);
        string reformat_1 = System.Text.Encoding.UTF8.GetString(bytes_1);
        string reformat_2 = System.Text.Encoding.UTF8.GetString(bytes_2);
        
        Characters = JsonConvert.DeserializeObject<List<Character>>(reformat_0);
        Rankings = JsonConvert.DeserializeObject<List<Rankings>>(reformat_1);
        Informs = JsonConvert.DeserializeObject<List<Information>>(reformat_2);
    }
    
    public void SaveCharacterIndex() {
        for (int i = 0; i < Characters.Count; i++)
            Characters[i].Selected = false;
        Characters[CharacterManager.index].Selected = true;
        DataSave();
        SceneManager.LoadScene(0);
    }

    public int GetSelectedCharIndex() {
        DataLoad();
        for (int i = 0; i < Characters.Count; i++)
            if (Characters[i].Selected) return i;
        return 0;
    }




    //#.Purchase Character
    public bool IsPurchased(int index) {
        DataLoad();
        return Characters[index].Purchased;
    }

    public void SaveCharacterPurchased(Animator obj) {
        if (Characters[CharacterManager.index].Price > Informs[0].Money)
            obj.GetComponent<Animator>().SetTrigger("notice");
        else {
            Characters[CharacterManager.index].Purchased = true;
            DataSave();
            DataLoad();
            Informs[0].Money -= Characters[CharacterManager.index].Price;
            DataSave();
            CharacterManager.ArrowBtn("null");
        }
    }

    public int GetPrice() { return Characters[CharacterManager.index].Price; }
    
    public int GetMoney() {
        DataLoad();
        return Informs[0].Money;
    }

    public void SaveMoney(int money) {
        DataLoad();
        Informs[0].Money = money;
        DataSave();
    }

    public bool IsRetry() { return Informs[0].Retry; }

    public void ChangeRetry(bool isRetry) {
        DataLoad();
        Informs[0].Retry = isRetry;
        DataSave();
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
        DataSave();
        
        int charIndex = GetSelectedCharIndex();
        Rankings[3].CharacterIndex = charIndex;
        
        Rankings.Sort(delegate (Rankings a, Rankings b) { return b.Score.CompareTo(a.Score); });

        DataSave();
        DataLoad();
    }

    public int GetBestScore() {
        DataLoad();
        return Rankings[0].Score;
    }
    
    public bool GetSettingOn(string type) {
        DataLoad();
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
        DataLoad();
        if (btn.name == "BgmBtn") {
            Informs[0].BgmOn = !Informs[0].BgmOn;
        }
        if (btn.name == "SoundBtn") {
            Informs[0].SoundEffectOn = !Informs[0].SoundEffectOn;
        }
        if (btn.name == "VibrateBtn") {
            Informs[0].VibrationOn = !Informs[0].VibrationOn;
        }
        DataSave();
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
