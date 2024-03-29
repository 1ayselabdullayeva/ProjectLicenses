﻿using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs.Licenses.Create;
using System.Security.Claims;

namespace Api.Controllers
{
    [Authorize("Customer")]
    [Route("api/[controller]")]
    [ApiController]
    public class LicensesController : ControllerBase
    {
        private ILicensesServices _licensesServices;
        public LicensesController(ILicensesServices licensesServices)
        {
            _licensesServices = licensesServices;
        }
        [HttpGet("getLicenses")]
        public IActionResult GetLicenses()
        {
            var id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var responseDto = _licensesServices.GetById(id);
            return Ok(responseDto);
        }

        [HttpPost("Buy")]
        public async Task<IActionResult>CreateLicenses(LicensesCreateDto request)
        {
            var id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var response =await _licensesServices.CreateLicenses(id, request);
            return Ok(response);
        }
        [HttpGet("GetByIdLicenses")]
        public IActionResult GetById(int LicensesId)
        {
            var id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var response =_licensesServices.GetByIdLicenses(id, LicensesId);
            return Ok(response);
        }

    }
}
