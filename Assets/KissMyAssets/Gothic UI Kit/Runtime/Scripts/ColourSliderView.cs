using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KissMyAssets.Modules.GothicUI
{
    public class ColourSliderView : MonoBehaviour
    {
        [Header("Slider")]
        [SerializeField] private Slider _slider;
        [SerializeField] private Image _sliderHandle;
        [SerializeField] private Image _sliderBackground;
        [SerializeField] private TextMeshProUGUI _onText;
        [SerializeField] private TextMeshProUGUI _offText;

        [Header("ONGraphic")]
        [SerializeField] private Sprite _onHandle;
        [SerializeField] private Sprite _onBackground;

        [Header("OFFGraphic")]
        [SerializeField] private Sprite _offHandle;
        [SerializeField] private Sprite _offBackground;

        private void Awake()
        {
            _slider.onValueChanged.AddListener((value) =>
            {
                bool isOn = value > 0;

                _sliderHandle.sprite = isOn ? _onHandle : _offHandle;
                _sliderBackground.sprite = isOn ? _onBackground : _offBackground;
                _onText.gameObject.SetActive(isOn);
                _offText.gameObject.SetActive(!isOn);
            });
        }
    }
}