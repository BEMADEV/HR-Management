namespace com.bemaservices.HrManagement.ViewModels
{
    /// <summary>
    /// The additional configuration options for the Pto Request List block.
    /// </summary>
    public class PtoRequestListOptionsBag
    {
        /// <summary>
        /// Gets or sets a value indicating whether there is a person context.
        /// </summary>
        public bool HasPersonContext { get; set; }

        /// <summary>
        /// Gets or sets the context person's IdKey.
        /// </summary>
        public string ContextPersonIdKey { get; set; }
    }
}
