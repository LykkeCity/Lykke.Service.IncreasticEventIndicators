using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Service.IntrinsicEventIndicators.Client.AutorestClient;
using Lykke.Service.IntrinsicEventIndicators.Client.AutorestClient.Models;
using Lykke.Service.IntrinsicEventIndicators.Client.Exceptions;
using Microsoft.Rest;
using Newtonsoft.Json;

namespace Lykke.Service.IntrinsicEventIndicators.Client
{
    public class IntrinsicEventIndicatorsClient : IIntrinsicEventIndicatorsClient
    {
        private readonly ILog _log;
        private readonly IntrinsicEventIndicatorsAPI _api;

        public IntrinsicEventIndicatorsClient(string serviceUrl, ILog log)
        {
            _log = log;
            _api = new IntrinsicEventIndicatorsAPI(new Uri(serviceUrl));
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
            var response = await _api.ApiV1LykkeIntrinsicEventIndicatorsIntrinsiceventindicatorsdeltaPutWithHttpMessagesAsync(column, cancellationToken: cancellationToken);
            ValidateResponse(response);
        }

        public async Task RemoveDeltaAsync(string columnId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var response = await _api.ApiV1LykkeIntrinsicEventIndicatorsIntrinsiceventindicatorsdeltaDeleteWithHttpMessagesAsync(columnId, cancellationToken: cancellationToken);
            ValidateResponse(response);
        }

        public async Task AddAssetPairAsync(IntrinsicEventIndicatorsRowPost row, CancellationToken cancellationToken = default(CancellationToken))
        {
            var response = await _api.ApiV1LykkeIntrinsicEventIndicatorsIntrinsiceventindicatorsassetpairPutWithHttpMessagesAsync(row, cancellationToken: cancellationToken);
            ValidateResponse(response);
        }

        public async Task EditAssetPairAsync(IntrinsicEventIndicatorsRowEdit row, CancellationToken cancellationToken = default(CancellationToken))
        {
            var response = await _api.ApiV1LykkeIntrinsicEventIndicatorsIntrinsiceventindicatorsassetpairPostWithHttpMessagesAsync(row, cancellationToken: cancellationToken);
            ValidateResponse(response);
        }

        public async Task RemoveAssetPairAsync(string rowId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var response = await _api.ApiV1LykkeIntrinsicEventIndicatorsIntrinsiceventindicatorsassetpairDeleteWithHttpMessagesAsync(rowId, cancellationToken: cancellationToken);
            ValidateResponse(response);
        }

        public async Task<IntrinsicEventIndicatorsDto> GetDataAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var response = await _api.ApiV1LykkeIntrinsicEventIndicatorsIntrinsiceventindicatorsdataGetWithHttpMessagesAsync(cancellationToken: cancellationToken);
            ValidateResponse(response);
            return response.Body;
        }

        public async Task<IDictionary<string, IList<RunnerStateDto>>> GetRunnersStatesAsync(
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var response = await _api.ApiV1LykkeIntrinsicEventIndicatorsIntrinsiceventindicatorsrunnersstatesGetWithHttpMessagesAsync(cancellationToken: cancellationToken);
            ValidateResponse(response);
            return response.Body;
        }

        public async Task AddDeltaExternalAsync(IntrinsicEventIndicatorsColumnPost column, CancellationToken cancellationToken = default(CancellationToken))
        {
            var response = await _api.ApiV1ExternalIntrinsicEventIndicatorsIntrinsiceventindicatorsdeltaexternalPutWithHttpMessagesAsync(column, cancellationToken: cancellationToken);
            ValidateResponse(response);
        }

        public async Task RemoveDeltaExternalAsync(string columnId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var response = await _api.ApiV1ExternalIntrinsicEventIndicatorsIntrinsiceventindicatorsdeltaexternalDeleteWithHttpMessagesAsync(columnId, cancellationToken: cancellationToken);
            ValidateResponse(response);
        }

        public async Task AddAssetPairExternalAsync(IntrinsicEventIndicatorsRowPost row, CancellationToken cancellationToken = default(CancellationToken))
        {
            var response = await _api.ApiV1ExternalIntrinsicEventIndicatorsIntrinsiceventindicatorsassetpairexternalPutWithHttpMessagesAsync(row, cancellationToken: cancellationToken);
            ValidateResponse(response);
        }

        public async Task EditAssetPairExternalAsync(IntrinsicEventIndicatorsRowEdit row, CancellationToken cancellationToken = default(CancellationToken))
        {
            var response = await _api.ApiV1ExternalIntrinsicEventIndicatorsIntrinsiceventindicatorsassetpairexternalPostWithHttpMessagesAsync(row, cancellationToken: cancellationToken);
            ValidateResponse(response);
        }

        public async Task RemoveAssetPairExternalAsync(string rowId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var response = await _api.ApiV1ExternalIntrinsicEventIndicatorsIntrinsiceventindicatorsassetpairexternalDeleteWithHttpMessagesAsync(rowId, cancellationToken: cancellationToken);
            ValidateResponse(response);
        }

        public async Task<IntrinsicEventIndicatorsDto> GetDataExternalAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var response = await _api.ApiV1ExternalIntrinsicEventIndicatorsIntrinsiceventindicatorsdataexternalGetWithHttpMessagesAsync(cancellationToken: cancellationToken);
            ValidateResponse(response);
            return response.Body;
        }

        public async Task<IDictionary<string, IList<RunnerStateDto>>> GetRunnersStatesExternalAsync(
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var response = await _api.ApiV1ExternalIntrinsicEventIndicatorsIntrinsiceventindicatorsrunnersstatesexternalGetWithHttpMessagesAsync(cancellationToken: cancellationToken);
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
