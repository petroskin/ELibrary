using ELibrary.Domain.Models;
using ELibrary.Service.RDF.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using VDS.RDF;

namespace ELibrary.Service.RDF.Extension
{
    public static class AuthorExtension
    {
        public static System.IO.FileStream GetRDFGraph(this Author author, Syntax syntax)
        {
            IGraph graph = new Graph();
            IUriNode authorNode = graph.CreateUriNode(new Uri($"https://localhost:5001/Authors/Details/{author.Id}"));
            IUriNode labelProperty = graph.CreateUriNode(new Uri("http://www.w3.org/2000/01/rdf-schema#label"));
            IUriNode nameProperty = graph.CreateUriNode(new Uri("http://xmlns.com/foaf/0.1/name"));
            IUriNode placeProperty = graph.CreateUriNode(new Uri("http://dbpedia.org/property/deathPlace"));
            graph.Assert(new Triple(authorNode, labelProperty, graph.CreateLiteralNode(author.FullName(), "en")));
            graph.Assert(new Triple(authorNode, nameProperty, graph.CreateLiteralNode(author.FullName(), "en")));
            graph.Assert(new Triple(authorNode, placeProperty, graph.CreateLiteralNode(author.Country, "en")));

            // if you get an error here, create folders rdf and rdf\author

            IRdfWriter writer;

            if (syntax == Syntax.Turtle)
            {
                writer = new VDS.RDF.Writing.TurtleWriter();
                writer.Save(graph, $"rdf\\author\\{author.Id}.ttl");
                return System.IO.File.Open($"rdf\\author\\{author.Id}.ttl", System.IO.FileMode.Open);
            }
            else if (syntax == Syntax.RDFXML)
            {
                writer = new VDS.RDF.Writing.RdfXmlWriter();
                writer.Save(graph, $"rdf\\author\\{author.Id}.xml");
                return System.IO.File.Open($"rdf\\author\\{author.Id}.xml", System.IO.FileMode.Open);
            }
            else
            {
                throw new Exception();
            }
        }
    }
}
