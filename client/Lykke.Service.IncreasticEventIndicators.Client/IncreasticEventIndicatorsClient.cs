using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Service.IncreasticEventIndicators.Client.AutorestClient;
using Lykke.Service.IncreasticEventIndicators.Client.AutorestClient.Models;
using Lykke.Service.IncreasticEventIndicators.Client.Exceptions;
using Microsoft.Rest;
using Newtonsoft.Json;

namespace Lykke.Service.IncreasticEventIndicators.Client
{
    public class IncreasticEventIndicatorsClient : IIncreasticEventIndicatorsClient
    {
        private readonly ILog _log;
        private readonly IncreasticEventIndicatorsAPI _api;

        public IncreasticEventIndicatorsClient(string serviceUrl, ILog log)
        {
            _log = log;
            _api = new IncreasticEventIndicatorsAPI(new Uri(serviceUrl));
        }

        public void Dispose()
        {
            _api.Dispose();
        }

        public async Task<IsAliveResponse> IsAliveAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var response = await _api.IsAliveWithHttpMessagesAsync(cancellationToken: cancellationToken);
            ValidateResponse(response);
            return JsonConvert.DeserializeObject<IsAliveResponse>(JsonConvert.SerializeObject(response.Body));
        }

        public async Task AddDeltaAsync(IntrinsicEventIndicatorsColumnPost column, CancellationToken cancellationToken = default(CancellationToken))
        {
            var response = await _api.ApiV1IntrinsicEventIndicatorsIntrinsiceventindicatorsdeltaPutWithHttpMessagesAsync(column, cancellationToken: cancellationToken);
            ValidateResponse(response);
        }

        public async Task RemoveDeltaAsync(string columnId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var response = await _api.ApiV1IntrinsicEventIndicatorsIntrinsiceventindicatorsdeltaDeleteWithHttpMessagesAsync(columnId, cancellationToken: cancellationToken);
            ValidateResponse(response);
        }

        public async Task AddAssetPairAsync(IntrinsicEventIndicatorsAssetPairPost row, CancellationToken cancellationToken = default(CancellationToken))
        {
            var response = await _api.ApiV1IntrinsicEventIndicatorsIntrinsiceventindicatorsassetpairPutWithHttpMessagesAsync(row, cancellationToken: cancellationToken);
            ValidateResponse(response);
        }

        public async Task RemoveAssetPairAsync(string rowId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var response = await _api.ApiV1IntrinsicEventIndicatorsIntrinsiceventindicatorsassetpairDeleteWithHttpMessagesAsync(rowId, cancellationToken: cancellationToken);
            ValidateResponse(response);
        }

        public async Task<IntrinsicEventIndicatorsDto> GetDataAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var response = await _api.ApiV1IntrinsicEventIndicatorsIntrinsiceventindicatorsdataGetWithHttpMessagesAsync(cancellationToken: cancellationToken);
            ValidateResponse(response);
            return response.Body;
        }

        public async Task<IDictionary<string, IList<RunnerStateDto>>> GetRunnersStatesAsync(
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var response = await _api.ApiV1IntrinsicEventIndicatorsIntrinsiceventindicatorsrunnersstatesGetWithHttpMessagesAsync(cancellationToken: cancellationToken);
            ValidateResponse(response);
            return response.Body;
        }

        private static void ValidateResponse<T>(IHttpOperationResponse<T> response, bool throwIfNotFound = true)
        {
            var error = response.Body as ErrorResponse;
            if (error != null)
            {
                throw new ApiException(error.ErrorMessage);
            }

            if (!response.Response.IsSuccessStatusCode)
            {
                ThrowIfErrorStatus(response.Response.StatusCode, response.Response.ReasonPhrase, throwIfNotFound);
            }
        }

        private static void ValidateResponse(IHttpOperationResponse response)
        {
            if (!response.Response.IsSuccessStatusCode)
            {
                ThrowIfErrorStatus(response.Response.StatusCode, response.Response.ReasonPhrase);
            }
        }

        private static void ThrowIfErrorStatus(HttpStatusCode statusCode, string errorMessage, bool throwIfNotFound = true)
        {
            switch (statusCode)
            {
                case HttpStatusCode.Conflict:
                    throw new ConflictException("Entity with the same key already exists.");
                case HttpStatusCode.NotFound:
                    if (throwIfNotFound)
                    {
                        throw new NotFoundException("Entity is not found.");
                    }
                    break;
                default:
                    throw new ApiException(errorMessage);
            }
        }
    }
}
