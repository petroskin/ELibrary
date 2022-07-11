using System;
using System.Collections.Generic;
using System.Text;
using VDS.RDF.Query;

namespace ELibrary.Service.RDF.Interface
{
    public interface IRDFService
    {
        SparqlResultSet GetBookInfo(string name);
        SparqlResultSet GetAuthorInfo(string name);
    }
}
