using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using UnityEngine.UI;

public class TimeSliderSync : RealtimeComponent<TimeSliderSyncModel> {
    private Slider _timeSlider;

    private void Awake() {
        //get reference to the slider
        _timeSlider = GameObject.FindGameObjectWithTag("TimeSlider").GetComponent<Slider>();
    }

    private void UpdateYear() {
        //get the year from the model and set it on the slider UI element
        _timeSlider.value = model.timeSliderYear;
    }

    protected override void OnRealtimeModelReplaced(TimeSliderSyncModel previousModel, TimeSliderSyncModel currentModel) {
        if(previousModel != null) {
            //unregister from events
            previousModel.timeSliderYearDidChange -= TimeSliderYearDidChange;
        }

        if (currentModel != null) {
            //if this is a model that has no data set on it, populate it with the current time slider year
            if (currentModel.isFreshModel) {
                currentModel.timeSliderYear = (int)_timeSlider.value;
            }

            //update the year to match the new model
            UpdateTimeSliderYear();

            //register for events so we'll know if the year changes later
            currentModel.timeSliderYearDidChange += TimeSliderYearDidChange;
        }
    }

    private void TimeSliderYearDidChange(TimeSliderSyncModel model, int value) {
        //Update the time slider year
        UpdateTimeSliderYear();
    }

    private void UpdateTimeSliderYear() {
        _timeSlider.value = model.timeSliderYear;
    }

    public void SetTimeSliderYear(int year) {
        //set the year on the slider
        //This will fire the TimeSliderYearChanged even on the model, which will update the renderer for
        //both the local player and all remote players.
        //print(model);
        model.timeSliderYear = year;
    }
}
