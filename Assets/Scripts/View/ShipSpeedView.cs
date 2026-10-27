using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShipSpeedView : MonoBehaviour
{
    [SerializeField] private TMP_Text _speed;
    [SerializeField] private Image _image;

    [Subscriber(Keys.Speed)]
    private void OnChangeShipSpeed(string speed)
    {
        _speed.text = speed;
    }

    [Subscriber(Keys.NormalizedSpeed)]
    private void OnChangeNormalizedSpeed(float normalizedSpeed)
    {
        _image.fillAmount = normalizedSpeed;
    }
}