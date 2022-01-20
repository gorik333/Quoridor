using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndGame : MonoBehaviour
{
    public static EndGame Instance { get; private set; }

    [SerializeField]
    private GameObject[] _toTurnOff;

    [SerializeField]
    private GameObject _panel;

    [SerializeField]
    private TextMeshProUGUI _text;


    private void Awake()
    {
        Instance = this;
    }


    public void OnWin(string color)
    {
        for (int i = 0; i < _toTurnOff.Length; i++)
        {
            _toTurnOff[i].SetActive(false);
        }

        _text.text = "WIN --> " + color;

        _panel.SetActive(true);
    }
}
