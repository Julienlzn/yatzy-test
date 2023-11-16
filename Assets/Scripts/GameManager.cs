using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static Action OnSixRolled;
    private const string _scoreSaveKey = "score_save_key";


    [SerializeField] private Button _rollButton;
    [SerializeField] private TextMeshProUGUI _scoreTxt;
    [SerializeField] private Dice _dice;

    private int _score;

    private void Start()
    {
        InitScore();

        OnSixRolled += OnRollEndCallback;
        _rollButton.onClick.AddListener(_dice.Roll);
        _rollButton.onClick.AddListener(() =>_rollButton.transform.DOScale(Vector3.one * 0.8f, 0.1f).SetEase(Ease.Flash));
        _rollButton.onClick.AddListener(() =>_rollButton.interactable = false);
    }

    private void OnRollEndCallback()
    {
        IncrementScore();
    }

    private void InitScore()
    {
        _score = PlayerPrefs.GetInt(_scoreSaveKey, 0);
        _scoreTxt.SetText(_score.ToString());
    }

    private void IncrementScore()
    {
        _score++;
        PlayerPrefs.SetInt(_scoreSaveKey, _score);

        _scoreTxt.transform.DOPunchScale(Vector3.one * 0.5f, 0.25f).SetEase(Ease.OutBack);
        _scoreTxt.SetText(_score.ToString());
    }

    public void ResetButton()
    {
        _rollButton.transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack);
        _rollButton.interactable = true;
    }
}