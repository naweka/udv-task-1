namespace VkPostReader.Models
{
    public class VkPostStatistics
    {
        public int Id { get; set; }
        public long OwnerID { get; set; }
        public long[] PostsID { get; set; }
        public string StatisticString { get; set; }
        public DateTime DateTime { get; set; }
    }
}
