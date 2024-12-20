using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WorkTimeSalary.Application.Interfaces;
using WorkTimeSalary.Infrastructure.DbContext;

namespace WorkTimeSalary.Application.Factories
{
    public class ServiceFactory : IServiceFactory
    {
        #region Collections
        private readonly Dictionary<Type, Func<object[], object>> _serviceFactories = new Dictionary<Type, Func<object[], object>>();

        //This list of array of objects is list of constructors.

        private List<object[]> _parametersForConstructors;
        #endregion

        #region For Dependecy Injection
        private readonly WorkTimeSalaryDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IServiceProvider _serviceProvider;
        #endregion

        private ParameterInfo[]? _parameters = null;

        public ServiceFactory(
            WorkTimeSalaryDbContext context,
            IMapper mapper,
            IServiceProvider serviceProvider)
        {
            _dbContext = context;
            _mapper = mapper;
            _serviceProvider = serviceProvider;
            RegisterParametersForConstructor();

            AutoRegisterService();
          
        }

        #region Registration
        private void AutoRegisterService()
        {
            var serviceTypes = Assembly.GetExecutingAssembly().GetTypes()
                        .Where(type => type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IService<>)) && !type.IsAbstract)
                        .ToList();

            foreach (var serviceType in serviceTypes)
            {
                RegisterService(serviceType);
            }
        }

        private void RegisterParametersForConstructor()
        {
            //Add new array of object if new(or changed) have different constructor.
            //The order of parameters should be the same as passed in the object constructor.
            _parametersForConstructors = new List<object[]>()
            {
                new object[] { _dbContext, _mapper },
            };
        }

        public void RegisterService(Type serviceType)
        {
            if (!serviceType.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IService<>)))
            {
                throw new ArgumentException($"{serviceType.Name} is not a subclass of IService<T>");
            }

            var constructorInfo = serviceType.GetConstructors();

            if (constructorInfo.Any())
            {
                foreach (var constructor in constructorInfo)
                {
                    if (constructor != null)
                    {
                        _serviceFactories[serviceType] = parameters => Activator.CreateInstance(serviceType, parameters);
                    }
                    else
                    {
                        throw new ArgumentException($"No suitable constructor found for {serviceType.Name}");
                    }
                }
            }
        }

        #endregion

        #region Get Service

        public T GetService<T>()
        {
            var actionType = typeof(IService<>).MakeGenericType(typeof(T));
            if (_serviceFactories.TryGetValue(actionType, out var factory))
            {
                return (T)factory(GetNeededConstructor(actionType.GetConstructors().FirstOrDefault()?.GetParameters()));
            }

            throw new ArgumentException($"Action type {actionType.Name} is not registered.");
        }

        public T GetServiceProvider<T>(Type serviceType)
        {
            var service = _serviceProvider.GetService(serviceType);
            if (service != null)
            {
                return (T)service;
            }
            throw new ArgumentException($"Service of type {typeof(T).Name} is not registered.");
        }

        private object[] GetNeededConstructor(ParameterInfo[] parameters)
        {
            return _parametersForConstructors.FirstOrDefault(p =>
                p.Length == parameters.Length &&
                parameters.All(param => p.Any(arg => param.ParameterType.IsAssignableFrom(arg.GetType()))));
        }

        #endregion
    }
}
