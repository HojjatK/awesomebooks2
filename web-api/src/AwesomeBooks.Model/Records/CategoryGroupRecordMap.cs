using CsvHelper.Configuration;

namespace AwesomeBooks.Model.Records
{
    public class CategoryGroupRecordMap : ClassMap<CategoryGroupRecord>
    {
        public CategoryGroupRecordMap()
        {
            Map(m => m.Name).Name("Name");
            Map(m => m.Description).Name("Description");
        }
    }
}
