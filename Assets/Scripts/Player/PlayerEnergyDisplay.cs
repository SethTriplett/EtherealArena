using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerEnergyDisplay : MonoBehaviour {

    private Slider energySlider;
    // private Image sliderFill;

    private int maxEnergy = 100;

    void Start() {
        energySlider = GetComponent<Slider>();
        // sliderFill = transform.Find("Fill Area").Find("Fill").GetComponent<Image>();

        energySlider.maxValue = maxEnergy;
    }

    public void SetEnergy(float energy) {
        energySlider.value = energy;
    }

}
