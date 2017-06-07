using System;
using System.Collections.Generic;
using Digipolis.Iod_abs.Dossier.Model.Models;
using System.Threading.Tasks;

namespace Digipolis.Iod_abs.Dossier.Domain.Interfaces
{
    public interface IDossierManager
    {
        IEnumerable<GenericDossier> GetAllDossiers();
        GenericDossier GetCompanyAssetDossierById(Guid id);
        GenericDossier InsertDossier(GenericDossier dossier, Guid dossierTypeId);
        GenericDossier UpdateDossier(Guid id, GenericDossier dossier);
    }
}
