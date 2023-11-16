using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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

        OnSixRolled += OnSixRollEndCallback;

        _rollButton.onClick.AddListener(delegate
        {
            _dice.Roll();
            DOTween.Kill("rollButtonCta");
            _rollButton.transform.rotation = Quaternion.Euler(0,0,0);
            _rollButton.transform.DOScale(Vector3.one * 0.8f, 0.1f).SetEase(Ease.Flash);
            _rollButton.interactable = false;
        });

        ButtonShakeAnimation();
    }

    private void ButtonShakeAnimation()
    {
        _rollButton.transform.DOShakeRotation(0.6f, new Vector3(0, 0, 5f), 50, 45, true, ShakeRandomnessMode.Harmonic)
            .SetId("rollButtonCta").OnComplete(
                delegate { DOVirtual.DelayedCall(Random.Range(4f, 6f), ButtonShakeAnimation).SetId("rollButtonCta"); });
    }

    private void OnSixRollEndCallback()
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
        DOVirtual.DelayedCall(Random.Range(4f, 6f), ButtonShakeAnimation).SetId("rollButtonCta");
    }
}