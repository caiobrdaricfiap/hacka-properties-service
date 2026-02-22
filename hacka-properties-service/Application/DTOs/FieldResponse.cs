namespace hacka_properties_service.Application.DTOs
{
    public class FieldResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double AreaInHectares { get; set; }
        public string Crop { get; set; }
    }
}