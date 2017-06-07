using Digipolis.Iod_abs.Dossier.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Digipolis.Iod_abs.Dossier.Domain.Interfaces
{
    public interface ITasksApiClient
    {
        bool StartProcessForDossierAndProcessDefinitionId(GenericDossier dossier, string processDefinitionId);
    }
}
