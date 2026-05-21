using KissMyAssets.Modules.GothicUI;
using UnityEngine;
using UnityEngine.UI;

namespace KissMyAssets.Modules.GothicUI
{
    public class InventorySlotView : MonoBehaviour
    {
        [SerializeField] private Image _inventoryItemImage;
        [SerializeField] private Image _inventorySlotmarkerImage;
        [SerializeField] private Button _inventorySlotButton;
        [SerializeField] private ESlotType eSlotType;

        private void Start()
        {
            _inventorySlotButton.onClick.AddListener(() =>
            {
                InventoryViewController.Instance.SetSlot(this, eSlotType);
            });
        }
        public void SetImage(Sprite imageToSet)
        {
            if (imageToSet == null)
                return;

            _inventoryItemImage.gameObject.SetActive(true);
            _inventoryItemImage.sprite = imageToSet;
        }
        public Sprite GetImage()
        {
            if (_inventoryItemImage.sprite == null)
                return default;

            Sprite image = _inventoryItemImage.sprite;
            _inventoryItemImage.sprite = null;
            _inventoryItemImage.gameObject.SetActive(false);
            return image;
        }
        public void SetMarker(bool isActive)
        {
            _inventorySlotmarkerImage.gameObject.SetActive(isActive);
        }
    }
}