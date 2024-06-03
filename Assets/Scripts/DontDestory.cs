using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestory : MonoBehaviour
{ 
    private static DontDestory _instance;
    private AudioSource _bgm;
    
    private void Awake() {
        if (_instance != null) {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void BgmStop() {
        if (_instance == null)
            _bgm = gameObject.GetComponent<AudioSource>();
        else
            _bgm = _instance.GetComponent<AudioSource>();
        _bgm.enabled = false;
    }

    public void PlayBgm() {
        if (_instance == null)
            _bgm = gameObject.GetComponent<AudioSource>();
        else
            _bgm = _instance.GetComponent<AudioSource>();
        _bgm.enabled = true;
    }
}


