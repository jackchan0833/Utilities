using Newtonsoft.Json;

namespace JC.Utilities
{
    /// <summary>
    /// Represents the base business response.
    /// </summary>
    public class BizResponse
    {
        /// <summary>
        /// The signature type.
        /// </summary>
        [JsonProperty("sign_type")]
        public string SignType { set; get; }

        /// <summary>
        /// The signature.
        /// </summary>
        [JsonProperty("sign")]
        public string Sign { set; get; }

        /// <summary>
        /// The code for success or failure.
        /// </summary>
        [JsonProperty("code")]
        public string Code { set; get; }

        /// <summary>
        /// The message for failure.
        /// </summary>
        [JsonProperty("msg")]
        public string Msg { set; get; }

        /// <summary>
        /// The sub code for failure message.
        /// </summary>
        [JsonProperty("sub_code")]
        public string SubCode { set; get; }

        /// <summary>
        /// The sub failure message.
        /// </summary>
        [JsonProperty("sub_msg")]
        public string SubMsg { set; get; }

        /// <summary>
        /// Check whether is failure.
        /// </summary>
        [JsonIgnore]
        public bool IsError
        {
            get { return Code != ExecutionResult.Success; }
        }

        /// <summary>
        /// Check whether is success.
        /// </summary>
        [JsonIgnore]
        public bool IsSuccess
        {
            get { return !IsError; }
        }

        /// <summary>
        /// Set response with failure error.
        /// </summary>
        /// <param name="subError"></param>
        /// <param name="errorMsg"></param>
        public void Fail(ErrorInfo subError, string errorMsg = "error")
        {
            Code = ExecutionResult.Fail;
            Msg = errorMsg;
            SubCode = subError.Code;
            SubMsg = subError.Message;
        }
        /// <summary>
        /// Set response to success.
        /// </summary>
        public void Success()
        {
            //default with success
            Code = ExecutionResult.Success;
            Msg = "ok";
        }

    }
    /// <summary>
    /// Represents the business responent with content.
    /// </summary>
    /// <typeparam name="TContent"></typeparam>
    public class BizResponse<TContent> : BizResponse
        where TContent : class, new()
    {
        /// <summary>
        /// The content to response.
        /// </summary>
        [JsonProperty("content")]
        public TContent Content { set; get; }

        /// <summary>
        /// Constructs the business response.
        /// </summary>
        public BizResponse()
        {
            this.Content = new TContent();
        }
    }
}
