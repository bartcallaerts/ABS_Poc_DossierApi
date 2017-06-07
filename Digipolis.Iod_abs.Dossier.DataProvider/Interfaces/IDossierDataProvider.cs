using Digipolis.Iod_abs.Dossier.Model.Models;
using System;
using System.Collections.Generic;

namespace Digipolis.Iod_abs.Dossier.DataProvider.Interfaces
{
    public interface IDossierDataProvider
    {
        IEnumerable<GenericDossier> GetDossiers();
        int CountDossiers();
        GenericDossier GetDossierById(Guid id);
        GenericDossier InsertDossier(GenericDossier dossier, Guid dossierTypeId);
        GenericDossier UpdateDossier(GenericDossier dossier);
    }
}
