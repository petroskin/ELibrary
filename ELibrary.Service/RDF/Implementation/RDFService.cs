using ELibrary.Service.RDF.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VDS.RDF.Parsing;
using VDS.RDF.Query;

namespace ELibrary.Service.RDF.Implementation
{
    public class RDFService : IRDFService 
    {
        private readonly SparqlParameterizedString _queryString;
        public RDFService()
        {
            _queryString = new SparqlParameterizedString();

            _queryString.Namespaces.AddNamespace("rdf", new Uri("http://www.w3.org/1999/02/22-rdf-syntax-ns#"));
            _queryString.Namespaces.AddNamespace("dbo", new Uri("http://dbpedia.org/ontology/"));
            _queryString.Namespaces.AddNamespace("dbp", new Uri("http://dbpedia.org/property/"));

            _queryString.QueryProcessor = new RemoteQueryProcessor(new SparqlRemoteEndpoint(new Uri("http://dbpedia.org/sparql"), "http://dbpedia.org"));

            GetBookInfo("Eugene Onegin");
        }

        public SparqlResultSet GetAuthorInfo(string name)
        {
            _queryString.CommandText = $"select * where " +
                $"{{ " +
                $"?book rdf:type dbo:Writer . " +
                $"?book rdfs:label ?name . " +
                $"?book ?rel ?obj . " +
                $"FILTER (REGEX(?name,\"{name}\")) " +
                $"}}";

            SparqlResultSet resultSet = _queryString.ExecuteQuery();

            return resultSet;
        }

        public SparqlResultSet GetBookInfo(string name)
        {
            _queryString.CommandText = $"select * where " +
                $"{{ " +
                $"?book rdf:type dbo:WrittenWork . " +
                $"?book dbp:name ?name . " +
                $"?book ?rel ?obj . " +
                $"FILTER (REGEX(?name,\"{name}\")) " +
                $"}}";

            SparqlResultSet resultSet = _queryString.ExecuteQuery();

            return resultSet;
        }
    }
}

/* 
 * 
 * book attributes: dbo:WrittenWork, dbo:abstract, dbp:genre, dbp:language, dbp:releaseDate, dbp:titleOrig, 
 * 
 * writer attributes: dbo:Writer, rdfs:label
 * 
 */