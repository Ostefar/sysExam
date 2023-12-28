using System;
using System.Threading;
using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using UserApi.Data;
using UserApi.Models;
using RabbitMQ.Client.Logging;
using SharedModels;

namespace UserApi.Infrastructure
{
    public class MessageListener
    {
        IServiceProvider provider;
        string connectionString;

        // The service provider is passed as a parameter, because the class needs
        // access to the product repository. With the service provider, we can create
        // a service scope that can provide an instance of the product repository.
        public MessageListener(IServiceProvider provider, string connectionString)
        {
            this.provider = provider;
            this.connectionString = connectionString;
        }

        public void Start()
        {
            using (var bus = RabbitHutch.CreateBus(connectionString))
            {
                bus.PubSub.Subscribe<TaskStatusChangedMessage>("taskTrackerApiToDo",
                    HandleTaskToDo, x => x.WithTopic("todo"));

                bus.PubSub.Subscribe<TaskStatusChangedMessage>("taskTrackerApiDoing",
                    HandleTaskDone, x => x.WithTopic("done"));

                bus.PubSub.Subscribe<TaskStatusChangedMessage>("taskTrackerApiDone",
                   HandleTaskDoing, x => x.WithTopic("doing"));

                bus.PubSub.Subscribe<TaskStatusChangedMessage>("taskTrackerApiThrown",
                    HandleTaskThrown, x => x.WithTopic("thrown"));


                // Block the thread so that it will not exit and stop subscribing.
                lock (this)
                {
                    Monitor.Wait(this);
                }
            }

        }
        private async void HandleTaskToDo(TaskStatusChangedMessage message)
        {
            // this should maybe do something different, ill return on that
            using (var scope = provider.CreateScope())
            {
                var services = scope.ServiceProvider;
                var userRepos = services.GetService<IRepository<MyUser>>();

                var user = await userRepos.GetAsync(message.UserId);
                user.TasksToDo = -1;
                user.TasksDoing = +1;


                await userRepos.EditAsync(user);
            }
        }

        private async void HandleTaskDoing(TaskStatusChangedMessage message)
        {
            using (var scope = provider.CreateScope())
            {
                var services = scope.ServiceProvider;
                var userRepos = services.GetService<IRepository<MyUser>>();

                var user = await userRepos.GetAsync(message.UserId);
                user.TasksToDo = -1;
                user.TasksDoing = +1;

           
                await userRepos.EditAsync(user);
            }
        }

        private async void HandleTaskDone(TaskStatusChangedMessage message)
        {
            using (var scope = provider.CreateScope())
            {
                var services = scope.ServiceProvider;
                var userRepos = services.GetService<IRepository<MyUser>>();

                var user = await userRepos.GetAsync(message.UserId);

                user.TasksDoing = -1;
                user.TasksDone = +1;

                await userRepos.EditAsync(user);
            }
        }

        private async void HandleTaskThrown(TaskStatusChangedMessage message)
        {
            using (var scope = provider.CreateScope())
            {
                var services = scope.ServiceProvider;
                var userRepos = services.GetService<IRepository<MyUser>>();

                var user = await userRepos.GetAsync(message.UserId);
                
                user.TasksToDo = -1;
                user.TasksDoing = +1;


                await userRepos.EditAsync(user);
            }
        }
    }
}