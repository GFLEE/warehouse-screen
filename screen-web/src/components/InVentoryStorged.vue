<template>
  <div class="storged-scroll-board">
    <dv-scroll-board :config="config" />
  </div>
</template>

<script>
import { connectServer } from "@/utils/signalR";
import bus from "@/utils/bus";
import Vue from "vue";
var v_type;
export default {
  name: "InVentoryStorged",
  data() {
    return {
      config: {
        header: ["料号", "批次", "质检", "数量"],
        data: [],
        align: ["center", "center", "center", "center"],
        rowNum: 6,
        // columnWidth: [100, 100, 50, 50],
        headerBGC: "#2c3e50",
        headerHeight: 30,
        oddRowBGC: "rgba(0, 44, 81, 0.8)",
        evenRowBGC: "rgba(10, 29, 50, 0.8)",
      },
    };
  },
  mounted() {
    v_type = Vue.prototype.$config.ZoneConfig;

    bus.$on("screen_storged", (data) => {
      const { config } = this;
      var data_arr = JSON.parse(data);

      data_arr = data_arr.filter((item) => item.zone_code.search(v_type) != -1);
      data_arr = data_arr.map(({ zone_code, ...e }) => e);

      data_arr.forEach(function (element) {
        element.item_code =
          "<span  class= " + "span_auto" + ">" + element.item_code + "</span>";
        if (
          element.lot_no == null ||
          element.lot_no == undefined ||
          element.lot_no == ""
        ) {
          // element.lot_no =
          //   "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
        }
        element.lot_no =
          "<span   class= " + "span_auto" + ">" + element.lot_no + "</span>";
        element.count =
          "<span   class= " + "span_auto" + ">" + element.count + "</span>";
        element.qc =
          "<span  class= " + "span_auto" + ">" + element.qc + "</span>";
      });

      this.config.data = data_arr;
      this.config = { ...this.config };
    });
  },
};
</script>

<style lang="less">
.storged-scroll-board {
  width: auto;
  box-sizing: border-box;
  height: 35%;
  overflow: hidden;
}
.storged-scroll-board .rows .ceil {
  width: auto;
}
.span_auto {
  font-size:0.9em
  // width: 100% !important;
  // float: left !important;
  // overflow: hidden !important;
  // text-overflow: ellipsis !important;
  // white-space: normal !important;
}
</style>
