using TMPro;
using UnityEngine;

public class ScoreView : MonoBehaviour
{
    [SerializeField] private TMP_Text _score;

    [Subscriber(Keys.Score)]
    private void OnChangeScore(string score)
    {
        _score.text = score;
    }
}