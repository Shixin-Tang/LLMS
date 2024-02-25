using LLMS.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace LLMS.Service
{
    internal class PropertyService : IPropertyService
    {
        private readonly IImageService _imageService;

        public PropertyService(IImageService imageService)
        {
            _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
        }

        /*--- CRUD ---*/
        public async Task<PropertyDto> CreatePropertyAsync(PropertyDto propertyDto)
        {
            if (propertyDto.ImageUrl == null)
            {
                throw new ApplicationException("An image must be associated with the property.");
            }
            try
            {
                using (var context = new testdb1Entities())
                {
                    var propertyEntity = await MapToModelAsync(propertyDto);

                    if (propertyEntity.image_id == 0)
                    {
                        throw new ApplicationException("An image must be associated with the property.");
                    }

                    context.properties.Add(propertyEntity);
                    await context.SaveChangesAsync();
                    return await MapToDtoAsync(propertyEntity);
                }
            }
            catch (DbUpdateException ex)
            {
                Trace.TraceError($"DbUpdateException in CreatePropertyAsync: {ex.Message}");
                throw new ApplicationException("An error occurred while accessing the database.");
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
            }
            catch (DbUpdateException ex)
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
            if (propertyDto.ImageUrl == null)
            {
                throw new ApplicationException("An image must be associated with the property.");
            }
            try
            {
                using (var context = new testdb1Entities())
                {
                    var propertyEntity = await context.properties.FindAsync(propertyDto.Id);
                    if (propertyEntity != null)
                    {
                        // 直接比较DTO和实体字段，当它们不一致时更新实体字段
                        if (!string.Equals(propertyEntity.address, propertyDto.Address))
                        {
                            propertyEntity.address = propertyDto.Address;
                        }
                        if (propertyEntity.number_of_units != propertyDto.NumberOfUnits)
                        {
                            propertyEntity.number_of_units = propertyDto.NumberOfUnits;
                        }
                        if (!string.Equals(propertyEntity.property_type, propertyDto.PropertyType))
                        {
                            propertyEntity.property_type = propertyDto.PropertyType;
                        }
                        if (propertyEntity.size_in_sq_ft != propertyDto.SizeInSqFt)
                        {
                            propertyEntity.size_in_sq_ft = propertyDto.SizeInSqFt;
                        }
                        if (propertyEntity.year_built != propertyDto.YearBuilt)
                        {
                            propertyEntity.year_built = propertyDto.YearBuilt;
                        }
                        if (propertyEntity.rental_price != propertyDto.RentalPrice)
                        {
                            propertyEntity.rental_price = propertyDto.RentalPrice;
                        }
                        if (!string.Equals(propertyEntity.amenities, propertyDto.Amenities))
                        {
                            propertyEntity.amenities = propertyDto.Amenities;
                        }
                        if (!string.Equals(propertyEntity.status, propertyDto.Status))
                        {
                            propertyEntity.status = propertyDto.Status;
                        }
                        if (!string.Equals(propertyEntity.lease_terms, propertyDto.LeaseTerms))
                        {
                            propertyEntity.lease_terms = propertyDto.LeaseTerms;
                        }
                        if (!string.Equals(propertyEntity.description, propertyDto.Description))
                        {
                            propertyEntity.description = propertyDto.Description;
                        }

                        int imageId = await _imageService.GetImageIdByUrlAsync(propertyDto.ImageUrl);

                        if (imageId == 0)
                        {
                            imageId = await _imageService.CreateImageRecordAsync(propertyDto.ImageUrl);
                        }
                        if (propertyEntity.image_id != imageId && imageId > 0)
                        {
                            propertyEntity.image_id = imageId;
                        }

                        await context.SaveChangesAsync();
                        return await MapToDtoAsync(propertyEntity);
                    }
                }

                return null;

                /*
                using (var context = new testdb1Entities())
                {
                    var propertyEntity = await context.properties.FindAsync(propertyDto.Id); 
                    
                    var PropertyData = await MapToModelAsync(propertyDto);
                    if(PropertyData.image_id == 0)
                    {
                        PropertyData.image_id = propertyEntity.image_id;
                    }
                    if (propertyEntity != null)
                    {
                        context.Entry(propertyEntity).CurrentValues.SetValues(PropertyData);
                        await context.SaveChangesAsync();
                        return await MapToDtoAsync(propertyEntity);
                    }
                }
                return null;
                */
            }
            catch (DbUpdateException ex)
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
            try
            {
                int imageId = await _imageService.GetImageIdByUrlAsync(dto.ImageUrl);

                if (imageId == 0)
                {
                    imageId = await _imageService.CreateImageRecordAsync(dto.ImageUrl);
                }
                return new property
                {
                    address = dto.Address,
                    number_of_units = dto.NumberOfUnits,
                    property_type = dto.PropertyType,
                    size_in_sq_ft = dto.SizeInSqFt,
                    year_built = dto.YearBuilt,
                    rental_price = dto.RentalPrice,
                    amenities = dto.Amenities,
                    status = dto.Status,
                    lease_terms = dto.LeaseTerms,
                    image_id = imageId,
                    description = dto.Description
                };
            }
            catch(Exception ex)
            {
                Trace.TraceError($"Exception in MapToModelAsync: {ex.Message}");
                throw new ApplicationException("An unexpected error occurred.");
            }
        }
    }
}
