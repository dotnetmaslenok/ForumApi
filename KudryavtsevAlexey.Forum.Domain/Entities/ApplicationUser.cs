﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KudryavtsevAlexey.Forum.Domain.BaseEntities;
using KudryavtsevAlexey.Forum.Domain.Entities.Comments;
using Microsoft.AspNetCore.Identity;

namespace KudryavtsevAlexey.Forum.Domain.Entities
{
	public class ApplicationUser : IdentityUser<int>
    {
        public string Name { get; set; }

		public string Summary { get; set; }

		public string Location { get; set; }

		public DateTime JoinedAt { get; set; } = DateTime.UtcNow.Date;

		public ICollection<Article> Articles { get; set; }

		public ICollection<Subscriber> Subscribers { get; set; }

		public ICollection<Listing> Listings { get; set; }

        public ICollection<ArticleMainComment> ArticleMainComments { get; set; }

        public ICollection<ListingMainComment> ListingMainComments { get; set; }

        public ICollection<ArticleSubComment> ArticleSubComments { get; set; }

        public ICollection<ListingSubComment> ListingSubComments { get; set; }

        public int OrganizationId { get; set; }

		public Organization Organization { get; set; }

        public ApplicationUser()
        {
            Articles = new List<Article>();
            Subscribers = new List<Subscriber>();
            Listings = new List<Listing>();
            ArticleMainComments = new List<ArticleMainComment>();
            ListingMainComments = new List<ListingMainComment>();
            ArticleSubComments = new List<ArticleSubComment>();
            ListingSubComments = new List<ListingSubComment>();
        }
	}
}