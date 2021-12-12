﻿using System;
using AutoMapper;
using KudryavtsevAlexey.Forum.Domain.CustomExceptions;
using KudryavtsevAlexey.Forum.Domain.Entities.Comments;
using KudryavtsevAlexey.Forum.Infrastructure.Database;
using KudryavtsevAlexey.Forum.Services.Dtos;
using KudryavtsevAlexey.Forum.Services.Dtos.Comment;
using KudryavtsevAlexey.Forum.Services.ServicesAbstractions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KudryavtsevAlexey.Forum.Services.Services
{
    internal sealed class CommentService : ICommentService
    {
        private readonly ForumDbContext _dbContext;

        private readonly IMapper _mapper;

        public CommentService(ForumDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<ArticleMainCommentDto>> GetArticleComments(int articleId)
        {
            var articleMainComments = await _dbContext.ArticleMainComments
                .Where(x => x.ArticleId == articleId)
                .Include(x => x.Article)
                .Include(x => x.SubComments)
                .ToListAsync();

            List<ArticleMainCommentDto> articleMainCommentsDtos = _mapper.Map<List<ArticleMainCommentDto>>(articleMainComments);

            return articleMainCommentsDtos;
        }

        public async Task<List<ListingMainCommentDto>> GetListingComments(int listingId)
        {
            var listingMainComments = await _dbContext.ListingMainComments
                .Where(x => x.ListingId == listingId)
                .Include(x => x.Listing)
                .Include(x => x.SubComments)
                .ToListAsync();

            List<ListingMainCommentDto> listingMainCommentsDtos = _mapper.Map<List<ListingMainCommentDto>>(listingMainComments);

            return listingMainCommentsDtos;
        }

        public async Task<ArticleMainCommentDto> GetArticleMainCommentById(int articleMainCommentId)
        {
            var articleMainComment = await _dbContext.ArticleMainComments
                .Include(x => x.Article)
                .Include(x => x.SubComments)
                .FirstOrDefaultAsync(x => x.Id == articleMainCommentId);

            if (articleMainComment is null)
            {
                throw new ArticleMainCommentNotFoundException(articleMainCommentId);
            }

            var articleMainCommentDto = _mapper.Map<ArticleMainCommentDto>(articleMainComment);

            return articleMainCommentDto;
        }

        public async Task<ListingMainCommentDto> GetListingMainCommentById(int listingMainCommentId)
        {
            var listingMainComment = await _dbContext.ListingMainComments
                .Include(x => x.Listing)
                .Include(x => x.SubComments)
                .FirstOrDefaultAsync(x => x.Id == listingMainCommentId);

            if (listingMainComment is null)
            {
                throw new ListingMainCommentNotFoundException(listingMainCommentId);
            }

            var listingMainCommentDto = _mapper.Map<ListingMainCommentDto>(listingMainComment);

            return listingMainCommentDto;
        }

        public async Task<ArticleSubCommentDto> GetArticleSubCommentById(int articleSubCommentId)
        {
            var articleSubComment = await _dbContext.ArticleSubComments
                .Include(x => x.Article)
                .Include(x => x.ArticleMainComment)
                .FirstOrDefaultAsync(x => x.Id == articleSubCommentId);

            if (articleSubComment is null)
            {
                throw new ArticleSubCommentNotFoundException(articleSubCommentId);
            }

            var articleSubCommentDto = _mapper.Map<ArticleSubCommentDto>(articleSubComment);

            return articleSubCommentDto;
        }

        public async Task<ListingSubCommentDto> GetListingSubCommentById(int listingSubCommentId)
        {
            var listingSubComment = await _dbContext.ListingSubComments
                .Include(x => x.Listing)
                .Include(x => x.ListingMainComment)
                .FirstOrDefaultAsync(x => x.Id == listingSubCommentId);

            if (listingSubComment is null)
            {
                throw new ListingSubCommentNotFoundException(listingSubCommentId);
            }

            var listingSubCommentDto = _mapper.Map<ListingSubCommentDto>(listingSubComment);

            return listingSubCommentDto;
        }

        public async Task<List<ArticleMainCommentDto>> GetAllArticlesComments()
        {
            var allArticleMainComments = await _dbContext.ArticleMainComments
                .Include(x => x.Article)
                .Include(x => x.SubComments)
                .ToListAsync();

            var allArticlesMainCommentsDtos = _mapper.Map<List<ArticleMainCommentDto>>(allArticleMainComments);

            return allArticlesMainCommentsDtos;
        }

        public async Task<List<ListingMainCommentDto>> GetAllListingsComments()
        {
            var allListingMainComments = await _dbContext.ListingMainComments
                .Include(x => x.Listing)
                .Include(x => x.SubComments)
                .ToListAsync();

            var allListingsMainCommentsDtos = _mapper.Map<List<ListingMainCommentDto>>(allListingMainComments);

            return allListingsMainCommentsDtos;
        }

        public async Task CreateArticleMainComment(CreateArticleMainCommentDto articleMainCommentDto)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == articleMainCommentDto.UserId);

            if (user is null)
            {
                throw new UserNotFoundException(articleMainCommentDto.UserId);
            }

            var articleMainComment = _mapper.Map<ArticleMainComment>(articleMainCommentDto);

            articleMainComment.User = user;
            user.ArticleMainComments.Add(articleMainComment);

            await _dbContext.ArticleMainComments.AddAsync(articleMainComment);
            await _dbContext.SaveChangesAsync();
        }

        public async Task CreateListingMainComment(CreateListingMainCommentDto listingMainCommentDto)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == listingMainCommentDto.UserId);

            if (user is null)
            {
                throw new UserNotFoundException(listingMainCommentDto.UserId);
            }

            var listingMainComment = _mapper.Map<ListingMainComment>(listingMainCommentDto);

            listingMainComment.User = user;
            user.ListingMainComments.Add(listingMainComment);

            await _dbContext.ListingMainComments.AddAsync(listingMainComment);
            await _dbContext.SaveChangesAsync();
        }

        public async Task CreateArticleSubComment(CreateArticleSubCommentDto articleSubCommentDto)
        {
            var article = await _dbContext.Articles.FirstOrDefaultAsync(x => x.Id == articleSubCommentDto.ArticleId);
            
            if (article is null)
            {
                throw new ArticleNotFoundException(articleSubCommentDto.ArticleId);
            }

            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == articleSubCommentDto.UserId);

            if (user is null)
            {
                throw new UserNotFoundException(articleSubCommentDto.UserId);
            }

            var articleMainComment = await _dbContext.ArticleMainComments.FirstOrDefaultAsync(x => x.Id == articleSubCommentDto.ArticleMainCommentId);

            if (articleMainComment is null)
            {
                throw new ArticleMainCommentNotFoundException(articleSubCommentDto.ArticleMainCommentId);
            }

            var articleSubComment = _mapper.Map<ArticleSubComment>(articleSubCommentDto);

            articleSubComment.Article = article;
            articleSubComment.User = user;
            articleSubComment.ArticleMainComment = articleMainComment;

            await _dbContext.ArticleSubComments.AddAsync(articleSubComment);
            await _dbContext.SaveChangesAsync();
        }

        public async Task CreateListingSubComment(CreateListingSubCommentDto listingSubCommentDto)
        {
            var listing = await _dbContext.Listings.FirstOrDefaultAsync(x => x.Id == listingSubCommentDto.ListingId);

            if (listing is null)
            {
                throw new ListingNotFoundException(listingSubCommentDto.ListingId);
            }

            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == listingSubCommentDto.UserId);

            if (user is null)
            {
                throw new UserNotFoundException(listingSubCommentDto.UserId);
            }

            var listingMainComment = await _dbContext.ListingMainComments.FirstOrDefaultAsync(x => x.Id == listingSubCommentDto.ListingMainCommentId);

            if (listingMainComment is null)
            {
                throw new ListingMainCommentNotFoundException(listingSubCommentDto.ListingMainCommentId);
            }

            var listingSubComment = _mapper.Map<ListingSubComment>(listingSubCommentDto);

            listingSubComment.Listing = listing;
            listingSubComment.User = user;
            listingSubComment.ListingMainComment = listingMainComment;

            await _dbContext.ListingSubComments.AddAsync(listingSubComment);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateArticleMainComment(int articleMainCommentId, UpdateArticleMainCommentDto articleMainCommentDto)
        {
            var articleMainComment = await _dbContext.ArticleMainComments.FirstOrDefaultAsync(x => x.Id == articleMainCommentId);

            if (articleMainComment is null)
            {
                throw new ArticleMainCommentNotFoundException(articleMainCommentId);
            }

            articleMainComment = _mapper.Map<ArticleMainComment>(articleMainCommentDto);

            _dbContext.ArticleMainComments.Update(articleMainComment);
            await _dbContext.SaveChangesAsync();

        }

        public async Task UpdateListingMainComment(int listingMainCommentId, UpdateListingMainCommentDto listingMainCommentDto)
        {
            var listingMainComment = await _dbContext.ListingMainComments.FirstOrDefaultAsync(x => x.Id == listingMainCommentId);

            if (listingMainComment is null)
            {
                throw new ListingMainCommentNotFoundException(listingMainCommentId);
            }

            listingMainComment.Content = listingMainCommentDto.Content;

            _dbContext.ListingMainComments.Update(listingMainComment);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateArticleSubComment(int articleSubCommentId, UpdateArticleSubCommentDto articleSubCommentDto)
        {
            var articleSubComment = await _dbContext.ArticleSubComments.FirstOrDefaultAsync(x => x.Id == articleSubCommentId);

            if (articleSubComment is null)
            {
                throw new ArticleSubCommentNotFoundException(articleSubCommentId);
            }

            articleSubComment.Content = articleSubCommentDto.Content;

            _dbContext.ArticleSubComments.Update(articleSubComment);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateListingSubComment(int listingSubCommentId, UpdateListingSubCommentDto listingSubCommentDto)
        {
            var listingSubComment = await _dbContext.ListingSubComments.FirstOrDefaultAsync(x => x.Id == listingSubCommentId);

            if (listingSubComment is null)
            {
                throw new ListingSubCommentNotFoundException(listingSubCommentId);
            }

            listingSubComment.Content = listingSubCommentDto.Content;

            _dbContext.ListingSubComments.Update(listingSubComment);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteArticleMainComment(int articleMainCommentId)
        {
            var articleMainComment = await _dbContext.ArticleMainComments.FirstOrDefaultAsync(x => x.Id == articleMainCommentId);

            if (articleMainComment is null)
            {
                throw new ArticleMainCommentNotFoundException(articleMainCommentId);
            }

            _dbContext.ArticleMainComments.Remove(articleMainComment);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteListingMainComment(int listingMainCommentId)
        {
            var listingMainComment = await _dbContext.ListingMainComments.FirstOrDefaultAsync(x => x.Id == listingMainCommentId);

            if (listingMainComment is null)
            {
                throw new ListingMainCommentNotFoundException(listingMainCommentId);
            }

            _dbContext.ListingMainComments.Remove(listingMainComment);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteArticleSubComment(int articleSubCommentId)
        {
            var articleSubComment = await _dbContext.ArticleSubComments.FirstOrDefaultAsync(x => x.Id == articleSubCommentId);

            if (articleSubComment is null)
            {
                throw new ArticleSubCommentNotFoundException(articleSubCommentId);
            }

            _dbContext.ArticleSubComments.Remove(articleSubComment);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteListingSubComment(int listingSubCommentId)
        {
            var listingSubComment = await _dbContext.ListingSubComments.FirstOrDefaultAsync(x => x.Id == listingSubCommentId);

            if (listingSubComment is null)
            {
                throw new ListingSubCommentNotFoundException(listingSubCommentId);
            }

            _dbContext.ListingSubComments.Remove(listingSubComment);
            await _dbContext.SaveChangesAsync();
        }
    }
}
