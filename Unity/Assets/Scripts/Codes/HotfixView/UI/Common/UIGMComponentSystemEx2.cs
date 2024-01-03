using ET;

namespace ET.Client
{
    public partial class UIGMComponentSystem
    {
        public static void OnValueChanged(this UIGMComponent self, XInputField inputField, int index)
        {
            var cfg = GMConfigCategory.Instance.Get(self.SelectedId);
            switch (cfg.Code)
            {
                case GMCode.AddRobot:
                    break;
                case GMCode.GetItem:
                {
                    switch (index)
                    {
                        case 1:
                        {
                            self.ShowItemsList(inputField.text);
                            break;
                        }
                    }
                    break;
                }
            }
        }
        
        public static void OnInputSelected(this UIGMComponent self, XInputField inputField, int index)
        {
            var cfg = GMConfigCategory.Instance.Get(self.SelectedId);
            switch (cfg.Code)
            {
                case GMCode.AddRobot:
                    break;
                case GMCode.GetItem:
                {
                    switch (index)
                    {
                        case 1:
                        {
                            self.popList.gameObject.Display(true);   
                            self.ShowItemsList(inputField.text);
                            break;
                        }
                    }
                    break;
                }
            }
        }

        public static void OnSelectPopListItem(this UIGMComponent self, UIGMComponent.GMTipsItem item)
        {
            var cfg = GMConfigCategory.Instance.Get(self.SelectedId);
            switch (cfg.Code)
            {
                case GMCode.AddRobot:
                    break;
                case GMCode.GetItem:
                {
                    self.ActiveInput.text = ItemConfigCategory.Instance.Get(item.Index).Id.ToString();
                    break;
                }
            }
        }

        private static void ShowItemsList(this UIGMComponent self, string searchTxt)
        {
            var all = ItemConfigCategory.Instance.GetAll();
            self.PopShow.Clear();
            foreach (var itemConfig in all.Values)
            {
                if (itemConfig.Id.ToString().Contains(searchTxt) ||
                    itemConfig.Name.Contains(searchTxt))
                {
                    self.PopShow.Add(new UIGMComponent.GMTipsItem()
                    {
                        Index = itemConfig.Id,
                        Name = $"{itemConfig.Id} {itemConfig.Name}"
                    });
                }
            }
            self.popList.SetData(self.PopShow);
        }
    }
}