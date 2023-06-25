using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ERNIE_Bot.SDK.Models
{
    public class ERNIEBotError
    {
        [JsonPropertyName("error_code")]
        public int Code { get; set; } = -1;

        [JsonPropertyName("error_msg")]
        public string Message { get; set; } = "unknown";
    }

    public class ERNIEBotException : Exception
    {
        public ERNIEBotError Error { get; }
        public ERNIEBotException(ERNIEBotError? error)
        {
            Error = error ?? new ERNIEBotError();
        }
        public ERNIEBotException(int code, string message)
        {
            this.Error = new ERNIEBotError()
            {
                Code = code,
                Message = message
            };
        }
    }

}
