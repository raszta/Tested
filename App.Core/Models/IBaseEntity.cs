namespace App.Core.Models
{
    public interface IBaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool ActiveFlag { get; set; }
    }
}
