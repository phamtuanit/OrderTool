namespace Common
{
    using System.Collections.Generic;

    public class OrderItem
    {
        /// <summary>
        /// Gets the name of the drink.
        /// </summary>
        /// <value>
        /// The name of the drink.
        /// </value>
        public string DrinkName
        {
            get
            {
                if (this.DrinkItems != null && this.DrinkItems.Count > 0)
                {
                    return this.DrinkItems[0].Name;
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string UserName { set; get; }

        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        /// <value>
        /// The comments.
        /// </value>
        public string Comments { set; get; }

        /// <summary>
        /// Gets or sets the drink items.
        /// </summary>
        /// <value>
        /// The drink items.
        /// </value>
        public IList<DrinkItem> DrinkItems { set; get; }

        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        /// <value>
        /// The file path.
        /// </value>
        public string FilePath { get; set; }
    }
}
