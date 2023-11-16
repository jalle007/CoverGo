using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoverGo.Task.Application.Interfaces;
using CoverGo.Task.Domain;

namespace CoverGo.Task.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CompaniesController : ControllerBase
{
    private readonly ICompaniesQuery _companiesQuery;
    private readonly ICompaniesWriteRepository _companiesWriteRepository;

    public CompaniesController(ICompaniesQuery companiesQuery, ICompaniesWriteRepository companiesWriteRepository)
    {
        _companiesQuery = companiesQuery;
        _companiesWriteRepository = companiesWriteRepository;
    }

    // GET: api/companies
    [HttpGet]
    public async ValueTask<ActionResult<List<Company>>> GetAll()
    {
        var companies = await _companiesQuery.GetAll();
        return Ok(companies);
    }

    // GET: api/companies/5
    [HttpGet("{id}")]
    public async ValueTask<ActionResult<Company>> GetById(string id)
    {
        var company = await _companiesQuery.GetById(id);
        if (company == null)
        {
            return NotFound();
        }
        return Ok(company);
    }

    // POST: api/companies
    [HttpPost]
    public async ValueTask<ActionResult<Company>> CreateOrUpdate([FromBody] Company company)
    {
        var existingCompany = await _companiesQuery.GetById(company.Id.ToString());
        if (existingCompany != null)
        {
            // The company exists, so update it.
            var updatedCompany = await _companiesWriteRepository.CreateOrUpdate(company);
            return Ok(updatedCompany);
        }
        else
        {
            // The company does not exist, so create it.
            var createdCompany = await _companiesWriteRepository.CreateOrUpdate(company);
            return CreatedAtAction(nameof(GetById), new { id = createdCompany.Id }, createdCompany);
        }
    }

    // DELETE: api/companies/5
    [HttpDelete("{id}")]
    public async ValueTask<ActionResult<bool>> Delete(string id)
    {
        var success = await _companiesWriteRepository.Delete(id);
        if (!success)
        {
            return NotFound();
        }
        return Ok(success);
    }
}
