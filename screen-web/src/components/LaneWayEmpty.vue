<template>
  <div id="scroll-board">
    <dv-scroll-board :config="config" />
  </div>
</template>

<script>
import { connectServer } from "@/utils/signalR";
import bus from "@/utils/bus";
import Vue from "vue";
var v_type;

export default {
  name: "LaneWayEmpty",
  data() {
    return {
      config: {
        header: ["设备名称", "设备状态", "空位数量"],
        data: [],
        rowNum: 4,
        // columnWidth: [130, 100, 120],
        align: ["center", "center", "center"],
        headerBGC: "#2c3e50",
        headerHeight: 30,
        oddRowBGC: "rgba(0, 44, 81, 0.8)",
        evenRowBGC: "rgba(10, 29, 50, 0.8)",
      },
    };
  },
  mounted() {
    v_type = Vue.prototype.$config.ZoneConfig == "ZJK" ? "正极" : "负极";
    bus.$on("laneway_empty", (data) => {
      const { config } = this;
      var data_arr = JSON.parse(data);
      data_arr = data_arr.filter((item) => item.sc.search(v_type) != -1);
      data_arr.forEach(function (element) {
        var status = element.status;
        if (status == "正常") {
          element.status =
            '<span style="color:#9fe6b8; font-size:0.9em">' +
            element.status +
            "</span>";
        } else {
          element.status =
            '<span style="color:#fb7293; font-size:0.9em">' +
            element.status +
            "</span>";
        }

        element.sc = '<span style=" font-size:0.9em">' + element.sc + "</span>";
        element.count =
          '<span style="  font-size:0.9em">' + element.count + "</span>";
      });
      this.config.data = data_arr;
      this.config = { ...this.config };
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
