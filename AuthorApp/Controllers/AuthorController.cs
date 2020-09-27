using AuthorApp.ApiContracts.Request;
using AuthorApp.ApiContracts.Response;
using AuthorApp.DataAccess.DataManagers.Authors;
using AuthorApp.DataAccessContracts.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthorApp.Controllers
{
    [ApiController]
    [Route("api/authors")]
    [Authorize]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorManager _authorManager;
        private readonly IMapper _mapper;

        public AuthorController(IAuthorManager authorManager, IMapper mapper)
        {
            _authorManager = authorManager;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAuthors()
        {
            var authors = await _authorManager.GetAuthors();
            if (!authors.IsSuccess)
                return BadRequest();

            var result = _mapper.Map<List<AuthorResponse>>(authors.Data);
            return Ok(new ResponseData<AuthorResponse>(result));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuthor(Guid id)
        {
            var author = await _authorManager.GetAuthorById(id);
            if (!author.IsSuccess)
                return BadRequest(author.ErrorMessage);

            return Ok(_mapper.Map<AuthorRecordResponse>(author.Data));
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create(AuthorCreateRequest author)
        {
            if (author == null)
                return BadRequest();

            var record = _mapper.Map<AuthorCreateDto>(author);
            var idCreatedResult = await _authorManager.Create(record, User.Identity.Name);
            if (!idCreatedResult.IsSuccess)
                return BadRequest(idCreatedResult.ErrorMessage);

            return Ok(idCreatedResult.Data);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Update(Guid id, AuthorUpdateRequest author)
        {
            if (author == null)
                return BadRequest();

            var record = _mapper.Map<AuthorUpdateDto>(author);
            record.Id = id;
            var updateResult = await _authorManager.Update(record, User.Identity.Name);
            if (!updateResult.IsSuccess)
                return BadRequest(updateResult.ErrorMessage);

            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest();

            var idCreatedResult = await _authorManager.Delete(id, User.Identity.Name);
            if (!idCreatedResult.IsSuccess)
                return BadRequest(idCreatedResult.ErrorMessage);

            return Ok();
        }
    }
}
