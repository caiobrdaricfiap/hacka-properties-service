using hacka_properties_service.Application.DTOs;
using hacka_properties_service.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace hacka_properties_service.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PropertiesController : ControllerBase
    {
        private static readonly List<Property> _properties = new();

        private Guid GetAuthenticatedProducerId()
        {
            var producerIdClaim = User.FindFirst("sub")?.Value;

            if (producerIdClaim == null)
                throw new UnauthorizedAccessException("Producer not found in token.");

            return Guid.Parse(producerIdClaim);
        }

        // =====================
        // PROPERTIES
        // =====================

        [HttpPost]
        public IActionResult Create(CreatePropertyRequest request)
        {
            var producerId = GetAuthenticatedProducerId();

            var property = new Property(
                request.Name,
                request.Location,
                producerId
            );

            _properties.Add(property);

            return CreatedAtAction(nameof(GetById), new { id = property.Id },
                new PropertyResponse
                {
                    Id = property.Id,
                    Name = property.Name,
                    Location = property.Location,
                    ProducerId = property.ProducerId
                });
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var producerId = GetAuthenticatedProducerId();

            var properties = _properties
                .Where(p => p.ProducerId == producerId)
                .Select(p => new PropertyResponse
                {
                    Id = p.Id,
                    Name = p.Name,
                    Location = p.Location,
                    ProducerId = p.ProducerId
                })
                .ToList();

            return Ok(properties);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var producerId = GetAuthenticatedProducerId();

            var property = _properties
                .FirstOrDefault(p => p.Id == id && p.ProducerId == producerId);

            if (property == null)
                return NotFound();

            return Ok(new PropertyResponse
            {
                Id = property.Id,
                Name = property.Name,
                Location = property.Location,
                ProducerId = property.ProducerId
            });
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, UpdatePropertyRequest request)
        {
            var producerId = GetAuthenticatedProducerId();

            var property = _properties
                .FirstOrDefault(p => p.Id == id && p.ProducerId == producerId);

            if (property == null)
                return NotFound();

            property.Update(request.Name, request.Location);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var producerId = GetAuthenticatedProducerId();

            var property = _properties
                .FirstOrDefault(p => p.Id == id && p.ProducerId == producerId);

            if (property == null)
                return NotFound();

            _properties.Remove(property);

            return NoContent();
        }

        // =====================
        // FIELDS
        // =====================

        [HttpPost("{propertyId}/fields")]
        public IActionResult AddField(Guid propertyId, CreateFieldRequest request)
        {
            var producerId = GetAuthenticatedProducerId();

            var property = _properties
                .FirstOrDefault(p => p.Id == propertyId && p.ProducerId == producerId);

            if (property == null)
                return NotFound("Property not found.");

            var field = new Field(
                request.Name,
                request.AreaInHectares,
                request.Crop,
                propertyId
            );

            property.AddField(field);

            return CreatedAtAction(nameof(GetFieldById),
                new { propertyId, fieldId = field.Id },
                new FieldResponse
                {
                    Id = field.Id,
                    Name = field.Name,
                    AreaInHectares = field.AreaInHectares,
                    Crop = field.Crop
                });
        }

        [HttpGet("{propertyId}/fields")]
        public IActionResult GetFields(Guid propertyId)
        {
            var producerId = GetAuthenticatedProducerId();

            var property = _properties
                .FirstOrDefault(p => p.Id == propertyId && p.ProducerId == producerId);

            if (property == null)
                return NotFound("Property not found.");

            var fields = property.Fields
                .Select(f => new FieldResponse
                {
                    Id = f.Id,
                    Name = f.Name,
                    AreaInHectares = f.AreaInHectares,
                    Crop = f.Crop
                })
                .ToList();

            return Ok(fields);
        }

        [HttpGet("{propertyId}/fields/{fieldId}")]
        public IActionResult GetFieldById(Guid propertyId, Guid fieldId)
        {
            var producerId = GetAuthenticatedProducerId();

            var property = _properties
                .FirstOrDefault(p => p.Id == propertyId && p.ProducerId == producerId);

            if (property == null)
                return NotFound("Property not found.");

            var field = property.Fields.FirstOrDefault(f => f.Id == fieldId);

            if (field == null)
                return NotFound("Field not found.");

            return Ok(new FieldResponse
            {
                Id = field.Id,
                Name = field.Name,
                AreaInHectares = field.AreaInHectares,
                Crop = field.Crop
            });
        }

        [HttpPut("{propertyId}/fields/{fieldId}")]
        public IActionResult UpdateField(Guid propertyId, Guid fieldId, UpdateFieldRequest request)
        {
            var producerId = GetAuthenticatedProducerId();

            var property = _properties
                .FirstOrDefault(p => p.Id == propertyId && p.ProducerId == producerId);

            if (property == null)
                return NotFound("Property not found.");

            var field = property.Fields.FirstOrDefault(f => f.Id == fieldId);

            if (field == null)
                return NotFound("Field not found.");

            field.Update(request.Name, request.AreaInHectares, request.Crop);

            return NoContent();
        }

        [HttpDelete("{propertyId}/fields/{fieldId}")]
        public IActionResult DeleteField(Guid propertyId, Guid fieldId)
        {
            var producerId = GetAuthenticatedProducerId();

            var property = _properties
                .FirstOrDefault(p => p.Id == propertyId && p.ProducerId == producerId);

            if (property == null)
                return NotFound("Property not found.");

            property.RemoveField(fieldId);

            return NoContent();
        }
    }
}