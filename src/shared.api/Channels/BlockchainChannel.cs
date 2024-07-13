using System.Threading.Channels;

namespace shared.api.Channels
{
    public interface IChannel<TMessage>
    {
        Task AddItemInQueueAsync(TMessage item);
        Task<TMessage> DequeueItemAsync(CancellationToken ct);
    }

    public class BlockchainChannel<TMessage> : IChannel<TMessage> where TMessage : class
    {
        private readonly Channel<TMessage> _queue;

        public BlockchainChannel()
        => _queue = Channel.CreateUnbounded<TMessage>(new UnboundedChannelOptions { SingleReader = true });

        public async Task AddItemInQueueAsync(TMessage item)
        => await _queue.Writer.WriteAsync(item);

        public async Task<TMessage> DequeueItemAsync(CancellationToken ct)
        => await _queue.Reader.ReadAsync(ct);
    }
}
