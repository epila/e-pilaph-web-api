﻿using Epila.Ph.WebApi.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Epila.Ph.WebApi.Contracts
{
    public interface IApiConnect
    {
        Task<T> PostDataAsync<T, T2>(string endPoint, T2 dto);
        Task<T> GetDataAsync<T>(string endPoint);
    }
}
