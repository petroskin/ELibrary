﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ELibrary.Domain.Models
{
    [Table("book", Schema = "elibrary")]
    public class Book : BaseEntity
    {
        public static IEnumerable<string> BookCategories = new List<string> { "Adventure", "Classic", "Novel", "Drama", "Fantasy", "Humor", "Mythology", "Romance", "Sci-Fi" };
        [Column("name")]
        public string Name { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("imagelink")]
        public string ImageLink { get; set; }
        public IEnumerable<BookAuthor> Authors { get; set; }
        public IEnumerable<CategoriesInBook> Categories { get; set; }
        [Column("publisherid")]
        public int PublisherId { get; set; }
        public Publisher Publisher { get; set; }
        [Column("totalrents")]
        public int TotalRents { get; set; }
        [Column("avgrating")]
        public decimal AvgRating { get; set; }
        public Book()
        {
            Categories = new List<CategoriesInBook>();
            Authors = new List<BookAuthor>();
            TotalRents = 0;
            AvgRating = 0;
        }
        public Book(string name, string description, string imageLink, int publisherId)
        {
            Name = name;
            Description = description;
            ImageLink = imageLink;
            Categories = new List<CategoriesInBook>();
            Authors = new List<BookAuthor>();
            PublisherId = publisherId;
            TotalRents = 0;
            AvgRating = 0;
        }
        public Book(string name, string description, string imageLink, Publisher publisher)
        {
            Name = name;
            Description = description;
            ImageLink = imageLink;
            Categories = new List<CategoriesInBook>();
            Authors = new List<BookAuthor>();
            Publisher = publisher;
            PublisherId = publisher.Id;
            TotalRents = 0;
            AvgRating = 0;
        }
    }
}
