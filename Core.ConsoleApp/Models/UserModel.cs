using Core.Lib.Database.Models;

namespace Core.ConsoleApp.Models
{
    public class UserModel : ARepositoryItem
    {
        public string Name {get; set;}
        public string Email {get; set;}
    }
}