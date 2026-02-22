namespace hacka_properties_service.Application.DTOs
{
    public class UpdateFieldRequest
    {
        public string Name { get; set; }
        public double AreaInHectares { get; set; }
        public string Crop { get; set; }
    }
}