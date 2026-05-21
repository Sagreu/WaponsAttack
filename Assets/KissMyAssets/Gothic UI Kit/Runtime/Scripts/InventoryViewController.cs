using UnityEngine;

namespace KissMyAssets.Modules.GothicUI
{
    public class InventoryViewController : MonoBehaviour
    {
        public static InventoryViewController Instance;

        [SerializeField] private InventorySlotView _backpackSlot;
        [SerializeField] private InventorySlotView _hotbarSlot;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        public void SetSlot(InventorySlotView slot, ESlotType eSlotType)
        {
            if (eSlotType == ESlotType.Backpack)
            {
                _backpackSlot.SetMarker(false);
                _backpackSlot = slot;
                _backpackSlot.SetMarker(true);
            }
            else
            {
                _hotbarSlot.SetMarker(false);
                _hotbarSlot = slot;
                _hotbarSlot.SetMarker(true);
            }
        }

        public void ExchangeSlotItems()
        {
            Sprite image = _hotbarSlot.GetImage();
            _hotbarSlot.SetImage(_backpackSlot.GetImage());
            _backpackSlot.SetImage(image);
        }
    }
    public enum ESlotType
    {
        Backpack = 0,
        HotBar = 1,
    }
}