﻿using RSAHyundai.DTOs.Tags;
using RSAHyundai.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RSAHyundai.Interfaces
{
    public interface ITagService
    {
        Task<ServiceResponse<TagDetailsDTO>> CreateAsync(Guid userId, TagDTO tagDto);
        Task<ServiceResponse<TagDetailsDTO>> FindByIdAsync(Guid userId, Guid id);
        Task<ServiceResponse<TagDetailsDTO>> UpdateAsync(Guid userId, Guid id, TagDTO tagDto);
        Task<ServiceResponse<TagDetailsDTO>> DeleteAsync(Guid userId, Guid id);
        Task<ServiceResponse<IEnumerable<TagDetailsDTO>>> FindAllAsync(Guid userId);
    }
}
