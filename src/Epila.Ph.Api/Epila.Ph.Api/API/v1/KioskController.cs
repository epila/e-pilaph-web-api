﻿using System.Threading.Tasks;
using AutoWrapper.Wrappers;
using Epila.Ph.Api.Contracts;
using Epila.Ph.Api.DTO.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using static Microsoft.AspNetCore.Http.StatusCodes;
namespace Epila.Ph.Api.API.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class KioskController : ControllerBase
    {
        private readonly IKioskManager _kioskManager;
        private readonly ILogger<KioskController> _logger;
        public KioskController(IKioskManager kioskManager, ILogger<KioskController> logger)
        {
            _kioskManager = kioskManager;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ApiResponse> Get()
        {
            var data = await _kioskManager.GetAllAsync().ConfigureAwait(false);
            return new ApiResponse(data);
        }

        [Route("{id:long}")]
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse), Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), Status404NotFound)]
        public async Task<ApiResponse> Get(long id)
        {
            var data = await _kioskManager.GetByIdAsync(id).ConfigureAwait(false);
            if (data != null)
                return new ApiResponse(data);
            throw new ApiProblemDetailsException($"Record with id: {id} does not exist.", Status404NotFound);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse), Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), Status422UnprocessableEntity)]
        public async Task<ApiResponse> Post([FromBody] KioskRequest request)
        {
            if (!ModelState.IsValid) throw new ApiProblemDetailsException(ModelState);
            var result = await _kioskManager.CreateAsync(request).ConfigureAwait(false);
            return new ApiResponse("Record successfully created.", result, Status201Created);
        }

        [Route("{id:long}")]
        [HttpPut]
        [ProducesResponseType(typeof(ApiResponse), Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), Status422UnprocessableEntity)]
        public async Task<ApiResponse> Put(long id, [FromBody] KioskRequest request)
        {
            if (!ModelState.IsValid) throw new ApiProblemDetailsException(ModelState);
            var result = await _kioskManager.UpdateAsync(request,id).ConfigureAwait(false);
            return new ApiResponse("Record successfully updated.", result, Status201Created);
        }

        [Route("{id:long}")]
        [HttpDelete]
        [ProducesResponseType(typeof(ApiResponse), Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), Status404NotFound)]
        public async Task<ApiResponse> Delete(long id)
        {
            if (await _kioskManager.DeleteAsync(id))
                return new ApiResponse($"Record with Id: {id} successfully deleted.", true);
            throw new ApiException($"Record with id: {id} does not exist.", Status404NotFound);
        }
    }
}