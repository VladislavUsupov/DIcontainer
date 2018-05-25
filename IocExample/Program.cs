using IocExample.Classes;
using Ninject;
using System;

namespace IocExample
{
    class Program
    {
        static void Main(string[] args)
        {
            //var logger = new ConsoleLogger();
            //var sqlConnectionFactory = new SqlConnectionFactory("SQL Connection", logger);
            //var createUserHandler = new CreateUserHandler(new UserService(new QueryExecutor(sqlConnectionFactory), new CommandExecutor(sqlConnectionFactory), new CacheService(logger, new RestClient("API KEY"))), logger);

            //createUserHandler.Handle();

            MainNinject();
            MainDependencyResolver();
        }



        static void MainNinject()
        {
            IKernel kernel = new StandardKernel();

            kernel.Bind<ILogger>().To<ConsoleLogger>();

            kernel.Bind<RestClient>()
                .ToConstructor(k => new RestClient("API_KEY"));

            kernel.Bind<IConnectionFactory>()
                .ToConstructor(k => new SqlConnectionFactory("SQL Connection", k.Inject<ILogger>()))
                .InSingletonScope();

            var createUserHandler = kernel.Get<CreateUserHandler>();

            createUserHandler.Handle();
        }



        static void MainDependencyResolver()
        {
            Resolver.Bind<ILogger, ConsoleLogger>();
            Resolver.Bind<UserService, UserService>();
            Resolver.Bind<QueryExecutor, QueryExecutor>();
            Resolver.Bind<CommandExecutor, CommandExecutor>();
            Resolver.Bind<CacheService, CacheService>();
            Resolver.Bind<CreateUserHandler, CreateUserHandler>();
            Resolver.Bind<IConnectionFactory>(new SqlConnectionFactory("SQL Connection", Resolver.Get<ILogger>()));
            Resolver.Bind<RestClient>(new RestClient("API_KEY"));

            var createUserHandler = Resolver.Get<CreateUserHandler>();
            createUserHandler.Handle();
            Console.ReadKey();
        }
    }

}
