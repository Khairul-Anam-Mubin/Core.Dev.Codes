using Core.Lib.Database.Interfaces;
namespace Core.Lib.Database.Models
{
    public abstract class ARepositoryItem : IRepositoryItem
    {
        public string Id { get; set; }
        
        public virtual void CreateGuidId()
        {
            Id = Guid.NewGuid().ToString();
        }
        
        public string GetId() 
        {
            return Id;
        }
        
        public void SetId(string id)
        {
            Id = id;
        }
    }
}