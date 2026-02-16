using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private GameObject _player;
    [SerializeField] private float _maximum_height;
    [SerializeField] private TextMeshProUGUI _depth_text;
    private float seconds_of_healing=0;

    // Start is called before the first frame update
    void Start()
    {
        losehealth(50);
        gainhealth(30);
    }

    // Update is called once per frame
    void Update()
    {
        _depth_text.text = "Depth: " + Mathf.Round(1000 * (1- (_player.transform.position.y / _maximum_height))).ToString() + " M";

        if (_lazybar.value > _healthbar.value)
        {
            _lazybar.value -= _sliderspeed * Time.deltaTime;
        }

        if (seconds_of_healing > 0)
        {
            seconds_of_healing -= Time.deltaTime;
            _healthbar.value += Time.deltaTime * _healingrate;
        }

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

}
