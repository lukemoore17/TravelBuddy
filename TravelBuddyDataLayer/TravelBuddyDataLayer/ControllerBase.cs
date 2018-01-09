using System;

namespace TravelBuddyDataLayer
{
    public class ControllerBase : IDisposable
    {
        protected RepositoryBase _repository = null;

        ///////////////////////////////////////////////////////////////////////////////////////////////
        //
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _repository.Dispose();
                }
            }

            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
