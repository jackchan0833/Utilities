
namespace JC.Utilities
{
    /// <summary>
    /// Represents the bussiness content with header.
    /// </summary>
    /// <typeparam name="TContent"></typeparam>
    public abstract class BizRequest<TContent>
        where TContent : class, new()
    {
        /// <summary>
        /// The request application id.
        /// </summary>
        public string AppId { set; get; }

        /// <summary>
        /// The request noncement.
        /// </summary>
        public string Nonce { set; get; }
        
        /// <summary>
        /// The signature type.
        /// </summary>
        public string SignType { set; get; }

        /// <summary>
        /// The signature.
        /// </summary>
        public string Sign { set; get; }

        /// <summary>
        /// The content to request.
        /// </summary>
        public TContent Content { set; get; }
        /// <summary>
        /// Constructs the <see cref="BizRequest{TContent}"/>.
        /// </summary>
        public BizRequest()
        {
            this.Content = new TContent();
        }
    }
}
