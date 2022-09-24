<template>
  <div id="scroll-board">
    <ScreenTable :cols="cols" :data="dataList" :height="190" :isload="isLoad" />
  </div>
</template>

<script>
import ScreenTable from "./custom/ScreenTable.vue";
import { connectServer } from "@/utils/signalR";
import bus from "@/utils/bus";
import Vue from "vue";
var v_type;

export default {
  name: "NewLaneWayEmpty",
  components: {
    ScreenTable,
  },
  data() {
    return {
      active: "commodity",
      isLoad: false,
      cols: [
        { field: "sc", title: "设备名称", align: "center" },
        {
          field: "status",
          title: "设备状态",
          align: "center",
          fontColor: { 正常: "#9fe6b8", 故障: "#fb7293" },
        },
        { field: "count", title: "空位数量", align: "center", width: 90 },
      ],
      dataList: [],
    };
  },
  methods: {
    btnClick(val) {
      this.active = val;
      this.loadData();
    },
    loadData() {
      let params = {
        type: this.active,
      };
    },
  },
  mounted() {
    v_type = Vue.prototype.$config.ZoneConfig == "ZJK" ? "正极" : "负极";
    bus.$on("laneway_empty", (data) => {
      var data_arr = JSON.parse(data);
      data_arr = data_arr.filter((item) => item.sc.search(v_type) != -1);

      this.dataList = data_arr;
    });
  },
};
</script>

<style lang="less">
#scroll-board {
  width: 100%;
  box-sizing: border-box;
  height: 30%;
  overflow: hidden;
}
.span_auto {
  font-size: 5px;
}
</style>
