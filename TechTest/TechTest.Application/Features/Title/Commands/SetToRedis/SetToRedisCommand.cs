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

namespace TechTest.Application.Features.Titles.Commands.SetToRedis
{
    public partial class SetToRedisCommand : IRequest<Response<bool>>
    {
        public Stream Stream { get; set; }
    }
    public class SetToRedisCommandHandler : IRequestHandler<SetToRedisCommand, Response<bool>>
    {
        private readonly ITitleRepositoryAsync _titleRepository;
        private readonly IMapper _mapper;
        public SetToRedisCommandHandler(ITitleRepositoryAsync titleRepository, IMapper mapper)
        {
            _titleRepository = titleRepository;
            _mapper = mapper;
        }

        public async Task<Response<bool>> Handle(SetToRedisCommand request, CancellationToken cancellationToken)
        {
            var titleList = await UploadFile(request.Stream);

            var cacheKey = (nameof(Title.Id) + 1);
            await _titleRepository.SetToRedisAsync(titleList, cacheKey).ConfigureAwait(false);

            return new Response<bool>(true);
        }

        private async Task<IEnumerable<Title>> UploadFile(Stream fileStream)
        {
            try
            {
                var titleList = new List<Title>();

                using var package = new ExcelPackage(fileStream);
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var currentSheet = package.Workbook.Worksheets;
                var workSheet = currentSheet.First();
                var noOfCol = workSheet.Dimension.End.Column;
                var noOfRow = workSheet.Dimension.End.Row;
                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                    titleList.Add(new Title
                    {
                        ToolName = workSheet.Cells[rowIterator, 1].Value?.ToString(),
                    });

                return titleList.ToHashSet();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
 
    }
}
