using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class CharacterManager : MonoBehaviour
{
    public int index;
    private readonly string[] _characterNames = { "Boo", "래퍼", "비서", "복서", "치어리더", "보안관", "배관공" };
    public DSLManager dslManager;
    public GameObject selectBtn, purchaseBtn;
    private AudioSource _sound;
    public Image characterImage;
    public Text characterName, price;

    private void Awake() {
        index = dslManager.GetSelectedCharIndex();
        _sound = GetComponent<AudioSource>();
        _sound.mute = !dslManager.GetSettingOn("SoundBtn");
        ArrowBtn("null");
    }

    
    public void ArrowBtn(string dir)
    {
        switch (dir)
        {
            case "Right":
            {
                index++;
                if (index == dslManager.characterSprite.Length - 1) index = 0;
                break;
            }
            case "Left":
            {
                if (index == -1)
                {
                    index = dslManager.characterSprite.Length - 2;
                }
                break;
            }
        }

        characterImage.sprite = dslManager.characterSprite[index];
        characterName.text = _characterNames[index];
        price.text = dslManager.GetPrice() + " 원";
        
        selectBtn.SetActive(dslManager.IsPurchased(index));
        purchaseBtn.SetActive(!dslManager.IsPurchased(index));
    }
}
