using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Quartz.Impl;
using Quartz;
using Quartz.Impl.Triggers;
using Newtonsoft.Json;
using Screen.Core.Service;
using Microsoft.Extensions.DependencyInjection;

namespace Screen.Core
{
    public class QuartzNetHostService : IHostedService
    {
        public static IScheduler CreateScheduler()
        {
            try
            {
                var schedulerFactory = new StdSchedulerFactory();
                GlobalSchdule.Schedule = schedulerFactory.GetScheduler().Result;

                int default_interval = int.Parse(ConfigurationHelper.GetByName("interval"));

                //正极库货位
                CreateJob<ZJK_LocationJob>("ZJK_LocationJob", default_interval);
                //负极库货位
                CreateJob<FJK_LocationJob>("FJK_LocationJob", default_interval);
                //巷道空货位数
                CreateJob<LaneWwayEmpty_Job>("LaneWwayEmpty_Job", default_interval);
                //物料任务
                CreateJob<TaskItem_Job>("TaskItem_Job", default_interval);

                //堆垛机任务数量
                CreateJob<Equ_Task_Job>("Equ_Task_Job", default_interval);

                GlobalSchdule.Schedule.Start();

                return GlobalSchdule.Schedule;

            } 
            catch (Exception e)
            {

            }
            return null;

        }

        public QuartzNetHostService()
        {

        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            CreateScheduler();


            return Task.CompletedTask;

        }


        public static IJobDetail CreateJob<T>(string triggerKey, int interval, bool IsCustomCron = false,
            string customCron = "0 0 1 * * ?") where T : IJob
        {
            string idf = Guid.NewGuid().ToString();
            DateTime dt = DateTime.Now.AddSeconds(10);
            DateTimeOffset dt_offset = DateTime.SpecifyKind(dt, DateTimeKind.Local);
            var jobDetail = JobBuilder.Create<T>()
            .WithIdentity(idf)
            .StoreDurably()
            .Build();
            var trigger = TriggerBuilder.Create()
                            .WithIdentity(triggerKey);

            if (IsCustomCron)
            {
                trigger = trigger.WithCronSchedule(customCron)
                .WithDescription(customCron);
            }
            else
            {
                trigger = trigger.WithSimpleSchedule((x) =>
                    {
                        x.WithIntervalInSeconds(interval).RepeatForever();
                    });
            }
            var t = trigger.StartAt(dt_offset)
                            .Build();
            GlobalSchdule.Schedule.ScheduleJob(jobDetail, t);
            return jobDetail;
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

    #region  jobs
    public class ZJK_LocationJob : IJob
    {
        public ILocationService _locationService;
        public ZJK_LocationJob()
        {
            _locationService = NtiIoc.ServiceProvider.GetService<ILocationService>();
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _locationService.Publish_ZJK_Data();
        }
    }


    public class LaneWwayEmpty_Job : IJob
    {
        public ILocationService _locationService;
        public LaneWwayEmpty_Job()
        {
            _locationService = NtiIoc.ServiceProvider.GetService<ILocationService>();
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _locationService.Pulish_LaneWayEmpty_Data();
        }
    }

    public class FJK_LocationJob : IJob
    {
        public ILocationService _locationService;
        public FJK_LocationJob()
        {
            _locationService = NtiIoc.ServiceProvider.GetService<ILocationService>();

        }

        public async Task Execute(IJobExecutionContext context)
        {
            _locationService.Publish_FJK_Data();

        }
    }


    public class TaskItem_Job : IJob
    {
        public ILocationService _locationService;
        public TaskItem_Job() 
        {
            _locationService = NtiIoc.ServiceProvider.GetService<ILocationService>();

        }

        public async Task Execute(IJobExecutionContext context)
        {
            _locationService.Publish_TaskItem_Data();

        }
    }
    public class Equ_Task_Job : IJob
    {
        public ILocationService _locationService;
        public Equ_Task_Job()
        {
            _locationService = NtiIoc.ServiceProvider.GetService<ILocationService>();

        }

        public async Task Execute(IJobExecutionContext context)
        {
            _locationService.Publish_EquTask_Data();
             
        }
    }
    
    #endregion

    public static class GlobalSchdule
    {
        public static IScheduler Schedule { get; set; }

        public static bool SetInterval(string triggerKey, int IntervalSeconds)
        {
            bool isSame = true;

            var t = GlobalSchdule.Schedule.GetTrigger(new TriggerKey(triggerKey)).Result as SimpleTriggerImpl;
            var t_interval = t.RepeatInterval.TotalSeconds.ToString();
            if (Convert.ToInt32(t_interval) != IntervalSeconds)
            {
                DateTime dt = DateTime.Now.AddSeconds(30);
                DateTimeOffset dt_offset = DateTime.SpecifyKind(dt, DateTimeKind.Local);

                var trigger = TriggerBuilder.Create()
                          .WithIdentity(triggerKey)
                          .WithSimpleSchedule((x) =>
                          {
                              x.WithIntervalInSeconds(IntervalSeconds).RepeatForever();
                          })
                          .StartAt(dt_offset)
                          .Build();
                GlobalSchdule.Schedule.RescheduleJob(new TriggerKey(triggerKey), trigger);

                isSame = false;

            }
            return isSame;
        }

        public static void ResetTrigger(string triggerKey, string dateTime, int IntervalSeconds)
        {
            try
            {
                var start_datetime = Convert.ToDateTime(dateTime);
                var offset = DateTime.SpecifyKind(start_datetime, DateTimeKind.Local);

                ITrigger trigger = TriggerBuilder
                                   .Create()
                                   .WithIdentity(triggerKey)
                                   .StartAt(offset)
                                    .WithSimpleSchedule((x) =>
                                    {
                                        x.WithIntervalInSeconds(IntervalSeconds).RepeatForever();
                                    })
                                   .Build();

                GlobalSchdule.Schedule.RescheduleJob(new TriggerKey(triggerKey), trigger);

            }
            catch (Exception ex)
            {

            }

        }


        public static void ResetTriggerByName(string key, int min, int second, int interval)
        {
            var year = DateTime.Now.Year;
            var mon = DateTime.Now.Month;
            var day = DateTime.Now.Day;
            var hour = DateTime.Now.Hour;

            DateTimeOffset dtNow = DateBuilder.DateOf(hour, min, second, day, mon, year);

            ITrigger trigger = TriggerBuilder
                               .Create()
                               .WithIdentity(key)
                               .StartAt(dtNow)
                                .WithSimpleSchedule((x) =>
                                { x.WithIntervalInSeconds(interval).RepeatForever(); })
                               .Build();

            GlobalSchdule.Schedule.RescheduleJob(new TriggerKey(key), trigger);

        }

    }


}
