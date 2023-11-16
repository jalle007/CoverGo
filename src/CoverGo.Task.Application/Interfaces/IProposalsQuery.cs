using CoverGo.Task.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoverGo.Task.Application.Interfaces;

public interface IProposalsQuery
{
    public ValueTask<List<Proposal>> GetAll();
    public ValueTask<Proposal?> GetById(string id, CancellationToken cancellationToken = default);
}
