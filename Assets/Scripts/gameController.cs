using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class gameController : MonoBehaviour
{
    [SerializeField] textManager _textManager;
    [SerializeField] GameObject _coin;
    private int _points = -1;
    private float _timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        update_text();
    }

    private void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            spawn_coin();
            _timer = Random.Range(0.1f, 2);
        }
    }

    private void spawn_coin()
    {
        GameObject spawned_coin = Instantiate(_coin);
        spawned_coin.transform.position = new Vector3(13, 14, 0);
    }

    // Update is called once per frame
    public void update_text()
    {
        _points+=1;
        _textManager.update_text(_points);
    }
}
