using System;
using System.Collections.Generic;
using System.Text;
using VDS.RDF.Query;

namespace ELibrary.Service.RDF.Interface
{
    public interface IRDFService
    {
        List<SparqlResult> GetBookInfo(string name);
        List<SparqlResult> GetAuthorInfo(string name);
    }
}
