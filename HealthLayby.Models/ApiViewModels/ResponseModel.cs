namespace HealthLayby.Models.ApiViewModels
{
    /// <summary>
    /// ResponseModel
    /// </summary>
    public class ResponseModel
    {
        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public int Status { get; init; }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; init; } = string.Empty;

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public object? Data { get; init; }

        /// <summary>
        /// Prevents a default instance of the <see cref="ResponseModel"/> class from being created.
        /// </summary>
        private ResponseModel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseModel"/> class.
        /// </summary>
        /// <param name="Status">The status.</param>
        public ResponseModel(int Status)
        {
            this.Status = Status;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseModel"/> class.
        /// </summary>
        /// <param name="Status">The status.</param>
        /// <param name="Message">The message.</param>
        public ResponseModel(int Status, string Message)
        {
            this.Status = Status;
            this.Message = Message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseModel"/> class.
        /// </summary>
        /// <param name="Status">The status.</param>
        /// <param name="Message">The message.</param>
        /// <param name="Data">The data.</param>
        public ResponseModel(int Status, string Message, object Data)
        {
            this.Status = Status;
            this.Message = Message;
            this.Data = Data;
        }

        /// <summary>
        /// Generates the response.
        /// </summary>
        /// <param name="Status">The status.</param>
        /// <returns></returns>
        public static ResponseModel GenerateResponse(int Status)
        {
            return new ResponseModel
            (
                Status: Status
            );
        }

        /// <summary>
        /// Generates the response.
        /// </summary>
        /// <param name="Status">The status.</param>
        /// <param name="Message">The message.</param>
        /// <returns></returns>
        public static ResponseModel GenerateResponse(int Status, string Message)
        {
            return new ResponseModel
            (
                Status: Status,
                Message: Message
            );
        }

        /// <summary>
        /// Generates the response.
        /// </summary>
        /// <param name="Status">The status.</param>
        /// <param name="Message">The message.</param>
        /// <param name="Data">The data.</param>
        /// <returns></returns>
        public static ResponseModel GenerateResponse(int Status, string Message, object Data)
        {
            return new ResponseModel
            (
                Status: Status,
                Message: Message,
                Data: Data
            );
        }
    }
}
