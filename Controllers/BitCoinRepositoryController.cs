﻿using BitCoinManagerModels;
using BitCoinRepositoryApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitCoinRepositoryApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BitCoinRepositoryController : ControllerBase
    {
        private readonly ILogger<BitCoinRepositoryController> _logger;
        private readonly BitCoinRepository _repository;

        public BitCoinRepositoryController(ILogger<BitCoinRepositoryController> logger, BitCoinRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpPost("getuser")]
        public ActionResult<User> GetUser([FromBody]User user)
        {
            try
            {
                return _repository.GetUser(user);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error in 'GetUser'. {e.Message}");
                return BadRequest(e);
            }
        }

        [HttpPost("createuser")]
        public ActionResult<int> CreateUser([FromBody] User user)
        {
            try
            {
                return _repository.InsertUser(user);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error in 'CreateUser'. {e.Message}");
                return BadRequest(e);
            }
        }

        [HttpPost("createorder")]
        public ActionResult<int> CreateOrder([FromRoute]int userId, [FromBody]Order order)
        {
            try
            {
                return _repository.InsertOrder(userId, order);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error in 'CreateOrder'. {e.Message}");
                return BadRequest(e);
            }
        }
    }
}
