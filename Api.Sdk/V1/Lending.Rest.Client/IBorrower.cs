﻿// Code generated by Microsoft (R) AutoRest Code Generator 0.9.7.0
// Changes may cause incorrect behavior and will be lost if the code is regenerated.

using System.Threading;
using System.Threading.Tasks;
using DH.Lending.Borrower.Api.Sdk.V1.Lending.Rest.Client.Models;

namespace DH.Lending.Borrower.Api.Sdk.V1.Lending.Rest.Client
{
    public partial interface IBorrower
    {
        /// <param name='borrowerId'>
        /// Required.
        /// </param>
        /// <param name='cancellationToken'>
        /// Cancellation token.
        /// </param>
        Task<HttpOperationResponse<object>> DeleteByBorroweridWithOperationResponseAsync(string borrowerId, CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        
        /// <param name='borrowerId'>
        /// Required.
        /// </param>
        /// <param name='cancellationToken'>
        /// Cancellation token.
        /// </param>
        Task<HttpOperationResponse<BorrowerDto>> GetByBorroweridWithOperationResponseAsync(string borrowerId, CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        
        /// <param name='borrower'>
        /// Required.
        /// </param>
        /// <param name='cancellationToken'>
        /// Cancellation token.
        /// </param>
        Task<HttpOperationResponse<BorrowerDto>> PostByBorrowerWithOperationResponseAsync(BorrowerDto borrower, CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        
        /// <param name='borrower'>
        /// Required.
        /// </param>
        /// <param name='cancellationToken'>
        /// Cancellation token.
        /// </param>
        Task<HttpOperationResponse<BorrowerDto>> PutByBorrowerWithOperationResponseAsync(BorrowerDto borrower, CancellationToken cancellationToken = default(System.Threading.CancellationToken));
    }
}
