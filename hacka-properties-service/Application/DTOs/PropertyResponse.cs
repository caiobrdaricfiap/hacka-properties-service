namespace hacka_properties_service.Application.DTOs
{
    public class PropertyResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public Guid ProducerId { get; set; }
    }
}