﻿using System.Collections.Generic;
using KudryavtsevAlexey.Forum.Domain.BaseEntities;

namespace KudryavtsevAlexey.Forum.Domain.Entities
{
    public class Organization : BaseEntity
    {
        public string Name { get; set; }

        public ICollection<Listing> Listings { get; set; }

        public ICollection<User> Users { get; set; }

        public ICollection<Article> Articles { get; set; }

        public string ImageUrl { get; set; } = "ProfileImages\\ProfileImage.png";
    }
}