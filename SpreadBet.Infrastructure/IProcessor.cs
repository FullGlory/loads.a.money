namespace SpreadBet.Infrastructure
{
    /// <summary>
    /// Abstraction for a process that can be stopped and started
    /// </summary>
    public interface IProcessor
    {
        /// <summary>
        /// Start the process
        /// </summary>
        void Start();
        
        /// <summary>
        /// Stop the process
        /// </summary>
        void Stop();
    }
}
