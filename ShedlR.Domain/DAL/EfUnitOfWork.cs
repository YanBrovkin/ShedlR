using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using ShedlR.Domain.Interfaces;
using ShedlR.Domain.Repository;

namespace ShedlR.Domain.DAL
{
    public class EfUnitOfWork: IEfUnitOfWork
    {
        private bool _disposed = false;
        private string _classname;
        private ILogger _logger;

        private IUnityContainer _container;
        private SRContext _context;


        public DbContext Session
        {
            get { return _context; }
        }

        public EfUnitOfWork()
        {
            try
            {
                _classname = this.ToString();
                _logger = DependencyResolver.Current.GetService<ILogger>();
                _container = new UnityContainer();
                _container.RegisterType(typeof(IEFRepository<>), typeof(EFGenericRepository<>), new HierarchicalLifetimeManager());
                _context = new SRContext();
                // запретим создавать proxy-классы для сущностей
                // чтобы избежать проблем при сериализации объектов
                _context.Configuration.ProxyCreationEnabled = false;

                // Отключим неявную "ленивую" загрузку
                // (избежим проблемы с сериализацией)
                _context.Configuration.LazyLoadingEnabled = false;

            }
            catch (Exception ex)
            {
                string methodname = System.Reflection.MethodBase.GetCurrentMethod().Name;
                _logger.Error(ex.Message, classname: _classname, methodname: methodname);
            }
        }

        public TRepository Get<TRepository>() where TRepository : IRepository
        {
            var repository = _container.Resolve<TRepository>(new ParameterOverride("context", _context));
            return repository;
        }

        public void Commit()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                string methodname = System.Reflection.MethodBase.GetCurrentMethod().Name;
                _logger.Error(ex.Message, classname: _classname, methodname: methodname);
            }
        }

        public void RollBack()
        {
            foreach (DbEntityEntry entry in _context.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    // Under the covers, changing the state of an entity from  
                    // Modified to Unchanged first sets the values of all  
                    // properties to the original values that were read from  
                    // the database when it was queried, and then marks the  
                    // entity as Unchanged. This will also reject changes to  
                    // FK relationships since the original value of the FK  
                    // will be restored. 
                    case System.Data.Entity.EntityState.Modified:
                        entry.State = System.Data.Entity.EntityState.Unchanged;
                        break;
                    case System.Data.Entity.EntityState.Added:
                        entry.State = System.Data.Entity.EntityState.Detached;
                        break;
                    // If the EntityState is the Deleted, reload the date from the database.   
                    case System.Data.Entity.EntityState.Deleted:
                        entry.Reload();
                        break;
                    default: break;
                }
            } 
        }

        // Используем финализатор
        ~EfUnitOfWork()
        {
            // Просто вызываем Dispose(false).
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Очистка управляемых объектов.
                    _context.Dispose();
                    _container.Dispose();
                }

                // Очистка неуправляемых объектов
                // Устанока в null больших полей объекта
                _disposed = true;
            }
        }

    }
}
