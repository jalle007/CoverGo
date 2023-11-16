using CoverGo.Task.Application.Services;
using CoverGo.Task.Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoverGo.Task.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InsuredGroupsController : ControllerBase
    {
        private readonly InsuredGroupService _insuredGroupService;

        public InsuredGroupsController(InsuredGroupService insuredGroupService)
        {
            _insuredGroupService = insuredGroupService;
        }

        // GET: api/insuredgroups
        [HttpGet]
        public async Task<ActionResult<List<InsuredGroup>>> GetAll()
        {
            var insuredGroups = await _insuredGroupService.GetAll();
            return Ok(insuredGroups);
        }

        // GET: api/insuredgroups/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<InsuredGroup>> GetById(Guid id)
        {
            var insuredGroup = await _insuredGroupService.GetById(id);
            if (insuredGroup == null)
            {
                return NotFound();
            }
            return Ok(insuredGroup);
        }

        // POST: api/insuredgroups
        [HttpPost]
        public async Task<ActionResult<InsuredGroup>> CreateOrUpdate([FromBody] InsuredGroup insuredGroup)
        {
            try
            {
                var savedInsuredGroup = await _insuredGroupService.CreateOrUpdate(insuredGroup);
                if (savedInsuredGroup == null)
                {
                    return BadRequest("Unable to create or update the insured group.");
                }
                return savedInsuredGroup.Id == insuredGroup.Id ? Ok(savedInsuredGroup) : CreatedAtAction(nameof(GetById), new { id = savedInsuredGroup.Id }, savedInsuredGroup);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/insuredgroups/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var success = await _insuredGroupService.Delete(id);
            if (!success)
            {
                return NotFound();
            }
            return Ok(success);
        }
    }
}
