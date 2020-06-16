﻿// Copyright (c) .NET Core Community. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using  DotNetFramework.CAP.Core.Logger;

namespace DotNetFramework.CAP
{
    /// <summary>
    /// Default implement of <see cref="T:DotNetFramework.CAP.IBootstrapper" />.
    /// </summary>
    internal class DefaultBootstrapper : IBootstrapper
    {
        private readonly ILogger<DefaultBootstrapper> _logger;

        public DefaultBootstrapper(
            ILogger<DefaultBootstrapper> logger,
            IStorage storage,
            IEnumerable<IProcessingServer> processors)
        {
            _logger = logger;
            Storage = storage;
            Processors = processors;
        }

        private IStorage Storage { get; }

        private IEnumerable<IProcessingServer> Processors { get; }

        public async Task BootstrapAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug("### CAP background task is starting.");

            try
            {
                await Storage.InitializeAsync(stoppingToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Initializing the storage structure failed!");
            }

            stoppingToken.Register(() =>
            {
                _logger.LogDebug("### CAP background task is stopping.");

                foreach (var item in Processors)
                {
                    try
                    {
                        item.Dispose();
                    }
                    catch (OperationCanceledException ex)
                    {
                        _logger.ExpectedOperationCanceledException(ex);
                    }
                }
            });

            await BootstrapCoreAsync();

            _logger.LogInformation("### CAP started!");
        }

        protected virtual Task BootstrapCoreAsync()
        {
            foreach (var item in Processors)
            {
                try
                {
                    item.Start();
                }
                catch (Exception ex)
                {
                    _logger.ProcessorsStartedError(ex);
                }
            }

            return Task.FromResult(true);
        }

        protected  async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await BootstrapAsync(stoppingToken);
        }
    }
}