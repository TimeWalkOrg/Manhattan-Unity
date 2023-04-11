using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeSliderSyncTest : MonoBehaviour
{
    [SerializeField]
    private int _currentYear    = 1620;
    private int _previousYear = 1620;

    private TimeSliderSync _timeSliderSync;
    private GameObject TimeSliderParent;
    private Slider TimeSlider;

    private void Awake() {
        //get a reference to the timeslider year value
        _timeSliderSync = GetComponent<TimeSliderSync>();
        TimeSliderParent = GameObject.FindGameObjectWithTag("TimeSlider");
        TimeSlider = TimeSliderParent.GetComponent<Slider>();
    }

    private void Start() {
        //TimeSliderParent.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(TimeSlider != null) {
            _currentYear = (int)TimeSlider.value;
        }
        if(_currentYear != _previousYear) {
            _timeSliderSync.SetTimeSliderYear(_currentYear);
            _previousYear = _currentYear;
        }
    }
}
