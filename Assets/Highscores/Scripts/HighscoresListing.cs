using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighscoresListing : MonoBehaviour {
    [SerializeField]
    private TextMeshProUGUI username;

    [SerializeField]
    private TextMeshProUGUI ranking;

    [SerializeField]
    private TextMeshProUGUI time;

    public void Init(Transform parent, string username, string ranking, string time) {
        this.transform.SetParent(parent);
        this.username.SetText(username);
        this.ranking.SetText(ranking);
        this.time.SetText(time);
    }
}
