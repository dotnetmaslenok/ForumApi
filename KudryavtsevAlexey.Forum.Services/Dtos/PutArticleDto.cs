﻿using System.Collections.Generic;
using KudryavtsevAlexey.Forum.Domain.Entities;

namespace KudryavtsevAlexey.Forum.Services.Dtos
{
    public record PutArticleDto(
            string Title,
            string ShortDescription,
            List<TagDto> Tags);
}
