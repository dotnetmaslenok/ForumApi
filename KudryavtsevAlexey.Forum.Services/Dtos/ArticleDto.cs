﻿using KudryavtsevAlexey.Forum.Domain.Entities;
using KudryavtsevAlexey.Forum.Domain.Entities.Comments;

using System;
using System.Collections.Generic;

namespace KudryavtsevAlexey.Forum.Services.Dtos
{
    public record ArticleDto(
        int Id,
        string Title,
        string ShortDescription,
        IEnumerable<TagDto> Tags,
        int OrganizationId,
        OrganizationDto Organization,
        int UserId,
        UserDto User,
        DateTime? PublishedAt,
        IEnumerable<ArticleMainCommentDto> MainComments);
}
