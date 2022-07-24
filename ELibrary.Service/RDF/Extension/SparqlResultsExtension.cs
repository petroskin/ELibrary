using ELibrary.Domain.Models;
using ELibrary.Service.RDF.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VDS.RDF;
using VDS.RDF.Query;

namespace ELibrary.Service.RDF.Extension
{
    public static class SparqlResultsExtension
    {
        public static IGraph AsGraph(this List<SparqlResult> results)
        {
            IGraph graph = new Graph();

            bool book = false;
            if (results[0].HasValue("book"))
                book = true;

            foreach(SparqlResult r in results)
            {
                if (book)
                    graph.Assert(new Triple(r.Value("book"), r.Value("rel"), r.Value("obj"), graph));
                else
                    graph.Assert(new Triple(r.Value("author"), r.Value("rel"), r.Value("obj"), graph));
            }

            return graph;
        }

        public static System.IO.FileStream AsGraph(this List<SparqlResult> results, Author author, Syntax syntax)
        {
            IRdfWriter writer;

            // if you get an error here, create folders rdf and rdf\author

            if (syntax == Syntax.Turtle)
            {
                writer = new VDS.RDF.Writing.TurtleWriter();
                writer.Save(results.AsGraph(), $"rdf\\author\\{author.Id}.sparql.ttl");
                return System.IO.File.Open($"rdf\\author\\{author.Id}.sparql.ttl", System.IO.FileMode.Open);
            }
            else if (syntax == Syntax.RDFXML)
            {
                writer = new VDS.RDF.Writing.RdfXmlWriter();
                writer.Save(results.AsGraph(), $"rdf\\author\\{author.Id}.sparql.xml");
                return System.IO.File.Open($"rdf\\author\\{author.Id}.sparql.xml", System.IO.FileMode.Open);
            }
            else
            {
                throw new Exception();
            }
        }

        public static System.IO.FileStream AsGraph(this List<SparqlResult> results, Book book, Syntax syntax)
        {
            IRdfWriter writer;

            // if you get an error here, create folders rdf and rdf\book

            if (syntax == Syntax.Turtle)
            {
                writer = new VDS.RDF.Writing.TurtleWriter();
                writer.Save(results.AsGraph(), $"rdf\\book\\{book.Id}.sparql.ttl");
                return System.IO.File.Open($"rdf\\book\\{book.Id}.sparql.ttl", System.IO.FileMode.Open);
            }
            else if (syntax == Syntax.RDFXML)
            {
                writer = new VDS.RDF.Writing.RdfXmlWriter();
                writer.Save(results.AsGraph(), $"rdf\\book\\{book.Id}.sparql.xml");
                return System.IO.File.Open($"rdf\\book\\{book.Id}.sparql.xml", System.IO.FileMode.Open);
            }
            else
            {
                throw new Exception();
            }
        }
    }
}
