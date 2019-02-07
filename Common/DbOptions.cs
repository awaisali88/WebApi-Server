
namespace Common
{
    public class DbOptions
    {
        public string ConnectionString { get; set; }
        public SqlProvider SqlProvider { get; set; }
        public bool LogQuery { get; set; }
        public bool UseQuotationMarks { get; set; }
    }
}
