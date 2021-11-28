﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KudryavtsevAlexey.Forum.Services.Dtos
{
    public record ListingMainCommentDto(
        int Id,
        string Name,
        DateTime CreatedAt,
        int ListingId,
        ListingDto Listing,
        List<ListingSubCommentDto> SubComments) : BaseDto(Id);
}
