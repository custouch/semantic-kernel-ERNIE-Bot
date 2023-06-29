using ERNIE_Bot.SDK.Models;

namespace ERNIE_Bot.Sample.Controllers.Models
{
    public class UserInput
    {
        public string Role { get; set; } = MessageRole.User;
        public string Text { get; set; } = string.Empty;
    }
    public class UserHistoryInput
    {
        public List<UserInput> Messages { get; set; } = new List<UserInput> { };
    }
}
