namespace FoodOrdering.Models
{
    using Caliburn.Micro;
    using Common;

    public class Request : PropertyChangedBase
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        /// <value>
        /// The comment.
        /// </value>
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        /// <value>
        /// The file path.
        /// </value>
        public string FilePath { get; set; }

        /// <summary>
        /// Gets or sets the request.
        /// </summary>
        /// <value>
        /// The request.
        /// </value>
        public DrinkItem RequestInfo { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Request"/> class.
        /// </summary>
        /// <param name="order">The order.</param>
        public Request(OrderItem order)
        {
            this.Name = order.UserName;
            this.Comment = order.Comments;
            this.RequestInfo = order.DrinkItems[0];
            this.FilePath = order.FilePath;
        }
    }
}
