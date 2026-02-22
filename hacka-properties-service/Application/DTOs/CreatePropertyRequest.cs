namespace hacka_properties_service.Application.DTOs
{
    public class CreatePropertyRequest
    {
        public string Name { get; set; } = null!;
        public string Location { get; set; } = null!;
    }
}