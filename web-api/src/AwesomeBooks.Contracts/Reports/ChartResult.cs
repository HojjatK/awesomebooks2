using System.Collections.Generic;

namespace AwesomeBooks.Contracts.Reports
{
    public class ChartSerie
    {
        public int SerieId { get; set; }
        public string SerieName { get; set; }
        public List<decimal> Data { get; set; }
    }

    public class ChartResult<T>
    {
        public string ChartTitle { get; set; }

        public List<string> Labels = new List<string>();

        public List<T> Data { get; set; }
    }

    public class ChartSerieResult : ChartResult<ChartSerie>
    {   
    }

    public class ChartResult : ChartResult<decimal>
    {   
    }
}
