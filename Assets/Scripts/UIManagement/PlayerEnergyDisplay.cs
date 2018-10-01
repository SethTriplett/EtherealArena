using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerEnergyDisplay : MonoBehaviour, IEventListener {

    private Slider energySlider;
    // Use later for coloring bar
    // private Image sliderFill;

    private int maxEnergy = 100;

    void OnEnable() {
        EventMessanger.GetInstance().SubscribeEvent(typeof(PlayerCurrentEnergyEvent), this);
    }

    void OnDisable() {
        EventMessanger.GetInstance().UnsubscribeEvent(typeof(PlayerCurrentEnergyEvent), this);
    }

    void Start() {
        energySlider = GetComponent<Slider>();
        // sliderFill = transform.Find("Fill Area").Find("Fill").GetComponent<Image>();

        energySlider.maxValue = maxEnergy;
    }

    void SetEnergy(float energy) {
        energySlider.value = energy;
    }

    public void ConsumeEvent(IEvent e) {
        if (e.GetType() == typeof(PlayerCurrentEnergyEvent)) {
            PlayerCurrentEnergyEvent currentEnergyEvent = e as PlayerCurrentEnergyEvent;
            SetEnergy(currentEnergyEvent.energy);
        }
    }

}
