using CsvHelper.Configuration;
using RazorAllinRent.Models;

namespace RazorAllinRent.Mappers
{
    public sealed class ItemTypeMap : ClassMap<ItemType>
    {
        public ItemTypeMap()
        {
            Map(m => m.Label).Name("Label");
            Map(m => m.IsActive).Name("IsActive");
        }
    }
}
