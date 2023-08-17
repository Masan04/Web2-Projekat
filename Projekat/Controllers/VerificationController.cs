using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Projekat.Dto;
using Projekat.Interfaces;
using System.Runtime.InteropServices;

namespace Projekat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VerificationController : ControllerBase
    {
        private readonly IVerificationService _verificationService;

        public VerificationController(IVerificationService verificationService)
        {
            _verificationService = verificationService;
        }

        [HttpGet("{userId}")]
        [Authorize(Roles ="admin,seller")]
        public IActionResult GetVerification(long userId)
        {
            VerificationDto verificationDto = _verificationService.GetByUserId(userId);
            if(verificationDto == null)
            {
                return BadRequest("Error occurred while getting verification!");
            }

            return Ok(verificationDto);
        }

        [HttpGet("all")]
        [Authorize(Roles ="admin")]
        public IActionResult GetAll()
        {
            List<VerificationDto> verifications = new List<VerificationDto>();
            verifications = _verificationService.GetAll(); 

            if(verifications == null)
            {
                return BadRequest("Error occurred while getting all verifications!");
            }

            return Ok(verifications);
        }

        [HttpPut("{id}")]
        [Authorize(Roles ="admin")]
        public IActionResult UpdateVerification(long id, [FromBody]VerificationDto verificationDto)
        {
            VerificationDto verification = _verificationService.UpdateVerification(id,verificationDto);
            if (verification == null)
            {
                return BadRequest("Error occurred while updating verification!");
            }

            return Ok(verification);
        }
    }
}
