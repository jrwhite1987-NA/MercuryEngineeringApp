using System.Collections.ObjectModel;

namespace MercuryEngApp
{
    public class ItemsMenu
    {
        public ItemsMenu()
        {
            this.Items = new ObservableCollection<ItemsMenu>();
        }

        public string Title { get; set; }
        public string PakcetValue { get; set; }
        public int StartRow { get; set; }
        public int StartColumn { get; set; }
        public int EndRow { get; set; }
        public int EndColumn { get; set; }

        public bool IsExpanded { get; set; }
        public bool Collapsed { get; set; }

        public ObservableCollection<ItemsMenu> Items { get; set; }

    }
}
