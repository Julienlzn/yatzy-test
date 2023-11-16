using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class Dice : MonoBehaviour
{
    [SerializeField] private List<Vector2> diceRotationAngle = new List<Vector2>();
    [SerializeField] private ParticleSystem _particleSystem;

    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }

    public void Roll()
    {
        int random = Random.Range(0, 6);
        Vector3 targetAngle = new Vector3(diceRotationAngle[random].x, diceRotationAngle[random].y, 0);

        var seq = DOTween.Sequence();
        seq.Append(transform.DOShakeRotation(1f, 1000, 10, 180, true, ShakeRandomnessMode.Harmonic));
        seq.Insert(0.8f, transform.DORotateQuaternion(Quaternion.Euler(targetAngle), 0.25f));

        seq.OnComplete(delegate
        {
            if (random + 1 == 6)
            {
                DOVirtual.DelayedCall(0.25f, delegate
                {
                    transform.DOScale(1.5f, 0.25f).SetEase(Ease.InBack);
                    transform.DORotate(new Vector3(0, 0, -360), 0.4f, RotateMode.WorldAxisAdd).SetDelay(0.15f);
                    transform.DOScale(1, 0.4f).SetDelay(0.4f).SetEase(Ease.OutBounce);
                    DOVirtual.DelayedCall(0.6f, delegate
                    {
                        GameManager.OnSixRolled?.Invoke();
                        _particleSystem.Play();
                        _gameManager.ResetButton();
                    });
                });
            }
            else
            {
                _gameManager.ResetButton();
            }
        });
    }
}