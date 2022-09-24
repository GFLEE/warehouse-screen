namespace Screen.Core.Service
{
    public interface ILocationService
    {
        /// <summary>
        /// 正极库货位数据
        /// </summary>
        void Publish_ZJK_Data();

        /// <summary>
        /// 负极库货位数据
        /// </summary>
        void Publish_FJK_Data();

        /// <summary>
        /// 巷道空货位数
        /// </summary>
        void Pulish_LaneWayEmpty_Data();
         

        /// <summary>
        /// 任务-物料数据
        /// </summary>
        void Publish_TaskItem_Data();

        /// <summary>
        /// 堆垛机任务
        /// </summary>
        void Publish_EquTask_Data();
        
    }
}
