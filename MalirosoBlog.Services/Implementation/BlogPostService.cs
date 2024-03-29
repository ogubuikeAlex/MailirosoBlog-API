using AutoMapper;
using MalirosoBlog.Data.Interfaces;
using MalirosoBlog.Models.DTO.Request;
using MalirosoBlog.Models.DTO.Response;
using MalirosoBlog.Models.Entities;
using MalirosoBlog.Services.Exceptions;
using MalirosoBlog.Services.Extensions;
using MalirosoBlog.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MalirosoBlog.Services.Implementation
{
    public class BlogPostService : IBlogPostService
    {
        private readonly IServiceFactory _serviceFactory;

        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        private readonly IRepository<BlogPost> _blogPostRepo;

        private readonly IHttpContextAccessor _contextAccessor;

        public BlogPostService(IServiceFactory serviceFactory, IHttpContextAccessor contextAccessor)
        {
            _serviceFactory = serviceFactory;

            _unitOfWork = _serviceFactory.GetService<IUnitOfWork>();

            _mapper = _serviceFactory.GetService<IMapper>();

            _blogPostRepo = _unitOfWork.GetRepository<BlogPost>();

            _contextAccessor = contextAccessor;
        }

        public async Task<BlogPostResponse> Create(CreateBlogPostDTO request)
        {
            var authorProfile = await GetAuthorProfile();

            var exisitingBlogPost =
                _blogPostRepo.GetSingleBy(x =>
                x.Title.ToLower().Equals(request.Title.ToLower().Trim()) &&
                x.Content.ToLower().Equals(request.Content.ToLower().Trim()) &&
                x.AuthorId.ToLower().Equals(authorProfile.AuthorId.ToLower().Trim()));

            if (exisitingBlogPost is not null)
            {
                throw new InvalidOperationException("An identical blogpost already exists");
            }

            var blogPost = _mapper.Map<BlogPost>(request);

            blogPost.AuthorId = authorProfile.AuthorId;

            var createdBlogPost = await _blogPostRepo.AddAsync(blogPost) ?? throw new InvalidOperationException("Blog Post Creation Failed");

            var response = _mapper.Map<BlogPostResponse>(createdBlogPost);

            return _mapper.Map(authorProfile, response);
        }

        public async Task<BlogPostResponse> Delete(long id)
        {
            var authorProfile = await GetAuthorProfile();

            BlogPost blogPost = await _blogPostRepo.GetByIdAsync(id);

            if (blogPost == null || !blogPost.Active)
                throw new NotFoundException("Blog Post Not Found");

            if (blogPost.AuthorId != authorProfile.AuthorId)
            {
                throw new UnauthorizedAccessException("Only the Blog Creator Can Delete The Blog");
            }
            
            if (blogPost == null || !blogPost.Active)
                throw new NotFoundException("Blog Post Not Found");

            blogPost.Active = false;

            var updateBlogPostResponse = await _blogPostRepo.UpdateAsync(blogPost);

            var response = _mapper.Map<BlogPostResponse>(updateBlogPostResponse);

            return _mapper.Map(authorProfile, response);
        }

        public async Task<BlogPostResponse> Get(long id)
        {
            BlogPost blogPost = await _blogPostRepo.GetSingleByAsync(x => x.Id == id, include: x => x.Include(x => x.Author).ThenInclude(x => x.User));

            if (blogPost == null || !blogPost.Active)
                throw new NotFoundException("Blog Post Not Found");

            return _mapper.Map<BlogPostResponse>(blogPost);
        }

        public async Task<PagedResponse<BlogPostResponse>> Get(BlogPostRequestParameter request)
        {
            var pagedItems = string.IsNullOrWhiteSpace(request.SearchTerm)
               ? await _blogPostRepo.GetPagedItems(request, c => c.Active, include: x => x.Include(x => x.Author).ThenInclude(x => x.User))
               : await _blogPostRepo.GetPagedItems(request,
               c =>
               c.Active &&
               (c.Title.Contains(request.SearchTerm, StringComparison.CurrentCultureIgnoreCase) ||
               c.Author.User.FirstName.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
               c.Author.User.LastName.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase)),
               include: c => c.Include(x => x.Author).ThenInclude(x => x.User));

            return _mapper.Map<PagedResponse<BlogPostResponse>>(pagedItems);
        }

        public async Task<BlogPostResponse> Update(long id, UpdateBlogPostDTO request)
        {
            var authorProfile = await GetAuthorProfile();

            BlogPost blogPost = await _blogPostRepo.GetByIdAsync(id);

            if (blogPost.AuthorId != authorProfile.AuthorId)
            {
                throw new UnauthorizedAccessException("Only the Blog Creator Can Update The Blog");
            }            

            if (blogPost is null || !blogPost.Active)
                throw new NotFoundException("Blog Post Not Found");

            if (blogPost.Title.Equals(request.Title.ToLower().Trim(), StringComparison.CurrentCultureIgnoreCase) && blogPost.Content.ToLower() == request.Content.ToLower().Trim())
                throw new InvalidOperationException("The Blog Post Update Information Matches The Exisiting Blogpost!");

            _mapper.Map(request, blogPost);

            blogPost = await _blogPostRepo.UpdateAsync(blogPost);

             var response = _mapper.Map<BlogPostResponse>(blogPost);

            return _mapper.Map(authorProfile, response);
        }

        private async Task<AuthorResponse> GetAuthorProfile()
        {
            string? userId = _contextAccessor.HttpContext?.User.GetUserId()
                ?? throw new UnauthorizedAccessException("UnAthorized User"); ;

            var authorService = _serviceFactory.GetService<IAuthorService>();

            var authorProfile = await authorService.GetByUserId(userId?.Trim())
                ?? throw new InvalidOperationException("Author Not Found");

            return authorProfile;
        }
    }
}
