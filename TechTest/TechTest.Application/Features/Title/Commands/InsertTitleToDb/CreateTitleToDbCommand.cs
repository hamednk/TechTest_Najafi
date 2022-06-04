using AutoMapper;
using MediatR;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TechTest.Application.Interfaces.Repositories;
using TechTest.Application.Wrappers;
using TechTest.Domain.Entities;

namespace TechTest.Application.Features.Titles.Commands.CreateTitleToDbCommand
{
    public partial class CreateTitleToDbCommand : IRequest<Response<bool>>
    {
    }
    public class CreateTitleToDbCommandCommandHandler : IRequestHandler<CreateTitleToDbCommand, Response<bool>>
    {
        private readonly ITitleRepositoryAsync _titleRepository;
        private readonly IMapper _mapper;
        public CreateTitleToDbCommandCommandHandler(ITitleRepositoryAsync titleRepository, IMapper mapper)
        {
            _titleRepository = titleRepository;
            _mapper = mapper;
        }

        public async Task<Response<bool>> Handle(CreateTitleToDbCommand request, CancellationToken cancellationToken)
        {
            var cacheKey = (nameof(Title.Id) + 1);
            var getData = await _titleRepository.GetFromRedisAsync(cacheKey).ConfigureAwait(false);

            await _titleRepository.AddBulkAsync(getData);

            return new Response<bool>(true);
        }

    }
}
