using AutoMapper;
using AutoMapper.Internal.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AwsWorkshop.Product.Api.Core.Application.Abstracts;
using AwsWorkshop.Product.Api.Core.Application.Abstracts.Repositories;
using AwsWorkshop.Product.Api.Core.Application.Abstracts.UnitOfWorks;
using AwsWorkshop.Product.Api.Core.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AwsWorkshop.Product.Api.Infrastructure.Persistence.Concretes.Services
{
    public class ServiceGeneric<TEntity, TDto> : IServiceGeneric<TEntity, TDto> where TEntity : class where TDto : class
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ServiceGeneric<TEntity, TDto>> _logger;

        private readonly IRepository<TEntity> _repository;

        public ServiceGeneric(IUnitOfWork unitOfWork, IRepository<TEntity> genericRepository, IMapper mapper, ILogger<ServiceGeneric<TEntity, TDto>> logger)
        {
            _unitOfWork = unitOfWork;
            _repository = genericRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<Response<TDto>> AddAsync(TDto entity)
        {
            try
            {
                var newEntity = _mapper.Map<TEntity>(entity);

                await _repository.AddAsync(newEntity);

                var newDto = _mapper.Map<TDto>(newEntity);

                return Response<TDto>.Success(newDto, 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response<TDto>.Fail(ex.Message, 500);
            }
        }
        public async Task<Response<TDto>> AddAndSaveAsync(TDto entity)
        {
            try
            {
                var newEntity = _mapper.Map<TEntity>(entity);

                await _repository.AddAsync(newEntity);
                await _unitOfWork.CommitAsync();

                var newDto = _mapper.Map<TDto>(newEntity);

                return Response<TDto>.Success(newDto, 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response<TDto>.Fail(ex.Message, 500);
            }
        }
        public async Task<Response<List<TDto>>> AddRangeAsync(List<TDto> entities)
        {
            try
            {
                var newEntity = _mapper.Map<List<TEntity>>(entities);

                await _repository.AddRangeAsync(newEntity);

                await _unitOfWork.CommitAsync();
                var newDto = _mapper.Map<List<TDto>>(newEntity);

                return Response<List<TDto>>.Success(newDto, 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response<List<TDto>>.Fail(ex.Message, 500);
            }
        }
        public async Task<Response<IEnumerable<TDto>>> GetAllAsync()
        {
            try
            {
                var response = _mapper.Map<List<TDto>>(await _repository.GetAllAsync());
                return Response<IEnumerable<TDto>>.Success(response, 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response<IEnumerable<TDto>>.Fail(ex.Message, 200);
            }
        }
        public async Task<Response<IEnumerable<TDto>>> GetAllAsync(Expression<Func<TEntity, object>> includeProperty)
        {
            try
            {
                var response = _mapper.Map<List<TDto>>(await _repository.GetAllAsync(includeProperty));
                return Response<IEnumerable<TDto>>.Success(response, 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response<IEnumerable<TDto>>.Fail(ex.Message, 200);
            }
        }
        public async Task<Response<IEnumerable<TDto>>> GetAllAsync(Expression<Func<TEntity, object>> includeProperty1, Expression<Func<TEntity, object>> includeProperty2)
        {
            try
            {
                var response = _mapper.Map<List<TDto>>(await _repository.GetAllAsync(includeProperty1, includeProperty2));
                return Response<IEnumerable<TDto>>.Success(response, 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response<IEnumerable<TDto>>.Fail(ex.Message, 200);
            }
        }
        public async Task<Response<IEnumerable<TDto>>> GetAllAsync(Expression<Func<TEntity, object>> includeProperty1, Expression<Func<TEntity, object>> includeProperty2, Expression<Func<TEntity, object>> includeProperty3)
        {
            try
            {
                var response = _mapper.Map<List<TDto>>(await _repository.GetAllAsync(includeProperty1, includeProperty2, includeProperty3));
                return Response<IEnumerable<TDto>>.Success(response, 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response<IEnumerable<TDto>>.Fail(ex.Message, 200);
            }
        }
        public async Task<Response<TDto>> GetByIdAsync(int id)
        {
            var response = await _repository.GetByIdAsync(id);

            if (response == null)
            {
                return Response<TDto>.Fail("Id not found", 404);
            }

            return Response<TDto>.Success(_mapper.Map<TDto>(response), 200);
        }
        public async Task<Response<NoContent>> Remove(int id)
        {
            var isExistEntity = await _repository.GetByIdAsync(id);

            if (isExistEntity == null)
            {
                return Response<NoContent>.Fail("Id not found", 404);
            }

            _repository.Remove(isExistEntity);

            //204 durum kodu =>  No Content  => Response body'sinde hiç bir dat  olmayacak.
            return Response<NoContent>.Success(204);
        }
        public async Task<Response<NoContent>> Update(TDto entity)
        {
            var updateEntity = _mapper.Map<TEntity>(entity);

            _repository.Update(updateEntity);

            return Response<NoContent>.Success(204);
        }
        public async Task<Response<IEnumerable<TDto>>> Where(Expression<Func<TEntity, bool>> predicate)
        {
            var list = _repository.Where(predicate);

            return Response<IEnumerable<TDto>>.Success(_mapper.Map<IEnumerable<TDto>>(await list.ToListAsync()), 200);
        }
        public async Task<Response<TDto>> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var entity = await _repository.FirstOrDefaultAsync(predicate);

            return Response<TDto>.Success(_mapper.Map<TDto>(entity), 200);
        }
        public async Task<Response<bool>> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var result = await _repository.AnyAsync(predicate);

            return Response<bool>.Success(result, 200);
        }
        public async Task<Response<NoContent>> SaveAsync()
        {
            try
            {
                await _repository.SaveAsync();
                return Response<NoContent>.Success(204);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response<NoContent>.Fail(ex.Message, 500);
            }
        }
    }
}
