using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class textManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    public void update_text(int score)
    {
        _text.text = "Points: " + score.ToString();

    }
}
