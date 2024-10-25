using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TxnManager.Domain.Service.Abstractions;
using TxnManager.Domain.Service.Filters;
using TxnManager.Web.Dto;

namespace TxnManager.Web.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionsService _transactionsService;
        private readonly IMapper _mapper;

        public TransactionsController(
            ITransactionsService transactionsService,
            IMapper mapper)
        {
            _transactionsService = transactionsService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<TransactionDto>>> GetAllByFilterAsync([FromQuery] TransactionsFilter transactionsFilter)
        {
            var transactions = await _transactionsService.GetAllAsync(transactionsFilter);

            var dtos = _mapper.Map<List<TransactionDto>>(transactions);
            return Ok(dtos);
        }
    }
}