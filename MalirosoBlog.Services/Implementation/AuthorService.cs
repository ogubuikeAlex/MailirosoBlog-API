using AutoMapper;
using MalirosoBlog.Data.Interfaces;
using MalirosoBlog.Models.DTO.Request;
using MalirosoBlog.Models.DTO.Response;
using MalirosoBlog.Models.Entities;
using MalirosoBlog.Services.Exceptions;
using MalirosoBlog.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MalirosoBlog.Services.Implementation
{
    public class AuthorService : IAuthorService
    {
        private readonly IServiceFactory _serviceFactory;

        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        private readonly IRepository<Author> _authorRepo;

        public AuthorService(IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;

            _unitOfWork = _serviceFactory.GetService<IUnitOfWork>();

            _mapper = _serviceFactory.GetService<IMapper>();

            _authorRepo = _unitOfWork.GetRepository<Author>();
        }

        public async Task<AuthorResponse> Create(CreateAuthorDTO request)
        {
            var exisitingAuthor =
                _authorRepo.GetSingleBy(x =>
                x.UserId.ToLower() == request.UserId.ToLower().Trim());

            if (exisitingAuthor is not null)
                throw new NotFoundException("An author already exists");

            var newAuthorDetails = _mapper.Map<Author>(request);

            var createdAuthor = await _authorRepo.AddAsync(newAuthorDetails);

            return createdAuthor is null
                ? throw new InvalidOperationException("Author Creation Failed")
                : _mapper.Map<AuthorResponse>(createdAuthor);
        }

        public async Task<AuthorResponse> Delete(string id)
        {
            Author author = await _authorRepo.GetSingleByAsync(x => x.Id.ToLower().Equals(id.Trim().ToLower()), include: x => x.Include(x => x.User));

            //TODO: Create a custom Exception
            if (author is null || !author.Active)
                throw new NotFoundException("Author Not Found");

            author.Active = false;

            var updateAuthorResponse = await _authorRepo.UpdateAsync(author);

            return _mapper.Map<AuthorResponse>(updateAuthorResponse);
        }

        public async Task<AuthorResponse> Get(string id)
        {
            Author author = await _authorRepo.GetSingleByAsync(x => x.Id.ToLower().Equals(id.Trim().ToLower()), include: x => x.Include(x => x.User));

            if (author is null || !author.Active)
                throw new NotFoundException("Author Not Found");

            return _mapper.Map<AuthorResponse>(author);
        }

        public async Task<PagedResponse<AuthorResponse>> Get(AuthorRequestParameter request)
        {
            var pagedItems = string.IsNullOrWhiteSpace(request.SearchTerm)
               ? await _authorRepo.GetPagedItems(request, c => c.Active, include: x => x.Include(x => x.User))
               : await _authorRepo.GetPagedItems(request,
               c =>
               c.Active &&
               (c.User.FirstName.Contains(request.SearchTerm, StringComparison.CurrentCultureIgnoreCase) ||
               c.User.LastName.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase)),
               include: c => c.Include(x => x.User));

            return _mapper.Map<PagedResponse<AuthorResponse>>(pagedItems);
        }

        public async Task<AuthorResponse> GetByUserId(string id)
        {
            Author author = await _authorRepo
                .GetSingleByAsync(x => x.UserId == id, include: x => x.Include(x => x.User));

            if (author is null || !author.Active)
                throw new NotFoundException("Author Not Found");

            return _mapper.Map<AuthorResponse>(author);
        }
    }
}
