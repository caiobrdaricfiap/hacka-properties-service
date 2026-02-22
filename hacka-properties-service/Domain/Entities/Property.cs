using System;
using System.Collections.Generic;
using System.Linq;

namespace hacka_properties_service.Domain.Entities
{
    public class Property
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = null!;
        public string Location { get; private set; } = null!;
        public Guid ProducerId { get; private set; }

        private readonly List<Field> _fields = new();
        public IReadOnlyCollection<Field> Fields => _fields.AsReadOnly();

        public Property(string name, string location, Guid producerId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Property name is required.");

            if (string.IsNullOrWhiteSpace(location))
                throw new ArgumentException("Location is required.");

            if (producerId == Guid.Empty)
                throw new ArgumentException("ProducerId is required.");

            Id = Guid.NewGuid();
            Name = name;
            Location = location;
            ProducerId = producerId;
        }

        public void Update(string name, string location)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Property name is required.");

            if (string.IsNullOrWhiteSpace(location))
                throw new ArgumentException("Location is required.");

            Name = name;
            Location = location;
        }

        public void AddField(Field field)
        {
            if (field == null)
                throw new ArgumentException("Field cannot be null.");

            _fields.Add(field);
        }

        public void RemoveField(Guid fieldId)
        {
            var field = _fields.FirstOrDefault(f => f.Id == fieldId);

            if (field == null)
                throw new ArgumentException("Field not found.");

            _fields.Remove(field);
        }
    }
}