using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoverGo.Task.Application.Interfaces;
using CoverGo.Task.Domain;

namespace CoverGo.Task.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PlansController : ControllerBase
{
    private readonly IPlansQuery _plansQuery;
    private readonly IPlansWriteRepository _plansWriteRepository;

    public PlansController(IPlansQuery plansQuery, IPlansWriteRepository plansWriteRepository)
    {
        _plansQuery = plansQuery;
        _plansWriteRepository = plansWriteRepository;
    }

    // GET: api/plans
    [HttpGet]
    public async ValueTask<ActionResult<List<Plan>>> GetAll()
    {
        var plans = await _plansQuery.GetAll();
        return Ok(plans);
    }

    // GET: api/plans/5
    [HttpGet("{id}")]
    public async ValueTask<ActionResult<Plan>> GetById(string id)
    {
        var plan = await _plansQuery.GetById(id);
        if (plan == null)
        {
            return NotFound();
        }
        return Ok(plan);
    }

    // POST: api/plans
    [HttpPost]
    public async ValueTask<ActionResult<Plan>> CreateOrUpdate([FromBody] Plan plan)
    {
        var existingPlan = await _plansQuery.GetById(plan.Id.ToString());
        if (existingPlan != null)
        {
            // The plan exists, so update it.
            var updatedPlan = await _plansWriteRepository.CreateOrUpdate(plan);
            return Ok(updatedPlan);
        }
        else
        {
            // The plan does not exist, so create it.
            var createdPlan = await _plansWriteRepository.CreateOrUpdate(plan);
            return CreatedAtAction(nameof(GetById), new { id = createdPlan.Id }, createdPlan);
        }
    }

    // DELETE: api/plans/5
    [HttpDelete("{id}")]
    public async ValueTask<ActionResult<bool>> Delete(string id)
    {
        var success = await _plansWriteRepository.Delete(id);
        if (!success)
        {
            return NotFound();
        }
        return Ok(success);
    }
}
