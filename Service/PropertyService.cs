using LLMS.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLMS.Service
{
    internal class PropertyService : IPropertyService
    {
        // Import IImageService
        private IImageService _imageService;

        // Constructor
        public PropertyService()
        {
            _imageService = new ImageService();
        }

        /*--- CRUD ---*/
        public async Task<PropertyDto> CreatePropertyAsync(PropertyDto propertyDto)
        {
            try
            {
                using (var context = new testdb1Entities())
                {
                    var propertyEntity = await MapToModelAsync(propertyDto);
                    context.properties.Add(propertyEntity);
                    await context.SaveChangesAsync();
                    return await MapToDtoAsync(propertyEntity);
                }
            }catch (DbUpdateException ex)
            {
                // Database Update Exception
                Trace.TraceError($"DbUpdateException in CreatePropertyAsync: {ex.Message}");
                throw new ApplicationException("An error occurred while accessing the database.");
            }
            catch (InvalidOperationException ex)
            {
                Trace.TraceError($"InvalidOperationException in CreatePropertyAsync: {ex.Message}");
                throw new ApplicationException("An invalid operation was attempted.");
            }
            catch (Exception ex)
            {
                Trace.TraceError($"Exception in CreatePropertyAsync: {ex.Message}");
                throw new ApplicationException("An unexpected error occurred.");
            }
        }

        public async Task<bool> DeletePropertyAsync(int id)
        {
            try
            {
                using (var context = new testdb1Entities())
                {
                    var propertyEntity = await context.properties.FindAsync(id);
                    if (propertyEntity != null)
                    {
                        context.properties.Remove(propertyEntity);
                        await context.SaveChangesAsync();
                        return true;
                    }
                    return false;
                }
            }catch (DbUpdateException ex)
            {
                // Database Update Exception
                Trace.TraceError($"DbUpdateException in DeletePropertyAsync: {ex.Message}");
                throw new ApplicationException("An error occurred while accessing the database.");
    }
            catch (InvalidOperationException ex)
            {
                Trace.TraceError($"InvalidOperationException in DeletePropertyAsync: {ex.Message}");
                throw new ApplicationException("An invalid operation was attempted.");
}
            catch (Exception ex)
            {
                Trace.TraceError($"Exception in DeletePropertyAsync: {ex.Message}");
                throw new ApplicationException("An unexpected error occurred.");
            }
        }

        public async Task<IEnumerable<PropertyDto>> GetAllPropertiesAsync()
        {
            try
            {
                using (var context = new testdb1Entities())
                {
                    var propertyEntities = await context.properties.ToListAsync();
                    var propertyDtosTasks = propertyEntities.Select(p => MapToDtoAsync(p));
                    var propertyDtos = await Task.WhenAll(propertyDtosTasks);
                    return propertyDtos;
                }
            }
            catch (DbUpdateException ex)
            {
                // Database Update Exception
                Trace.TraceError($"DbUpdateException in GetAllPropertiesAsync: {ex.Message}");
                throw new ApplicationException("An error occurred while accessing the database.");
            }
            catch (InvalidOperationException ex)
            {
                Trace.TraceError($"InvalidOperationException in GetAllPropertiesAsync: {ex.Message}");
                throw new ApplicationException("An invalid operation was attempted.");
            }
            catch (Exception ex)
            {
                Trace.TraceError($"Exception in GetAllPropertiesAsync: {ex.Message}");
                throw new ApplicationException("An unexpected error occurred.");
            }
        }


        public async Task<PropertyDto> GetPropertyByIdAsync(int id)
        {
            try
            {

                using (var context = new testdb1Entities())
                {
                    var propertyEntity = await context.properties.FindAsync(id);
                    if (propertyEntity != null)
                    {
                        return await MapToDtoAsync(propertyEntity);
                    }
                }
                return null;
            }
            catch (DbUpdateException ex)
            {
                // Database Update Exception
                Trace.TraceError($"DbUpdateException in GetPropertyByIdAsync: {ex.Message}");
                throw new ApplicationException("An error occurred while accessing the database.");
            }
            catch (InvalidOperationException ex)
            {
                Trace.TraceError($"InvalidOperationException in GetPropertyByIdAsync: {ex.Message}");
                throw new ApplicationException("An invalid operation was attempted.");
            }
            catch (Exception ex)
            {
                Trace.TraceError($"Exception in GetPropertyByIdAsync: {ex.Message}");
                throw new ApplicationException("An unexpected error occurred.");
            }       
        }

        public async Task<PropertyDto> UpdatePropertyAsync(PropertyDto propertyDto)
        {
            try
            {
                using (var context = new testdb1Entities())
                {
                    var propertyEntity = await context.properties.FindAsync(propertyDto.Id);
                    var PropertyData = MapToModelAsync(propertyDto);
                    if (propertyEntity != null)
                    {
                        context.Entry(propertyEntity).CurrentValues.SetValues(PropertyData);
                        await context.SaveChangesAsync();
                        return await MapToDtoAsync(propertyEntity);
                    }
                }
                return null;
            } catch (DbUpdateException ex)
            {
            // Database Update Exception
            Trace.TraceError($"DbUpdateException in UpdatePropertyAsync: {ex.Message}");
            throw new ApplicationException("An error occurred while accessing the database.");
            }
            catch (InvalidOperationException ex)
            {
                Trace.TraceError($"InvalidOperationException in UpdatePropertyAsync: {ex.Message}");
                throw new ApplicationException("An invalid operation was attempted.");
            }
            catch (Exception ex)
            {
                Trace.TraceError($"Exception in UpdatePropertyAsync: {ex.Message}");
                throw new ApplicationException("An unexpected error occurred.");
            }
        }

        private async Task<PropertyDto> MapToDtoAsync(property propertyEntity)
        {
            string imageUrl = null;
            // Check if image_id is greater than 0
            if (propertyEntity.image_id > 0)
            {
                imageUrl = await _imageService.GetImageUrlByIdAsync(propertyEntity.image_id);
            }

            return new PropertyDto
            {
                Id = propertyEntity.id,
                Address = propertyEntity.address,
                NumberOfUnits = propertyEntity.number_of_units,
                PropertyType = propertyEntity.property_type,
                SizeInSqFt = propertyEntity.size_in_sq_ft,
                YearBuilt = propertyEntity.year_built,
                RentalPrice = propertyEntity.rental_price,
                Amenities = propertyEntity.amenities,
                Status = propertyEntity.status,
                LeaseTerms = propertyEntity.lease_terms,
                ImageUrl = imageUrl,
                Description = propertyEntity.description
            };
        }

        private async Task<property> MapToModelAsync(PropertyDto dto)
        {
            int? imageId = null;
            if (!string.IsNullOrWhiteSpace(dto.ImageUrl))
            {
                // Try to get imageId from ImageService
                imageId = await _imageService.GetImageIdByUrlAsync(dto.ImageUrl);

                // Update image record if imageId is not null
                if (imageId == null)
                {
                    // TODO: Implement UploadImageAndCreateRecord method in ImageService
                    // imageId = await _imageService.UploadImageAndCreateRecord(dto.ImageUrl);
                }
            }

            // Map to property model
            return new property
            {
                id = dto.Id,
                address = dto.Address,
                number_of_units = dto.NumberOfUnits,
                property_type = dto.PropertyType,
                size_in_sq_ft = dto.SizeInSqFt,
                year_built = dto.YearBuilt,
                rental_price = dto.RentalPrice,
                amenities = dto.Amenities,
                status = dto.Status,
                lease_terms = dto.LeaseTerms,
                image_id = (int)imageId, 
                description = dto.Description
            };
        }
    }
}
