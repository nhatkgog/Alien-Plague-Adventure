﻿using Assets.Scripts.Save_and_Load;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{

    public PlayerDatabase PlayerDB;

    public TMP_Text namePlayer;
    public SpriteRenderer artworkSprite;

    private int selectOption = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!PlayerPrefs.HasKey("selectOption"))
        {
            selectOption = 0;
        }
        else
        {
            Load();
        }

        updatePlayer(selectOption);
    }

    // Update is called once per frame
    void Update()
    {
        updatePlayer(selectOption);
    }

    public void nextOption()
    {
        selectOption++;

        if (selectOption >= PlayerDB.PlayerCount)
        {
            selectOption = 0;
        }

        updatePlayer(selectOption);
        Save();
    }

    public void backOption()
    {
        selectOption--;

        if (selectOption < 0)
        {
            selectOption = PlayerDB.PlayerCount - 1;
        }

        updatePlayer(selectOption);
        Save();
    }

    private void updatePlayer(int selectOption)
    {
        PlayerStatus status = PlayerDB.GetPlayer(selectOption);
        artworkSprite.sprite = status.characterIcon;
        namePlayer.text = status.name;

    }
    private void Load()
    {
        selectOption = PlayerPrefs.GetInt("selectOption");
    }
    private void Save()
    {
        PlayerPrefs.SetInt("selectOption", selectOption);
    }

    public void ConfirmSelection()
    {
        PlayerStatus selected = PlayerDB.GetPlayer(selectOption);
        PlayerSelector.Instance.SetSelectedPlayer(selected);
    }


    public void changeScene()
    {
        PlayerStatus selected = PlayerDB.GetPlayer(selectOption);
        PlayerSelector.Instance.SetSelectedPlayer(selected);
        SceneManager.LoadScene("Scence1"); 
    }

}
