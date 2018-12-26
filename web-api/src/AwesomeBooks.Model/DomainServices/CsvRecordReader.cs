using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AwesomeBooks.Model.DomainServices
{
    public interface ICsvRecordReader
    {
        IList<TRecord> GetRecords<TRecord, TRecordMap>(string contect) where TRecordMap : ClassMap<TRecord>;
        string GetContent<TRecord, TRecordMap>(IEnumerable<TRecord> records) where TRecordMap : ClassMap<TRecord>;
    }

    public class CsvRecordReader : ICsvRecordReader
    {
        public IList<TRecord> GetRecords<TRecord, TRecordMap>(string content) where TRecordMap : ClassMap<TRecord>
        {
            var textReader = new StringReader(content);
            var csv = new CsvReader(textReader);
            csv.Configuration.IgnoreBlankLines = true;
            csv.Configuration.MissingFieldFound = null;
            csv.Configuration.ShouldSkipRecord = record =>
            {
                if (record.Any(s => s.Contains("\0")))
                {
                    return true;
                }
                return record.All(string.IsNullOrWhiteSpace);
            };
            csv.Configuration.RegisterClassMap<TRecordMap>();
            var records = csv.GetRecords<TRecord>().ToList();
            return records;
        }

        public string GetContent<TRecord, TRecordMap>(IEnumerable<TRecord> records) where TRecordMap : ClassMap<TRecord>
        {
            var textWriter = new StringWriter();
            var csv = new CsvWriter(textWriter);
            csv.Configuration.RegisterClassMap<TRecordMap>();
            csv.Configuration.QuoteAllFields = true;
            csv.WriteRecords(records);
            textWriter.Flush();
            return textWriter.GetStringBuilder().ToString();
        }
    }
}
