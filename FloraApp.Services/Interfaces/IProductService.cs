using FloraApp.Model.Requests;
using FloraApp.Model.Responses;
using FloraApp.Model.SearchObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraApp.Services.Interfaces
{
    public interface IProductService : ICrudService<ProductResponse, ProductSearchObject, ProductUpsertRequest, ProductUpsertRequest>
    {
        // Ovdje možete dodati dodatne metode specifične za proizvode
    }
}
