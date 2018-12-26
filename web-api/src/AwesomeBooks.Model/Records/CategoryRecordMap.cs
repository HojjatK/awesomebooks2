using CsvHelper.Configuration;

namespace AwesomeBooks.Model.Records
{
    public class CategoryRecordMap : ClassMap<CategoryRecord>
    {
        public CategoryRecordMap()
        {
            Map(m => m.Name).Name("Name");
            Map(m => m.Description).Name("Description");
            Map(m => m.GroupName).Name("Group Name");
        }
    }
}
