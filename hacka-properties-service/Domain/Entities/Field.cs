namespace hacka_properties_service.Domain.Entities
{
    public class Field
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; } = null!;
        public double AreaInHectares { get; private set; }
        public string Crop { get; private set; } = null!;
        public Guid PropertyId { get; private set; }

        public Field(string name, double areaInHectares, string crop, Guid propertyId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Field name is required.");

            if (areaInHectares <= 0)
                throw new ArgumentException("Area must be greater than zero.");

            if (string.IsNullOrWhiteSpace(crop))
                throw new ArgumentException("Crop must be defined.");

            Id = Guid.NewGuid();
            Name = name;
            AreaInHectares = areaInHectares;
            Crop = crop;
            PropertyId = propertyId;
        }

        public void Update(string name, double areaInHectares, string crop)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Field name is required.");

            if (areaInHectares <= 0)
                throw new ArgumentException("Area must be greater than zero.");

            if (string.IsNullOrWhiteSpace(crop))
                throw new ArgumentException("Crop must be defined.");

            Name = name;
            AreaInHectares = areaInHectares;
            Crop = crop;
        }
    }
}