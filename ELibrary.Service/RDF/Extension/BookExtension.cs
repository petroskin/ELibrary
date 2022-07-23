using ELibrary.Domain.Models;
using ELibrary.Service.RDF.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VDS.RDF;
using VDS.RDF.Writing;

namespace ELibrary.Service.RDF.Extension
{
    public static class BookExtension
    {
        public static System.IO.FileStream GetRDFGraph(this Book book, Syntax syntax)
        {
            IGraph graph = new Graph();
            IUriNode bookNode = graph.CreateUriNode(new Uri($"https://localhost:5001/Books/Details/{book.Id}"));
            IUriNode bookAuthorNode = graph.CreateUriNode(new Uri($"https://localhost:5001/Authors/Details/{book.AuthorId}"));
            IUriNode labelProperty = graph.CreateUriNode(new Uri("http://www.w3.org/2000/01/rdf-schema#label"));
            IUriNode abstractProperty = graph.CreateUriNode(new Uri("http://dbpedia.org/ontology/abstract"));
            IUriNode authorProperty = graph.CreateUriNode(new Uri("http://dbpedia.org/property/author"));
            IUriNode genreProperty = graph.CreateUriNode(new Uri("http://dbpedia.org/property/genre"));
            graph.Assert(new Triple(bookNode, labelProperty, graph.CreateLiteralNode(book.Name, "en")));
            graph.Assert(new Triple(bookNode, abstractProperty, graph.CreateLiteralNode(book.Description, "en")));
            graph.Assert(new Triple(bookNode, authorProperty, bookAuthorNode));
            book.CategoriesInBook.ToList().ForEach(category => graph.Assert(new Triple(bookNode, genreProperty, graph.CreateLiteralNode(category.Category, "en"))));

            IRdfWriter writer;

            // if you get an error here, create folders rdf and rdf\book

            if (syntax == Syntax.Turtle)
            {
                writer = new VDS.RDF.Writing.TurtleWriter();
                writer.Save(graph, $"rdf\\book\\{book.Id}.ttl");
                return System.IO.File.Open($"rdf\\book\\{book.Id}.ttl", System.IO.FileMode.Open);
            }
            else if (syntax == Syntax.RDFXML)
            {
                writer = new VDS.RDF.Writing.RdfXmlWriter();
                writer.Save(graph, $"rdf\\book\\{book.Id}.xml");
                return System.IO.File.Open($"rdf\\book\\{book.Id}.xml", System.IO.FileMode.Open);
            }
            else
            {
                throw new Exception();
            }
        }
    }
}
