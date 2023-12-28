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
                    HandleTaskMoved, x => x.WithTopic("moved"));

                // Block the thread so that it will not exit and stop subscribing.
                lock (this)
                {
                    Monitor.Wait(this);
                }
            }

        }
        private async void HandleTaskMoved(TaskStatusChangedMessage message)
        {
            using (var scope = provider.CreateScope())
            {
                var services = scope.ServiceProvider;
                var userRepos = services.GetService<IRepository<MyUser>>();

                var user = await userRepos.GetAsync(message.UserId);

                user.TasksMoved++;


                await userRepos.EditAsync(user);
            }
        }
    }
}