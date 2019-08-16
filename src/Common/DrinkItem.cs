namespace Common
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class DrinkItem : INotifyPropertyChanged
    {
        private bool isSelected;

        /// <summary>
        /// Gets or sets the cost.
        /// </summary>
        /// <value>
        /// The cost.
        /// </value>
        public string Cost { set; get; }

        /// <summary>
        /// Gets or sets the name of the drink.
        /// </summary>
        /// <value>
        /// The name of the drink.
        /// </value>
        public string Name { set; get; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is selected.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is selected; otherwise, <c>false</c>.
        /// </value>
        public bool IsSelected
        {
            set
            {
                this.isSelected = value;
                this.NotifyPropertyChanged();
            }
            get
            {
                return this.isSelected;
            }
        }

        /// <summary>
        /// Gets or sets the available.
        /// </summary>
        /// <value>
        /// The available.
        /// </value>
        public bool Available { set; get; }

        /// <summary>
        /// Gets the note.
        /// </summary>
        /// <value>
        /// The note.
        /// </value>
        public string Note { get; set; }

        #region Implement INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notifies the property changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
