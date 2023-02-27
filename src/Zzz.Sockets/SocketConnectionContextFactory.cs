﻿using Microsoft.Extensions.Logging;
using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Zzz.Connections;
using Zzz.Sockets.Internal;

namespace Zzz.Sockets
{
    public sealed class SocketConnectionContextFactory : IDisposable
    {
        private readonly SocketConnectionFactoryOptions _options;
        private readonly ILogger _logger;
        private readonly int _settingsCount;
        private readonly QueueSettings[] _settings;

        // long to prevent overflow
        private long _settingsIndex;

        /// <summary>
        /// Creates the <see cref="SocketConnectionContextFactory"/>.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="logger">The logger.</param>
        public SocketConnectionContextFactory(SocketConnectionFactoryOptions options, ILogger logger)
        {
            ArgumentNullException.ThrowIfNull(options);
            ArgumentNullException.ThrowIfNull(logger);

            _options = options;
            _logger = logger;
            _settingsCount = _options.IOQueueCount;

            var maxReadBufferSize = _options.MaxReadBufferSize ?? 0;
            var maxWriteBufferSize = _options.MaxWriteBufferSize ?? 0;
            var applicationScheduler = options.UnsafePreferInlineScheduling ? PipeScheduler.Inline : PipeScheduler.ThreadPool;

            if (_settingsCount > 0)
            {
                _settings = new QueueSettings[_settingsCount];

                for (var i = 0; i < _settingsCount; i++)
                {
                    var memoryPool = _options.MemoryPoolFactory();
                    var transportScheduler = options.UnsafePreferInlineScheduling ? PipeScheduler.Inline : new IOQueue();

                    _settings[i] = new QueueSettings()
                    {
                        Scheduler = transportScheduler,
                        InputOptions = new PipeOptions(memoryPool, applicationScheduler, transportScheduler, maxReadBufferSize, maxReadBufferSize / 2, useSynchronizationContext: false),
                        OutputOptions = new PipeOptions(memoryPool, transportScheduler, applicationScheduler, maxWriteBufferSize, maxWriteBufferSize / 2, useSynchronizationContext: false),
                        SocketSenderPool = new SocketSenderPool(PipeScheduler.Inline),
                        MemoryPool = memoryPool,
                    };
                }
            }
            else
            {
                var memoryPool = _options.MemoryPoolFactory();
                var transportScheduler = options.UnsafePreferInlineScheduling ? PipeScheduler.Inline : PipeScheduler.ThreadPool;

                _settings = new QueueSettings[]
                {
                new QueueSettings()
                {
                    Scheduler = transportScheduler,
                    InputOptions = new PipeOptions(memoryPool, applicationScheduler, transportScheduler, maxReadBufferSize, maxReadBufferSize / 2, useSynchronizationContext: false),
                    OutputOptions = new PipeOptions(memoryPool, transportScheduler, applicationScheduler, maxWriteBufferSize, maxWriteBufferSize / 2, useSynchronizationContext: false),
                    SocketSenderPool = new SocketSenderPool(PipeScheduler.Inline),
                    MemoryPool = memoryPool,
                }
                };
                _settingsCount = 1;
            }
        }

        /// <summary>
        /// Create a <see cref="ConnectionContext"/> for a socket.
        /// </summary>
        /// <param name="socket">The socket for the connection.</param>
        /// <returns></returns>
        public ConnectionContext Create(Socket socket)
        {
            var setting = _settings[Interlocked.Increment(ref _settingsIndex) % _settingsCount];

            var connection = new SocketConnection(socket,
                setting.MemoryPool,
                setting.SocketSenderPool.Scheduler,
                _logger,
                setting.SocketSenderPool,
                setting.InputOptions,
                setting.OutputOptions,
                waitForData: _options.WaitForDataBeforeAllocatingBuffer);

            connection.Start();
            return connection;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            // Dispose any pooled senders and memory pools
            foreach (var setting in _settings)
            {
                setting.SocketSenderPool.Dispose();
                setting.MemoryPool.Dispose();
            }
        }

        private sealed class QueueSettings
        {
            public PipeScheduler Scheduler { get; init; } = default!;
            public PipeOptions InputOptions { get; init; } = default!;
            public PipeOptions OutputOptions { get; init; } = default!;
            public SocketSenderPool SocketSenderPool { get; init; } = default!;
            public MemoryPool<byte> MemoryPool { get; init; } = default!;
        }
    }
}