using System.Buffers;

namespace Zzz.Buffers
{
    public static class PinnedBlockMemoryPoolFactory
    {
        public static MemoryPool<byte> Create()
        {
            return new PinnedBlockMemoryPool();
        }
    }
}