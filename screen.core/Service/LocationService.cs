using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Screen.Core.Model;
using System.Collections.Generic;
using System.Linq;

namespace Screen.Core.Service
{
    public class LocationService : ILocationService
    {
        private readonly IHubContext<InterfaceServiceHub> _interfaceServiceHub;
        public IFreeSql fsql { get; set; }
        public LocationService(IHubContext<InterfaceServiceHub> interfaceServiceHub, IFreeSql freeSql)
        {
            _interfaceServiceHub = interfaceServiceHub;
            fsql = freeSql;

        }

        public void Publish_ZJK_Data()
        {
            string sql = "select * from v_screen_data where zone_code = 'ZJK' ";
            List<LocationEntity> list = fsql.Ado.Query<LocationEntity>(sql);

            this._interfaceServiceHub.Clients.All.SendAsync
                ("zjk_data", "zjk_data", JsonConvert.SerializeObject(list, Formatting.Indented));

        }


        public void Publish_FJK_Data()
        {
            string sql = "select * from v_screen_data where zone_code = 'FJK' ";
            List<LocationEntity> list = fsql.Ado.Query<LocationEntity>(sql);

            this._interfaceServiceHub.Clients.All.SendAsync
            ("fjk_data", "fjk_data", JsonConvert.SerializeObject(list, Formatting.Indented));

        }


        public void Pulish_LaneWayEmpty_Data()
        {

            string sql = "select * from v_laneway_empty ";
            List<LaneWayEntity> list = fsql.Ado.Query<LaneWayEntity>(sql);

            this._interfaceServiceHub.Clients.All.SendAsync
            ("laneway_empty", "laneway_empty", JsonConvert.SerializeObject(list, Formatting.Indented));

        }

        /// <summary>
        /// 当日任务物料明细
        /// </summary>
        public void Publish_TaskItem_Data()
        {

            string sql = "select * from cwms.v_screen_taskitem ";
            List<TaskEntity> list = fsql.Ado.Query<TaskEntity>(sql);
            List<TaskItemEntity> item_list = new List<TaskItemEntity>();

            //入库 
            item_list.AddRange(GetTaskItemEntity(list, "inbound", "入库数量"));

            //出库 
            item_list.AddRange(GetTaskItemEntity(list, "outbound", "出库数量"));

            //待捡 
            item_list.AddRange(GetTaskItemEntity(list, "qc", "待捡物料"));

            //呆滞 
            item_list.AddRange(GetTaskItemEntity(list, "expire", "呆滞物料"));

            this._interfaceServiceHub.Clients.All.SendAsync
            ("task_item", "task_item", JsonConvert.SerializeObject(item_list, Formatting.Indented));

        }


        public List<TaskItemEntity> GetTaskItemEntity(List<TaskEntity> list, string type, string title)
        {
            TaskItemEntity taskItemEntity = new TaskItemEntity();
            List<TaskItemEntity> result_items = new List<TaskItemEntity>();

            string[] zones = new string[] { "ZJK", "FJK" };

            foreach (var zone_code in zones)
            {

                var inbound = list.Where(p => p.type == type && p.zone_code == zone_code).ToList();
                if (inbound.Count() == 0)
                {
                    taskItemEntity = new TaskItemEntity() { count = 0, title = title, zone_code = zone_code };
                }
                else
                {
                    foreach (var item in inbound)
                    {
                        taskItemEntity = new TaskItemEntity() { count = item.count, title = item.title, zone_code = zone_code };
                    }
                }
                result_items.Add(taskItemEntity);

            }



            return result_items;
        }

        public void Publish_EquTask_Data()
        {

            string sql = "select * from cwms.v_screen_equ_task_count ";
            List<EquipTaskEntity> list = fsql.Ado.Query<EquipTaskEntity>(sql);

            this._interfaceServiceHub.Clients.All.SendAsync
            ("equ_task", "equ_task", JsonConvert.SerializeObject(list, Formatting.Indented));

            sql = "select * from cwms.v_screen_inventory_zjk ";
            List<TaskEntity> list_inventory = fsql.Ado.Query<TaskEntity>(sql);

            this._interfaceServiceHub.Clients.All.SendAsync
            ("inventory_data_zjk", "inventory_data_zjk", JsonConvert.SerializeObject(list_inventory, Formatting.Indented));

            sql = "select * from cwms.v_screen_inventory_fjk ";
            list_inventory = fsql.Ado.Query<TaskEntity>(sql);

            this._interfaceServiceHub.Clients.All.SendAsync
            ("inventory_data_fjk", "inventory_data_fjk", JsonConvert.SerializeObject(list_inventory, Formatting.Indented));


            //========================
            sql = "select * from cwms.v_screen_storged ";
            var storged_list = fsql.Ado.Query<StorgedEntity>(sql);

            this._interfaceServiceHub.Clients.All.SendAsync
            ("screen_storged", "screen_storged", JsonConvert.SerializeObject(storged_list, Formatting.Indented));



        }



    }
}
