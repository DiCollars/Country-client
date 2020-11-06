namespace Client.Models
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int CapitalId { get; set; }
        public double Area { get; set; }
        public int Population { get; set; }
        public int RegionId { get; set; }

        public City Capital { get; set; }
        public Region Region { get; set; }
    }
}
