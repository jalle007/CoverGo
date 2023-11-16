using CoverGo.Task.Application.Services;
using CoverGo.Task.Domain;
using Microsoft.AspNetCore.Mvc;

namespace CoverGo.Task.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProposalsController : ControllerBase
{
    private readonly ProposalService _proposalService;

    public ProposalsController(ProposalService proposalService)
    {
        _proposalService = proposalService;
    }

    // GET: api/proposals
    [HttpGet]
    public async Task<ActionResult<List<Proposal>>> GetAll()
    {
        var proposals = await _proposalService.GetAll();
        if (proposals == null)
        {
            return NotFound();
        }
        return Ok(proposals);
    }

    // GET: api/proposals/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Proposal>> GetById(string id)
    {
        var proposal = await _proposalService.GetById(id);
        if (proposal == null)
        {
            return NotFound();
        }
        return Ok(proposal);
    }

    // POST: api/proposals
    [HttpPost]
    public async Task<ActionResult<Proposal>> CreateOrUpdate([FromBody] Proposal proposal)
    {
        var updatedOrCreatedProposal = await _proposalService.CreateOrUpdate(proposal);
        if (updatedOrCreatedProposal == null)
        {
            return BadRequest("Proposal could not be created or updated.");
        }
        return Ok(updatedOrCreatedProposal);
    }


    // DELETE: api/proposals/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> Delete(string id)
    {
        var success = await _proposalService.DeleteProposal(id);
        if (!success)
        {
            return NotFound();
        }
        return NoContent();
    }
}
