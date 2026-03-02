using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    [SerializeField] private Slider _healthbar;
    [SerializeField] private Slider _lazybar;
    [SerializeField] private float _sliderspeed;
    [SerializeField] private float _healingrate;
    [SerializeField] private float _maximum_height;
    [SerializeField] private TextMeshProUGUI _depth_text;
    [SerializeField] private TextMeshProUGUI _questStatusText;
    [SerializeField] private Image _right_hand;
    [SerializeField] private Image _left_hand;
    [SerializeField] private Sprite _left_hand_unselected;
    [SerializeField] private Sprite _left_hand_selected;
    [SerializeField] private Sprite _right_hand_unselected;
    [SerializeField] private Sprite _right_hand_selected;

    public string _currentQuestStatus;
    private float seconds_of_healing=0;

    // Start is called before the first frame update
    void Start()
    {
        unselect_left_hand();
        select_right_hand();
        _currentQuestStatus = "None";
    }

    // Update is called once per frame
    void Update()
    {
        _depth_text.text = "Depth: " + Mathf.Round(1000 * (1- (GameController.Instance.Player.transform.position.y / _maximum_height))).ToString() + " M";

        if (_lazybar.value > _healthbar.value)
        {
            _lazybar.value -= _sliderspeed * Time.deltaTime;
        }

        if (seconds_of_healing > 0)
        {
            seconds_of_healing -= Time.deltaTime;
            _healthbar.value += Time.deltaTime * _healingrate;
        }

        _questStatusText.text = "Quest Status: " + _currentQuestStatus;

    }

    public void losehealth(float damage)
    {
        _lazybar.value = _healthbar.value;
        _healthbar.value -= damage;
    }

    public void gainhealth(float healing_seconds)
    {
        seconds_of_healing += healing_seconds;
    }

    public void select_right_hand()
    {
        _right_hand.sprite = _right_hand_selected;
    }
    public void select_left_hand()
    {
        _left_hand.sprite = _left_hand_selected;
    }
    public void unselect_right_hand()
    {
        _right_hand.sprite = _right_hand_unselected;
    }

    public void unselect_left_hand()
    {
        _left_hand.sprite = _left_hand_unselected;
    }
}
