using System.Collections.ObjectModel;

namespace MercuryEngApp
{
    public class ItemsMenu
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ItemsMenu()
        {
            this.Items = new ObservableCollection<ItemsMenu>();
        }

        /// <summary>
        /// Gets of sets the Title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets of sets the Packet Value
        /// </summary>
        public string PakcetValue { get; set; }

        /// <summary>
        /// Gets of sets the Start Row
        /// </summary>
        public int StartRow { get; set; }

        /// <summary>
        /// Gets of sets the Start Column
        /// </summary>
        public int StartColumn { get; set; }

        /// <summary>
        /// Gets of sets the End Row
        /// </summary>
        public int EndRow { get; set; }

        /// <summary>
        /// Gets of sets the End Column
        /// </summary>
        public int EndColumn { get; set; }

        /// <summary>
        /// Gets of sets the Is Expanded
        /// </summary>
        public bool IsExpanded { get; set; }

        /// <summary>
        /// Gets of sets the IsCollapsed
        /// </summary>
        public bool Collapsed { get; set; }

        /// <summary>
        /// Gets of sets the Items Collection
        /// </summary>
        public ObservableCollection<ItemsMenu> Items { get; set; }

    }
}
