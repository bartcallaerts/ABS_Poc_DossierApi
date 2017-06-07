using Digipolis.Iod_abs.Dossier.Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Digipolis.Iod_abs.Dossier.DataProvider.Interfaces
{
    public interface IDossierTypeConfigDataProvider
    {
        DossierTypeConfig GetDossierTypeConfigByDossierTypeId(Guid dossierTypeId);
    }
}
