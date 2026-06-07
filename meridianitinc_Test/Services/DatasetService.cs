using System;
using System.Collections.Generic;
using System.Text;

namespace Meridianitinc_Assessment.Services
{
    public class DatasetService
    {
        private readonly ApiService _apiService;

        public DatasetService(ApiService apiService)
        {
            _apiService = apiService;
        }
    }
}
